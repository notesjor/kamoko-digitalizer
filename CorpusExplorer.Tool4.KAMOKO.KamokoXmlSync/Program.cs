using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Bcs.IO;
using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;
using CorpusExplorer.Tool4.KAMOKO.Model.Controller;
using CorpusExplorer.Tool4.KAMOKO.Model.Parser;

namespace CorpusExplorer.Tool4.KAMOKO.KamokoXmlSync
{
  internal class Program
  {
    private static string CalculateFileHash(string file)
    {
      var buffer = FileIO.ReadBytes(file);
      byte[] res;

      using (var sha1 = SHA1.Create())
      {
        res = sha1.ComputeHash(buffer);
      }

      return Convert.ToBase64String(res);
    }

    private static Dictionary<string, string> LoadHashes(string path)
    {
      path = Path.Combine(path, "hashes.csv");
      if (!File.Exists(path))
        return new Dictionary<string, string>();

      var lines = FileIO.ReadLines(path);

      return
        lines.Select(line => line.Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries))
             .Where(split => split.Length == 2)
             .ToDictionary(split => split[0], split => split[1]);
    }

    private static void Main(string[] args)
    {
      CorpusExplorerEcosystem.InitializeMinimal();
      Console.WriteLine(Configuration.GetDependencyPath("TreeTagger"));

      var path = args != null && args.Length > 0 && args[0].StartsWith("path:")
                   ? args[0].Replace("path:", "")
                   : @"C:\Projekte\KAMOKO\GIT\5 - KAMOKO-XML";

      var files = Directory.GetFiles(path, "*.kamoko.xml");

      var hashes = LoadHashes(path);

      // ReSharper disable LocalizableElement
      foreach (var file in files.Where(file => File.Exists(file) && !file.EndsWith(".emergency")))
      {
        Console.Write("{0}...", Path.GetFileNameWithoutExtension(file));
        var hash = CalculateFileHash(file);
        if (hashes.ContainsKey(file) && hashes[file] == hash)
        {
          Console.WriteLine("ALREADY DONE!");
          continue;
        }

        try
        {
          var controller = new KamokoController();
          controller.Load(file);
          controller.Export(
                            new SimpleKamokoTagger
                              {LanguageSelected = "Französisch", CorpusBuilder = new CorpusBuilderWriteDirect()},
                            file.Replace(".kamoko.xml", ".cec6"));

          if (hashes.ContainsKey(file))
            hashes[file] = CalculateFileHash(file);
          else
            hashes.Add(file, CalculateFileHash(file));
          SaveHashes(path, hashes);
        }
        catch (Exception ex)
        {
          Console.WriteLine("ERR!");
          Console.WriteLine(ex.Message);
          Console.WriteLine(ex.StackTrace);
          Console.WriteLine("-----");
          continue;
        }

        Console.WriteLine("OK!");
      }
    }

    private static void SaveHashes(string path, Dictionary<string, string> hashes)
    {
      try
      {
        var lines = hashes.Select(hash => $"{hash.Key}\t{hash.Value}").ToList();
        FileIO.Write(Path.Combine(path, "hashes.csv"), lines.ToArray());
      }
      catch
      {
      }
    }
  }
}