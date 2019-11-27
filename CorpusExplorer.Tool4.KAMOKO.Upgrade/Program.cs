using System;
using System.Collections.Generic;
using System.IO;
using jor.CorpusExplorer.Kamoko.RawAnnotate.Controler;

namespace CorpusExplorer.Tool4.KAMOKO.Upgrade
{
  internal class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      var files = new List<string>();
      files.AddRange(
                     Directory.GetFiles(@"C:\Projekte\KAMOKO\GIT\4 - XML-Dateien überprüft & angereichert\",
                                        "*.speaker.xml"));

      foreach (var file in files)
      {
        Console.WriteLine(file);
        try
        {
          LayerConversionControler.Start(
                                         file,
                                         Path.Combine(
                                                      @"C:\Projekte\KAMOKO\GIT\5 - KAMOKO-XML\",
                                                      Path.GetFileName(file).Replace(".speaker.xml", ".kamoko.xml")));
          Console.WriteLine("OK!");
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine();
      }

      Console.WriteLine("FERTIG!");
      Console.ReadLine();
    }
  }
}