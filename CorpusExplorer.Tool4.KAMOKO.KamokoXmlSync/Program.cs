using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Bcs.IO;
using CorpusExplorer.Tool4.KAMOKO.Controller;

namespace CorpusExplorer.Tool4.KAMOKO.KamokoXmlSync
{
  internal class Program
  {
    private static string CalculateFileHash(string file)
    {
      var buffer = FileIO.ReadBytes(file);
      byte[] res;

      using (var sha1 = SHA1.Create()) res = sha1.ComputeHash(buffer);

      return Convert.ToBase64String(res);
    }

    private static Dictionary<string, string> LoadHashes(string path)
    {
      path = Path.Combine(path, "hashes.csv");
      if (!File.Exists(path)) return new Dictionary<string, string>();

      var lines = FileIO.ReadLines(path);

      return
        lines.Select(line => line.Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries))
             .Where(split => split.Length == 2)
             .ToDictionary(split => split[0], split => split[1]);
    }

    private static void Main(string[] args)
    {
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
        if (hashes.ContainsKey(file) &&
            hashes[file] == hash)
        {
          Console.WriteLine("ALREADY DONE!");
          continue;
        }
        if (!hashes.ContainsKey(file)) hashes.Add(file, hash);

        try
        {
          var controller = new KamokoController();
          controller.Load(file);
          controller.Export(file.Replace(".kamoko.xml", ".cec5"));
        }
        catch
        {
          Console.WriteLine("ERR!");
          continue; // wichtig, da sonst der Hash gespeichert würde, wenn ein Fehler auftritt.
        }
        Console.WriteLine("OK!");

        hashes[file] = hash;
      }
      // ReSharper restore LocalizableElement

      SaveHashes(path, hashes);
    }

    private static void SaveHashes(string path, Dictionary<string, string> hashes)
    {
      var lines = hashes.Select(hash => string.Format("{0}\t{1}", hash.Key, hash.Value)).ToList();
      FileIO.Write(Path.Combine(path, "hashes.csv"), lines.ToArray());
    }
  }
}