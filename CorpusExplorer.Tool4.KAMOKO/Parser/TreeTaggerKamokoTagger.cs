#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bcs.IO;
using CorpusExplorer.Core.DocumentProcessing.Tagger.Helper;
using CorpusExplorer.Core.DocumentProcessing.Tagger.TreeTagger.Abstract;
using CorpusExplorer.Core.DocumentProcessing.Tagger.TreeTagger.Parameter;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Adapter.Corpus;
using CorpusExplorer.Sdk.Model.Adapter.Layer;
using CorpusExplorer.Sdk.Model.Adapter.Layer.Abstract;
using CorpusExplorer.Sdk.Model.Extension;
using CorpusExplorer.Tool4.KAMOKO.Model;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Parser
{
  public class TreeTaggerKamokoTagger : AbstractTreeTagger
  {
    /// <summary>
    ///   The _doc lemma.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docLemma = new Dictionary<Guid, int[][]>();

    private readonly Dictionary<Guid, int[][]> _docOrignal = new Dictionary<Guid, int[][]>();

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

    private readonly Dictionary<Guid, int[][]> _docSpeaker1 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker2 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker3 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker4 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker5 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker6 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker7 = new Dictionary<Guid, int[][]>();
    private readonly Dictionary<Guid, int[][]> _docSpeaker8 = new Dictionary<Guid, int[][]>();

    /// <summary>
    ///   The _doc wort.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docWort = new Dictionary<Guid, int[][]>();

    private readonly HashSet<string> _languageses = new HashSet<string>
    {
      "Französisch"
    };

    /// <summary>
    ///   The _list lemma.
    /// </summary>
    private readonly ListOptimized<string> _listLemma = new ListOptimized<string>();

    private readonly ListOptimized<string> _listOriginal = new ListOptimized<string>();

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

    private readonly ListOptimized<string> _listSpeaker = new ListOptimized<string>();

    /// <summary>
    ///   The _list wort.
    /// </summary>
    private readonly ListOptimized<string> _listWort = new ListOptimized<string>();

    /// <summary>
    ///   The _request index lock.
    /// </summary>
    private readonly object _requestIndexLock = new object();

    private readonly int _speakerCount;

    /// <summary>
    ///   The _tasks.
    /// </summary>
    private readonly List<Task> _tasks = new List<Task>();

    private string _languageSelected = "Französisch";

    /// <summary>
    ///   The _meta.
    /// </summary>
    private Dictionary<Guid, Dictionary<string, object>> _meta =
      new Dictionary<Guid, Dictionary<string, object>>();

    /// <summary>
    ///   Initializes a new instance of the <see cref="Obsolet.Parser.TreeTagger" /> class.
    /// </summary>
    public TreeTaggerKamokoTagger(int speakerCount)
    {
      _speakerCount = speakerCount;

      _listLemma = TaggerHelper.SetEmptyValue(_listLemma);
      _listPos = TaggerHelper.SetEmptyValue(_listPos);
      _listWort = TaggerHelper.SetEmptyValue(_listWort);
      _listSatz = TaggerHelper.SetStandardSentenceLayerValues(_listSatz);
      _listPhrase = TaggerHelper.SetEmptyValue(_listPhrase); // wichtig: SetEmptyValue

      _listSpeaker.Add(" ");
      _listSpeaker.Add("Zustimmung");
      _listSpeaker.Add("Ablehnung");
      _listSpeaker.Add("Bedingte Zustimmung");

      _listOriginal.Add(" ");
      _listOriginal.Add("Original");
    }

    public override string DisplayName { get { return "TreeTagger (Layer) / KAMOKO-Edition"; } }

    public override string InstallationPath { get { return "(NICHT WÄHLBAR - OPTIMIERTE VERSION)"; } set { } }
    public Course KamokoProperty { get; set; }

    public override IEnumerable<string> LanguagesAvailabel => _languageses;

    public override string LanguageSelected
    {
      get { return _languageSelected; }
      set
      {
        if (!_languageses.Contains(value))
          throw new NotSupportedException("LanguageSelected-value is not in List of LanguagesAvailabel");
        _languageSelected = value;
      }
    }

    public override void Execute()
    {
      if (Input.Count == 0)
        return;

      ParseMetadata();

      ParseScrapDocumentWithTreeTagger("tag-french.bat");

      Task.WaitAll(_tasks.ToArray());

      var layers = new List<AbstractLayerAdapter>
      {
        LayerAdapterSingleFile.Create(_docLemma, _listLemma, new Dictionary<string, object>(), "Lemma"),
        LayerAdapterSingleFile.Create(_docPhrase, _listPhrase, new Dictionary<string, object>(), "Phrase"),
        LayerAdapterSingleFile.Create(_docPos, _listPos, new Dictionary<string, object>(), "POS"),
        LayerAdapterSingleFile.Create(_docSatz, _listSatz, new Dictionary<string, object>(), "Satz"),
        LayerAdapterSingleFile.Create(_docWort, _listWort, new Dictionary<string, object>(), "Wort"),
        LayerAdapterSingleFile.Create(_docOrignal, _listOriginal, new Dictionary<string, object>(), "Original")
      };

      if (_speakerCount >= 1)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker1,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 1"));

      if (_speakerCount >= 2)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker2,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 2"));

      if (_speakerCount >= 3)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker3,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 3"));

      if (_speakerCount >= 4)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker4,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 4"));

      if (_speakerCount >= 5)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker5,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 5"));

      if (_speakerCount >= 6)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker6,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 6"));

      if (_speakerCount >= 7)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker7,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 7"));

      if (_speakerCount >= 8)
        layers.Add(
          LayerAdapterSingleFile.Create(
            _docSpeaker8,
            _listSpeaker,
            new Dictionary<string, object>(),
            "Sprecher 8"));

      var corpus = CorpusAdapterSingleFile.Create(
        OutputPath,
        Path.GetFileNameWithoutExtension(OutputPath),
        layers,
        _meta,
        null,
        null,
        false);
      corpus.Metadata.Add("KAMOKO", KamokoProperty);
      corpus.Save(OutputPath);

      Output.Enqueue(corpus);
    }

    protected override string ExecuteTagger(string text)
    {
      using (var fileOutput = new TemporaryFile(Configuration.TempPath))
      {
        using (var fileInput = new TemporaryFile(Configuration.TempPath))
        {
          FileIO.Write(fileInput.Path, text);

          try
          {
            var process = new Process
            {
              StartInfo =
              {
                FileName = Path.Combine(TreeTaggerLocator.TreeTaggerRootDirectory, @"bin\tree-tagger.exe"),
                Arguments =
                  string.Format(
                    "-quiet -token -lemma -sgml -no-unknown \"" +
                    TreeTaggerLocator.ParFile("Französisch") + "\" \"{0}\" \"{1}\"",
                    fileInput.Path,
                    fileOutput.Path),
                RedirectStandardError = false,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Hidden
              }
            };
            process.Start();
            process.WaitForExit();

            return FileIO.ReadText(fileOutput.Path, Configuration.Encoding);
          }
          catch
          {
            return string.Empty;
          }
        }
      }
    }

    /// <summary>
    ///   The parse document.
    /// </summary>
    /// <param name="guid">Document GUID</param>
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
      var cacheOrignial = new Dictionary<string, int>();

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
      var satzOriginal = new List<int>();


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
      var docOriginal = new List<int[]>();

      var satzende = new HashSet<string> {".", "!", "?"};

      var phrase = RequestIndex(cachePhrase, _listPhrase, " ");
      var speaker1 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker2 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker3 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker4 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker5 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker6 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker7 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var speaker8 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
      var original = RequestIndex(cacheOrignial, _listOriginal, " ");
      var satzNo = RequestIndex(cacheSatz, _listSatz, " ");

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
          satzOriginal = new List<int>();
        }

        if ((line.Length == 1) ||
            (line[0][0] == '<'))
        {
          if (line[0].Length > 1)
            switch (line[0][1])
            {
              case '/':
                if (line[0] == "</M>")
                {
                  speaker1 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker2 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker3 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker4 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker5 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker6 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker7 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  speaker8 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
                  original = RequestIndex(cacheOrignial, _listOriginal, " ");
                }
                else
                  phrase = RequestIndex(cachePhrase, _listPhrase, " ");
                break;
              case 'P':
                phrase = RequestIndex(cachePhrase, _listPhrase, "PC");
                break;
              case 'V':
                phrase = RequestIndex(cachePhrase, _listPhrase, "VC");
                break;
              case 'N':
                phrase = RequestIndex(cachePhrase, _listPhrase, "NC");
                break;
              case 'M': // M = Meinung
                speaker1 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S1B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S1Z") ? "Zustimmung" : "Ablehnung");
                speaker2 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S2B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S2Z") ? "Zustimmung" : "Ablehnung");
                speaker3 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S3B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S3Z") ? "Zustimmung" : "Ablehnung");
                speaker4 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S4B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S4Z") ? "Zustimmung" : "Ablehnung");
                speaker5 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S5B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S5Z") ? "Zustimmung" : "Ablehnung");
                speaker6 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S6B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S6Z") ? "Zustimmung" : "Ablehnung");
                speaker7 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S7B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S7Z") ? "Zustimmung" : "Ablehnung");
                speaker8 = RequestIndex(
                  cacheSpeaker,
                  _listSpeaker,
                  line[0].Contains("S8B")
                    ? "Bedingte Zustimmung"
                    : line[0].Contains("S8Z") ? "Zustimmung" : "Ablehnung");
                original = RequestIndex(
                  cacheOrignial,
                  _listOriginal,
                  line[0].Contains("ORIGINAL") ? "Original" : " ");
                break;
            }

          continue;
        }

        satzWort.Add(RequestIndex(cacheWort, _listWort, line[0]));
        satzPos.Add(RequestIndex(cachePos, _listPos, line[1]));
        satzLemma.Add(RequestIndex(cacheLemma, _listLemma, line[2]));
        satzPhrase.Add(phrase);
        satzSpeaker1.Add(speaker1);
        satzSpeaker2.Add(speaker2);
        satzSpeaker3.Add(speaker3);
        satzSpeaker4.Add(speaker4);
        satzSpeaker5.Add(speaker5);
        satzSpeaker6.Add(speaker6);
        satzSpeaker7.Add(speaker7);
        satzSpeaker8.Add(speaker8);
        satzOriginal.Add(original);
        satzSatz.Add(line[1][0] == '$' ? RequestIndex(cacheSatz, _listSatz, line[0]) : satzNo);

        if (!satzende.Contains(line[0]))
          continue;

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
        docOriginal.Add(satzOriginal.ToArray());
        docSatz.Add(satzSatz.ToArray());

        // Zur Sicherheit um Satzübergreifene Prasen zu verhindern.
        phrase = RequestIndex(cachePhrase, _listPhrase, " ");
        speaker1 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker2 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker3 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker4 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker5 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker6 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker7 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        speaker8 = RequestIndex(cacheSpeaker, _listSpeaker, " ");
        original = RequestIndex(cacheOrignial, _listOriginal, " ");

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
        satzOriginal = null;
        satzSatz = null;
      }

      _docWort.Add(guid, docWort.ToArray());
      _docLemma.Add(guid, docLemma.ToArray());
      _docPos.Add(guid, docPos.ToArray());
      _docPhrase.Add(guid, docPhrase.ToArray());
      _docSatz.Add(guid, docSatz.ToArray());
      _docSpeaker1.Add(guid, docSpeaker1.ToArray());
      _docSpeaker2.Add(guid, docSpeaker2.ToArray());
      _docSpeaker3.Add(guid, docSpeaker3.ToArray());
      _docSpeaker4.Add(guid, docSpeaker4.ToArray());
      _docSpeaker5.Add(guid, docSpeaker5.ToArray());
      _docSpeaker6.Add(guid, docSpeaker6.ToArray());
      _docSpeaker7.Add(guid, docSpeaker7.ToArray());
      _docSpeaker8.Add(guid, docSpeaker8.ToArray());
      _docOrignal.Add(guid, docOriginal.ToArray());
    }

    /// <summary>
    ///   The parse metadata.
    /// </summary>
    private void ParseMetadata()
    {
      var meta = new ConcurrentDictionary<Guid, Dictionary<string, object>>();

      Parallel.ForEach(
        Input,
        sdm =>
        {
          var dic = sdm.GetMetaDictionary().ToDictionary(entry => entry.Key, entry => entry.Value);
          var guid = sdm.Get("GUID", Guid.NewGuid());
          dic.Add("GUID", guid);
          meta.TryAdd(guid, dic);
        });

      _meta = meta.ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    ///   The parse scrap document with tree tagger.
    /// </summary>
    /// <param name="parserSkript">
    ///   The Tagger skript.
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
    private void ParseScrapDocumentWithTreeTagger(string parserSkript)
    {
      var enc = Encoding.UTF8;

      // Concat der Texte
      const string splitTag = "\r\n<ENDOFCORPUSEXPLORERFILE>\r\n";
      var tempPath = Path.Combine(Configuration.TempPath, "files.txt");
      var list = new ListOptimized<string> {".", "!", "?", ":", ";", ")", "]", "+"};

      const int max = 200000;
      const int min = 10;
      var act = max;

      while (Input.Count > 0)
      {
        var stb = new StringBuilder();
        var listI = new List<Dictionary<string, object>>();
        act = act < min ? min : act > max ? max : act;

        while ((stb.Length < act) &&
               (Input.Count > 0))
        {
          Dictionary<string, object> i;
          if (!Input.TryDequeue(out i))
            continue;
          var t = i.Get("Text", string.Empty);
          if (string.IsNullOrEmpty(t) ||
              (t.Length < min))
            continue;

          stb.Append(t);
          listI.Add(i);

          // überprüft ob das letzte Zeichen des Textes ein Satzzeichen ist, ansonsten hänge einen . an.
          if (!list.Contains(t[t.Length - 1].ToString(CultureInfo.InvariantCulture)))
            stb.Append(".");

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
        var tmp = str.Replace("\r>", ">").Split(new[] {splitTag}, StringSplitOptions.RemoveEmptyEntries);
        // mkpt - maximal korrekt geparste texte
        var mkpt = str.EndsWith(splitTag) ? tmp.Length : tmp.Length - 1;
        if ((listI.Count == 1) &&
            (mkpt == 0))
          mkpt = 1;

        var j = 0;
        for (; j < mkpt; j++)
        {
          var d = tmp[j];
          var guid = listI[j].Get("GUID", Guid.NewGuid());
          var daten =
            d.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
             .Select(line => line.Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries))
             .ToArray();
          _tasks.Add(
            Task.Factory.StartNew(
                  () =>
                  {
                    try
                    {
                      ParseDocument(guid, daten);
                    }
                    catch {}
                  }));
        }
        if (mkpt == listI.Count)
        {
          act += stb.Length - (stb.Length - act)/2;
          continue;
        }

        // Wenn TreeTagger total versagt, breche das Dokument ab.
        if (act <= min)
          continue;
        // Wenn nur ein Dokument in der Queue und dieses nicht bearbeitbar ist, verwerfe es.
        if ((listI.Count == 1) &&
            (mkpt < 1))
          continue;

        act /= 3;
        for (; j < listI.Count; j++)
          Input.Enqueue(listI[j]);
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
        return cache[data];

      var idx = RequestIndex(list, data);
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
      lock (_requestIndexLock)
      {
        var idx = list.IndexOf(data);
        if (idx > -1)
          return idx;

        list.Add(data);
        return list.IndexOf(data);
      }
    }

    protected override string TextPostTaggerCleanup(string text)
    {
      return text.Replace("<EMPTY>", "[...]\t[...]\t[...]");
    }

    /// <summary>
    ///   Uses the tree tagger.
    /// </summary>
    /// <param name="treeTaggerPath">
    ///   The tree tagger path.
    /// </param>
    /// <param name="parserSkript">
    ///   The Tagger skript.
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
          try
          {
            File.Delete(fout);
          }
          catch {}

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
  }
}