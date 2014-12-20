namespace CorpusExplorer.Sdk.Utils.DocumentProcessing.Parser
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Threading.Tasks;

  using CorpusExplorer.Sdk.Helper;
  using CorpusExplorer.Sdk.Model;
  using CorpusExplorer.Sdk.Model.Scraper;
  using CorpusExplorer.Sdk.Utils.DocumentProcessing.Parser.Abstract;

  public class TreeTaggerKamokoParser : AbstractParser
  {
    private readonly int _speakerCount;

    #region Fields

    /// <summary>
    ///   The _doc lemma.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docLemma = new Dictionary<Guid, int[][]>();

    /// <summary>
    ///   The _doc phrase.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docPhrase = new Dictionary<Guid, int[][]>();

    /// <summary>
    ///   The _doc pos.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docPos = new Dictionary<Guid, int[][]>();

    /// <summary>
    ///   The _doc satz.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docSatz = new Dictionary<Guid, int[][]>();

    /// <summary>
    ///   The _doc wort.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docWort = new Dictionary<Guid, int[][]>();


    private readonly Dictionary<Guid, int[][]> _docSpeaker1 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker2 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker3 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker4 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker5 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker6 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker7 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker8 = new Dictionary<Guid, int[][]>();


    /// <summary>
    ///   The _list lemma.
    /// </summary>
    private readonly ListOptimized<string> _listLemma = new ListOptimized<string>();

    /// <summary>
    ///   The _list phrase.
    /// </summary>
    private readonly ListOptimized<string> _listPhrase = new ListOptimized<string>();

    /// <summary>
    ///   The _list pos.
    /// </summary>
    private readonly ListOptimized<string> _listPos = new ListOptimized<string>();

    /// <summary>
    ///   The _list satz.
    /// </summary>
    private readonly ListOptimized<string> _listSatz = new ListOptimized<string>();

    /// <summary>
    ///   The _list wort.
    /// </summary>
    private readonly ListOptimized<string> _listWort = new ListOptimized<string>();

    private readonly ListOptimized<string> _listSpeaker = new ListOptimized<string>();

    /// <summary>
    ///   The _meta.
    /// </summary>
    private readonly Dictionary<Guid, Dictionary<string, object>> _meta =
      new Dictionary<Guid, Dictionary<string, object>>();

    /// <summary>
    ///   The _request index lock.
    /// </summary>
    private readonly object _requestIndexLock = new object();

    /// <summary>
    ///   The _tasks.
    /// </summary>
    private readonly List<Task> _tasks = new List<Task>();

    /// <summary>
    ///   The _docs.
    /// </summary>
    private List<ScrapDocument> _docs;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Obsolet.Parser.TreeTagger" /> class.
    /// </summary>
    public TreeTaggerKamokoParser(int speakerCount)
    {
      this._speakerCount = speakerCount;

      this._listLemma.Add(" ");
      this._listPos.Add(" ");
      this._listWort.Add(" ");

      this._listSatz.Add(" ");
      this._listSatz.Add(",");
      this._listSatz.Add(".");
      this._listSatz.Add("?");
      this._listSatz.Add("!");
      this._listSatz.Add(";");
      this._listSatz.Add(":");
      this._listSatz.Add("-");
      this._listSatz.Add("[");
      this._listSatz.Add("]");
      this._listSatz.Add("(");
      this._listSatz.Add(")");

      this._listPhrase.Add(" ");
      this._listPhrase.Add("NC");
      this._listPhrase.Add("PC");
      this._listPhrase.Add("VC");

      this._listSpeaker.Add(" ");
      this._listSpeaker.Add("Zustimmung");
      this._listSpeaker.Add("Ablehnung");
      this._listSpeaker.Add("Bedingte Zustimmung");
    }

    #endregion

    #region Public Properties

    public override string DisplayName
    {
      get
      {
        return "TreeTagger (Layer) / KAMOKO-Edition";
      }
    }

    public override bool IsReady
    {
      get
      {
        return this.ScraperResults != null && (!string.IsNullOrEmpty(this.CorpusName))
               && (!string.IsNullOrEmpty(this.TreeTaggerBatchFilename));
      }
    }

    public string TreeTaggerBatchFilename { get; set; }

    #endregion

    #region Public Methods and Operators

    public override Corpus Execute()
    {
      if (ScraperResults == null || !ScraperResults.Any())
      {
        return null;
      }

      this._docs = ScraperResults.ToList();

      this.ParseMetadata();

      this.ParseScrapDocumentWithTreeTagger(TreeTaggerBatchFilename, _docs);

      Task.WaitAll(this._tasks.ToArray());

      var layers = new List<Layer>
                     {
                       new Layer(this._docLemma, this._listLemma, new Dictionary<string, object>()) { Displayname  = "Lemma"},
                       new Layer(this._docPhrase, this._listPhrase, new Dictionary<string, object>()) {Displayname="Phrase"},
                       new Layer(this._docPos, this._listPos, new Dictionary<string, object>()){Displayname="POS"},
                       new Layer(this._docSatz, this._listSatz, new Dictionary<string, object>()){Displayname="Satz"},
                       new Layer(this._docWort, this._listWort, new Dictionary<string, object>()){Displayname="Wort"}
                     };

      if(1 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker1, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 1"});

      if(2 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker2, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 2"});

      if(3 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker3, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 3"});

      if(4 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker4, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 4"});

      if(5 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker5, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 5"});

      if(6 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker6, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 6"});

      if(7 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker7, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 7"});

      if(8 >= _speakerCount)
        layers.Add(new Layer(this._docSpeaker8, _listSpeaker, new Dictionary<string, object>()) { Displayname = "Sprecher 8"});

      return Corpus.Create(CorpusName, layers, this._meta);
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Uses the tree tagger.
    /// </summary>
    /// <param name="treeTaggerPath">
    ///   The tree tagger path.
    /// </param>
    /// <param name="parserSkript">
    ///   The parser skript.
    /// </param>
    /// <param name="f">
    ///   The f.
    /// </param>
    /// <param name="enc">
    /// </param>
    /// <returns>
    ///   System.String.
    /// </returns>
    private static string UseTreeTagger(string parserSkript, string f, Encoding enc)
    {
      try
      {
        var fout = f + ".out";

        if (File.Exists(fout))
        {
          try
          {
            File.Delete(fout);
          }
          catch
          {
          }
        }

        var process = new Process
                        {
                          StartInfo =
                            {
                              FileName =
                                Path.Combine(
                                  Configuration.ExternAppTreetaggerPath,
                                  "bin\\" + parserSkript),
                              Arguments = f + " > " + fout,
                              RedirectStandardError = true,
                              CreateNoWindow = true,
                              UseShellExecute = false,
                              RedirectStandardOutput = true,
                              WindowStyle = ProcessWindowStyle.Hidden,
                              StandardOutputEncoding = enc
                            }
                        };
        process.Start();
        //var outOut = process.StandardOutput.ReadToEnd();
        //var outErr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        string res;
        using (var fs = new FileStream(fout, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          var buffer = new byte[fs.Length];
          fs.Read(buffer, 0, buffer.Length);
          res = enc.GetString(buffer);
        }

        return res;
      }
      catch
      {
        return string.Empty;
      }
    }

    /// <summary>
    ///   The parse document.
    /// </summary>
    /// <param name="data">
    ///   The data.
    /// </param>
    /// <returns>
    ///   The <see cref="Guid" />.
    /// </returns>
    private void ParseDocument(Guid guid, string[][] data)
    {
      var cacheWort = new Dictionary<string, int>();
      var cacheSatz = new Dictionary<string, int>();
      var cachePhrase = new Dictionary<string, int>();
      var cacheLemma = new Dictionary<string, int>();
      var cachePos = new Dictionary<string, int>();
      var cacheSpeaker = new Dictionary<string, int>();

      var satzWort = new List<int>();
      var satzLemma = new List<int>();
      var satzPos = new List<int>();
      var satzSatz = new List<int>();
      var satzPhrase = new List<int>();
      var satzSpeaker1 = new List<int>();
      var satzSpeaker2 = new List<int>();
      var satzSpeaker3 = new List<int>();
      var satzSpeaker4 = new List<int>(); 
      var satzSpeaker5 = new List<int>();
      var satzSpeaker6 = new List<int>();
      var satzSpeaker7 = new List<int>();
      var satzSpeaker8 = new List<int>();


      var docWort = new List<int[]>();
      var docPos = new List<int[]>();
      var docLemma = new List<int[]>();
      var docSatz = new List<int[]>();
      var docPhrase = new List<int[]>();
      var docSpeaker1 = new List<int[]>();
      var docSpeaker2 = new List<int[]>();
      var docSpeaker3 = new List<int[]>();
      var docSpeaker4 = new List<int[]>();
      var docSpeaker5 = new List<int[]>();
      var docSpeaker6 = new List<int[]>();
      var docSpeaker7 = new List<int[]>();
      var docSpeaker8 = new List<int[]>();

      var satzende = new HashSet<string> { ".", "!", "?" };

      var phrase = this.RequestIndex(cachePhrase, this._listPhrase, " ");
      var speaker1 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker2 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker3 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker4 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker5 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker6 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker7 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var speaker8 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
      var satzNo = this.RequestIndex(cacheSatz, this._listSatz, " ");

      foreach (var line in data)
      {
        if (satzWort == null)
        {
          satzWort = new List<int>();
          satzLemma = new List<int>();
          satzPos = new List<int>();
          satzSatz = new List<int>();
          satzPhrase = new List<int>();
          satzSpeaker1 = new List<int>();
          satzSpeaker2 = new List<int>();
          satzSpeaker3 = new List<int>();
          satzSpeaker4 = new List<int>();
          satzSpeaker5 = new List<int>();
          satzSpeaker6 = new List<int>();
          satzSpeaker7 = new List<int>();
          satzSpeaker8 = new List<int>();
        }

        if (line.Length == 1 || line[0][0] == '<')
        {
          if (line[0].Length > 1)
          {
            switch (line[0][1])
            {
              case '/':
                if (line[0] == "</M>")
                {
                  speaker1 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker2 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker3 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker4 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker5 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker6 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker7 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                  speaker8 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
                }
                else
                {
                  phrase = this.RequestIndex(cachePhrase, this._listPhrase, " ");
                }
                break;
              case 'P':
                phrase = this.RequestIndex(cachePhrase, this._listPhrase, "PC");
                break;
              case 'V':
                phrase = this.RequestIndex(cachePhrase, this._listPhrase, "VC");
                break;
              case 'N':
                phrase = this.RequestIndex(cachePhrase, this._listPhrase, "NC");
                break;
              case 'M': // M = Meinung
                speaker1 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S1B") ? "Bedingte Zustimmung" : line[0].Contains("S1Z") ? "Zustimmung" : "Ablehnung");
                speaker2 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S2B") ? "Bedingte Zustimmung" : line[0].Contains("S2Z") ? "Zustimmung" : "Ablehnung");
                speaker3 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S3B") ? "Bedingte Zustimmung" : line[0].Contains("S3Z") ? "Zustimmung" : "Ablehnung");
                speaker4 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S4B") ? "Bedingte Zustimmung" : line[0].Contains("S4Z") ? "Zustimmung" : "Ablehnung");
                speaker5 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S5B") ? "Bedingte Zustimmung" : line[0].Contains("S5Z") ? "Zustimmung" : "Ablehnung");
                speaker6 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S6B") ? "Bedingte Zustimmung" : line[0].Contains("S6Z") ? "Zustimmung" : "Ablehnung");
                speaker7 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S7B") ? "Bedingte Zustimmung" : line[0].Contains("S7Z") ? "Zustimmung" : "Ablehnung");
                speaker8 = this.RequestIndex(cacheSpeaker, this._listSpeaker, line[0].Contains("S8B") ? "Bedingte Zustimmung" : line[0].Contains("S8Z") ? "Zustimmung" : "Ablehnung");
                break;
            }
          }

          continue;
        }

        satzWort.Add(this.RequestIndex(cacheWort, this._listWort, line[0]));
        satzPos.Add(this.RequestIndex(cachePos, this._listPos, line[1]));
        satzLemma.Add(this.RequestIndex(cacheLemma, this._listLemma, line[2]));
        satzPhrase.Add(phrase);
        satzSpeaker1.Add(speaker1);
        satzSpeaker2.Add(speaker2);
        satzSpeaker3.Add(speaker3);
        satzSpeaker4.Add(speaker4);
        satzSpeaker5.Add(speaker5);
        satzSpeaker6.Add(speaker6);
        satzSpeaker7.Add(speaker7);
        satzSpeaker8.Add(speaker8);
        satzSatz.Add(line[1][0] == '$' ? this.RequestIndex(cacheSatz, this._listSatz, line[0]) : satzNo);

        if (!satzende.Contains(line[0]))
        {
          continue;
        }

        docWort.Add(satzWort.ToArray());
        docPos.Add(satzPos.ToArray());
        docLemma.Add(satzLemma.ToArray());
        docPhrase.Add(satzPhrase.ToArray());
        docSpeaker1.Add(satzSpeaker1.ToArray());
        docSpeaker2.Add(satzSpeaker2.ToArray());
        docSpeaker3.Add(satzSpeaker3.ToArray());
        docSpeaker4.Add(satzSpeaker4.ToArray());
        docSpeaker5.Add(satzSpeaker5.ToArray());
        docSpeaker6.Add(satzSpeaker6.ToArray());
        docSpeaker7.Add(satzSpeaker7.ToArray());
        docSpeaker8.Add(satzSpeaker8.ToArray());
        docSatz.Add(satzSatz.ToArray());

        // Zur Sicherheit um Satzübergreifene Prasen zu verhindern.
        phrase = this.RequestIndex(cachePhrase, this._listPhrase, " ");
        speaker1 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker2 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker3 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker4 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker5 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker6 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker7 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");
        speaker8 = this.RequestIndex(cacheSpeaker, this._listSpeaker, " ");

        satzWort = null;
        satzPos = null;
        satzLemma = null;
        satzPhrase = null;
        satzSpeaker1 = null;
        satzSpeaker2 = null;
        satzSpeaker3 = null;
        satzSpeaker4 = null;
        satzSpeaker5 = null;
        satzSpeaker6 = null;
        satzSpeaker7 = null;
        satzSpeaker8 = null;
        satzSatz = null;
      }

      this._docWort.Add(guid, docWort.ToArray());
      this._docLemma.Add(guid, docLemma.ToArray());
      this._docPos.Add(guid, docPos.ToArray());
      this._docPhrase.Add(guid, docPhrase.ToArray());
      this._docSatz.Add(guid, docSatz.ToArray());
      this._docSpeaker1.Add(guid, docSpeaker1.ToArray());
      this._docSpeaker2.Add(guid, docSpeaker2.ToArray());
      this._docSpeaker3.Add(guid, docSpeaker3.ToArray());
      this._docSpeaker4.Add(guid, docSpeaker4.ToArray());
      this._docSpeaker5.Add(guid, docSpeaker5.ToArray());
      this._docSpeaker6.Add(guid, docSpeaker6.ToArray());
      this._docSpeaker7.Add(guid, docSpeaker7.ToArray());
      this._docSpeaker8.Add(guid, docSpeaker8.ToArray());
    }

    /// <summary>
    ///   The parse metadata.
    /// </summary>
    private void ParseMetadata()
    {
      foreach (var sdm in this._docs)
      {
        var dic = new Dictionary<string, object>();

        var guid = sdm.Get("GUID", Guid.NewGuid());
        dic.Add("GUID", guid);
        dic.Add("Titel", sdm.Get("Titel", guid.ToString()));
        dic.Add("Autor", sdm.Get("Autor", "Kein Autor"));
        dic.Add("Datum", sdm.Get("Datum", DateTime.MinValue));
        dic.Add("Publikationsart", sdm.Get("Publikationsart", "Keine Zuordnung"));
        dic.Add("Rubrik", sdm.Get("Rubrik", "Keine Zuordnung"));
        dic.Add("Zeitung", sdm.Get("Zeitung", "Keine Zuordnung"));

        this._meta.Add(guid, dic);
      }
    }

    /// <summary>
    ///   The parse scrap document with tree tagger.
    /// </summary>
    /// <param name="parserSkript">
    ///   The parser skript.
    /// </param>
    /// <param name="docs">
    ///   The docs.
    /// </param>
    /// <param name="treeTaggerPath">
    ///   The tree tagger path.
    /// </param>
    /// <param name="enc">
    ///   The enc.
    /// </param>
    private void ParseScrapDocumentWithTreeTagger(string parserSkript, IEnumerable<ScrapDocument> docs)
    {
      var enc = Encoding.UTF8;

      // Concat der Texte
      const string splitTag = "\r\n<ENDOFCORPUSEXPLORERFILE>\r\n";
      var tempPath = Path.Combine(Configuration.TempPath, "files.txt");
      var list = new ListOptimized<string> { ".", "!", "?", ":", ";", ")", "]", "+" };

      var queue = new Queue<ScrapDocument>(docs);

      const int max = 200000;
      const int min = 10;
      var act = max;

      while (queue.Count > 0)
      {
        var stb = new StringBuilder();
        var listI = new List<ScrapDocument>();
        act = act < min ? min : act > max ? max : act;

        while (stb.Length < act && queue.Count > 0)
        {
          var i = queue.Dequeue();
          var t = i.Get("Text", string.Empty);
          if (string.IsNullOrEmpty(t) || t.Length < min)
          {
            continue;
          }

          stb.Append(t);
          listI.Add(i);

          // überprüft ob das letzte Zeichen des Textes ein Satzzeichen ist, ansonsten hänge einen . an.
          if (!list.Contains(t[t.Length - 1].ToString(CultureInfo.InvariantCulture)))
          {
            stb.Append(".");
          }

          stb.Append(splitTag);
        }

        stb.Replace("–", "-");
        stb.Replace("«", "\"");
        stb.Replace("»", "\"");

        var text = Regex.Replace(stb.ToString(), "[\u0000-\u0009]", string.Empty);
        text = Regex.Replace(text, "[\u000B-\u000C]", string.Empty);
        text = Regex.Replace(text, "[\u000E-\u001F]", string.Empty);
        text = Regex.Replace(text, "[\u00FF-\uFFFF]", string.Empty);

        using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
        {
          var buffer = enc.GetBytes(text);
          fs.Write(buffer, 0, buffer.Length);
        }

        // TreeTagger
        var str = UseTreeTagger(parserSkript, tempPath, enc);
        var tmp = str.Replace("\r>", ">").Split(new[] { splitTag }, StringSplitOptions.RemoveEmptyEntries);
        // mkpt - maximal korrekt geparste texte
        var mkpt = str.EndsWith(splitTag) ? tmp.Length : tmp.Length - 1;
        if (listI.Count == 1 && mkpt == 0)
        {
          mkpt = 1;
        }

        var j = 0;
        for (; j < mkpt; j++)
        {
          var d = tmp[j];
          var guid = listI[j].Get("GUID", Guid.NewGuid());
          var daten =
            d.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
              .Select(line => line.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries))
              .ToArray();
          _tasks.Add(
            Task.Factory.StartNew(
              () =>
              {
                try
                {
                  ParseDocument(guid, daten);
                }
                catch
                {
                }
              }));
        }
        if (mkpt == listI.Count)
        {
          act += stb.Length - ((stb.Length - act) / 2);
          continue;
        }

        // Wenn TreeTagger total versagt, breche das Dokument ab.
        if (act <= min)
        {
          continue;
        }
        // Wenn nur ein Dokument in der Queue und dieses nicht bearbeitbar ist, verwerfe es.
        if (listI.Count == 1 && mkpt < 1)
        {
          continue;
        }

        act /= 3;
        for (; j < listI.Count; j++)
        {
          queue.Enqueue(listI[j]);
        }
      }
    }

    /// <summary>
    ///   The request index.
    /// </summary>
    /// <param name="cache">
    ///   The cache.
    /// </param>
    /// <param name="list">
    ///   The list.
    /// </param>
    /// <param name="data">
    ///   The data.
    /// </param>
    /// <returns>
    ///   The <see cref="int" />.
    /// </returns>
    private int RequestIndex(Dictionary<string, int> cache, ListOptimized<string> list, string data)
    {
      if (cache.ContainsKey(data))
      {
        return cache[data];
      }

      var idx = this.RequestIndex(list, data);
      cache.Add(data, idx);
      return idx;
    }

    /// <summary>
    ///   The request index.
    /// </summary>
    /// <param name="list">
    ///   The list.
    /// </param>
    /// <param name="data">
    ///   The data.
    /// </param>
    /// <returns>
    ///   The <see cref="int" />.
    /// </returns>
    private int RequestIndex(ListOptimized<string> list, string data)
    {
      lock (this._requestIndexLock)
      {
        var idx = list.IndexOf(data);
        if (idx > -1)
        {
          return idx;
        }

        list.Add(data);
        return list.IndexOf(data);
      }
    }

    #endregion
  }
}
