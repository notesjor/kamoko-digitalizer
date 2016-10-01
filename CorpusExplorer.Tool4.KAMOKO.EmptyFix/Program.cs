using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CorpusExplorer.Tool4.KAMOKO.EmptyFix
{
  class Program
  {
    static void Main(string[] args)
    {
      var files = new List<string>();
      files.AddRange(Directory.GetFiles(@"C:\Projekte\KAMOKO\GIT\3 - XML-Dateien überprüft\", "*.xml"));
      files.AddRange(Directory.GetFiles(@"C:\Projekte\KAMOKO\GIT\4 - XML-Dateien überprüft & angereichert\", "*.xml"));
      files.AddRange(Directory.GetFiles(@"C:\Projekte\KAMOKO\GIT\5 - KAMOKO-XML\", "*.xml"));

      var res = new HashSet<string>();

      foreach (var file in files)
      {
        var text = File.ReadAllText(file);

        var idx = 0;
        while (true)
        {
          var next = text.IndexOf("EMPTY", idx);
          if (next == -1)
            break;

          var s = 0;
          while (next - s >= 0 && text[next - s] != ' ')
            s++;

          var e = 0;
          while (next + e < text.Length && text[next + e] != ' ')
            e++;

          res.Add(text.Substring(next - s, (next + e) - (next - s)).Trim());
          idx = next + e;
        }
      }

      var lines = res.ToArray();
      for (int i = 0; i < lines.Length; i++)
      {
        lines[i] += "\t" + lines[i];
      }

      if (File.Exists("empty.fix"))
        File.Delete("empty.fix");

      File.WriteAllLines("empty.txt", lines, Encoding.UTF8);

      Console.WriteLine("EMPTY erkannt - Bitte empty.fix bereistellen");
      Console.WriteLine("PRESS ENTER TO CONTINUE...");
      Console.ReadLine();

      var cli = File.ReadAllLines("empty.fix", Encoding.UTF8);
      var cor = (from l in cli select l.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries) into split where split.Length == 2 select new KeyValuePair<string, string>(split[0], split[1])).ToList(); // Muss so sein - Dictionary würde die Reihenfolge verändern.

      foreach (var file in files)
      {
        var text = File.ReadAllText(file, Encoding.UTF8);
        using (var md5 = MD5.Create())
        {
          var h1 = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(text)));

          foreach (var x in cor)
          {
            text = text.Replace(" " + x.Key + " ", " " + x.Value + " ");
            text = text.Replace("\r\n" + x.Key + " ", "\r\n" + x.Value + " ");
            text = text.Replace(" " + x.Key + "\r\n", " " + x.Value + "\r\n");
            text = text.Replace("\t" + x.Key + "\r\n", "\r\n" + x.Value + "\r\n");
          }

          var h2 = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(text)));

          if (h1 == h2)
            continue;
        }
        File.WriteAllText(file, text, Encoding.UTF8);
      }
    }
  }
}
