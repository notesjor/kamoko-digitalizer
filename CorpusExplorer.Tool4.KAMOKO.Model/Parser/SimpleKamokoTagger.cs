#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bcs.IO;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Adapter.Layer;
using CorpusExplorer.Sdk.Model.Adapter.Layer.Abstract;
using CorpusExplorer.Sdk.Model.Extension;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.Helper;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.TreeTagger.LocatorStrategy;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.TreeTagger.LocatorStrategy.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Parser.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Parser
{
  public class SimpleKamokoTagger : AbstractKamokoTagger
  {
    /// <summary>
    ///   The _doc lemma.
    /// </summary>
    private readonly Dictionary<Guid, int[][]> _docLemma = new Dictionary<Guid, int[][]>();

    private readonly Dictionary<Guid, int[][]> _docOrignal = new Dictionary<Guid, int[][]>();

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

    /// <summary>
    ///   The _list lemma.
    /// </summary>
    private readonly ListOptimized<string> _listLemma = new ListOptimized<string>();

    private readonly ListOptimized<string> _listOriginal = new ListOptimized<string>();

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

    /// <summary>
    ///   The _tasks.
    /// </summary>
    private readonly List<Task> _tasks = new List<Task>();

    /// <summary>
    ///   The _meta.
    /// </summary>
    private Dictionary<Guid, Dictionary<string, object>> _meta =
      new Dictionary<Guid, Dictionary<string, object>>();


    /// <summary>
    ///   Initializes a new instance of the <see cref="Obsolet.Parser.TreeTagger" /> class.
    /// </summary>
    public SimpleKamokoTagger()
    {
      _listLemma = TaggerHelper.SetEmptyValue(_listLemma);
      _listPos = TaggerHelper.SetEmptyValue(_listPos);
      _listWort = TaggerHelper.SetEmptyValue(_listWort);
      _listSatz = TaggerHelper.SetStandardSentenceLayerValues(_listSatz); // wichtig: SetEmptyValue

      _listSpeaker.Add(" ");
      _listSpeaker.Add("Zustimmung");
      _listSpeaker.Add("Ablehnung");
      _listSpeaker.Add("Bedingte Zustimmung");

      _listOriginal.Add(" ");
      _listOriginal.Add("Original");
    }

    public override string DisplayName => "TreeTagger (Layer) / KAMOKO-Edition";

    public override void Execute()
    {
      if (Input.Count == 0)
        return;

      Tokenizer = new HighSpeedSpaceTokenizer(); //new TreeTaggerTokenizer { Language = LanguageSelected };

      ParseMetadata();
      StartTaggingProcess();

      Task.WaitAll(_tasks.ToArray());

      var layers = new List<AbstractLayerAdapter>
      {
        LayerAdapterWriteDirect.Create(_docLemma, _listLemma, "Lemma"),
        LayerAdapterWriteDirect.Create(_docPos, _listPos, "POS"),
        LayerAdapterWriteDirect.Create(_docSatz, _listSatz, "Satz"),
        LayerAdapterWriteDirect.Create(_docWort, _listWort, "Wort"),
        LayerAdapterWriteDirect.Create(_docOrignal, _listOriginal, "Original")
      };

      if (SpeakerCount >= 1)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker1,
                                                  _listSpeaker,
                                                  "Sprecher 1"));

      if (SpeakerCount >= 2)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker2,
                                                  _listSpeaker,
                                                  "Sprecher 2"));

      if (SpeakerCount >= 3)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker3,
                                                  _listSpeaker,
                                                  "Sprecher 3"));

      if (SpeakerCount >= 4)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker4,
                                                  _listSpeaker,
                                                  "Sprecher 4"));

      if (SpeakerCount >= 5)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker5,
                                                  _listSpeaker,
                                                  "Sprecher 5"));

      if (SpeakerCount >= 6)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker6,
                                                  _listSpeaker,
                                                  "Sprecher 6"));

      if (SpeakerCount >= 7)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker7,
                                                  _listSpeaker,
                                                  "Sprecher 7"));

      if (SpeakerCount >= 8)
        layers.Add(
                   LayerAdapterWriteDirect.Create(
                                                  _docSpeaker8,
                                                  _listSpeaker,
                                                  "Sprecher 8"));

      var corpus = CorpusBuilder
                  .Create(layers.Select(x => x.ToLayerState()), _meta, new Dictionary<string, object>(), null)
                  .FirstOrDefault();
      if (corpus == null)
        return;

      corpus.SetCorpusMetadata("KAMOKO", Serializer.SerializeToBase64String(KamokoProperty));
      Output.Enqueue(corpus);
    }

    // private object _taggerLock = new object();

    protected override string ExecuteTagger(string text)
    {
      // lock (_taggerLock)
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
                FileName = Path.Combine(LocatorStrategy.TreeTaggerRootDirectory, @"bin\tree-tagger.exe"),
                Arguments =
                  string.Format(
                                "-quiet -token -lemma -sgml -no-unknown \"" +
                                LocatorStrategy.ApplyLanguage(LanguageSelected) + "\" \"{0}\" \"{1}\"",
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
          catch (Exception)
          {
            return string.Empty;
          }
        }
      }
    }

    protected override string TextPostTaggerCleanup(string text)
    {
      return text.Replace("<EMPTY>", "[...]\t[...]\t[...]");
    }

    private string KamokoTextCleanup(string text)
    {
      text = text.Replace("–", "-");
      text = text.Replace("«", "\"");
      text = text.Replace("»", "\"");
      text = Regex.Replace(text, "[\u0000-\u0009]", string.Empty);
      text = Regex.Replace(text, "[\u000B-\u000C]", string.Empty);
      text = Regex.Replace(text, "[\u000E-\u001F]", string.Empty);
      return Regex.Replace(text, "[\u00FF-\uFFFF]", string.Empty);
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
    private void ParseDocument(Guid guid, ref string text)
    {
      var cacheWort = new Dictionary<string, int>();
      var cacheSatz = new Dictionary<string, int>();
      var cacheLemma = new Dictionary<string, int>();
      var cachePos = new Dictionary<string, int>();
      var cacheSpeaker = new Dictionary<string, int>();
      var cacheOrignial = new Dictionary<string, int>();

      var satzWort = new List<int>();
      var satzLemma = new List<int>();
      var satzPos = new List<int>();
      var satzSatz = new List<int>();
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

      var lines = text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
      foreach (var line in lines)
      {
        if (satzWort == null)
        {
          satzWort = new List<int>();
          satzLemma = new List<int>();
          satzPos = new List<int>();
          satzSatz = new List<int>();
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

        if (line.Length == 1 ||
            line[0]     == '<')
        {
          if (line.Length > 1)
            switch (line[1])
            {
              case '/':
                if (line == "</M>")
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

                break;
              case 'M': // M = Meinung
                speaker1 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S1B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S1Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker2 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S2B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S2Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker3 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S3B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S3Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker4 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S4B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S4Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker5 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S5B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S5Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker6 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S6B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S6Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker7 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S7B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S7Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                speaker8 = RequestIndex(
                                        cacheSpeaker,
                                        _listSpeaker,
                                        line.Contains("S8B")
                                          ? "Bedingte Zustimmung"
                                          : line.Contains("S8Z")
                                            ? "Zustimmung"
                                            : "Ablehnung");
                original = RequestIndex(
                                        cacheOrignial,
                                        _listOriginal,
                                        line.Contains("ORIGINAL") ? "Original" : " ");
                break;
            }

          continue;
        }

        var entries = line.Split(TaggerValueSeparator, StringSplitOptions.RemoveEmptyEntries);

        satzWort.Add(RequestIndex(cacheWort, _listWort, entries[0]));
        satzPos.Add(RequestIndex(cachePos, _listPos, entries[1]));
        satzLemma.Add(RequestIndex(cacheLemma, _listLemma, entries[2]));
        satzSpeaker1.Add(speaker1);
        satzSpeaker2.Add(speaker2);
        satzSpeaker3.Add(speaker3);
        satzSpeaker4.Add(speaker4);
        satzSpeaker5.Add(speaker5);
        satzSpeaker6.Add(speaker6);
        satzSpeaker7.Add(speaker7);
        satzSpeaker8.Add(speaker8);
        satzOriginal.Add(original);
        satzSatz.Add(entries[1][0] == '$' ? RequestIndex(cacheSatz, _listSatz, entries[0]) : satzNo);

        if (!satzende.Contains(entries[0]))
          continue;

        docWort.Add(satzWort.ToArray());
        docPos.Add(satzPos.ToArray());
        docLemma.Add(satzLemma.ToArray());
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
                       Configuration.ParallelOptions,
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

    private void StartTaggingProcess()
    {
      // act definiert wieviele Tokens maximal pro Turn gewählt werden.
      var act = TaggerContentLengthMax;

      while (Input.Count > 0)
      {
        // Überprüfe den Überlauf von act - ggf. setze die Grenzen neu
        act = act < TaggerContentLengthMin
                ? TaggerContentLengthMin
                : act > TaggerContentLengthMax
                  ? TaggerContentLengthMax
                  : act;

        GetDocumentClusters(act);

        while (InternQueue.Count > 0)
          Parallel.For(
                       0,
                       InternQueue.Count,
                       Configuration.ParallelOptions,
                       i =>
                       {
                         Dictionary<string, object>[] turn;
                         if (!InternQueue.TryDequeue(out turn))
                           return;

                         // TreeTagger
                         var text = TextPostTaggerCleanup(ExecuteTagger(KamokoTextCleanup(GenerateText(ref turn, out var guids))));
                         var tmp = text.Split(new[] {TaggerFileSeparator}, StringSplitOptions.RemoveEmptyEntries);
                         // mkpt - maximal korrekt geparste texte
                         var correct = text.EndsWith(TaggerFileSeparator) ? tmp.Length : tmp.Length - 1;

                         if (turn.Length == 1 &&
                             correct     == 0)
                           correct = 1;

                         // Vor der Parallelisierung
                         var @lock = new object();

                         Parallel.For(
                                      0,
                                      correct,
                                      Configuration.ParallelOptions,
                                      j =>
                                      {
                                        try
                                        {
                                          ParseDocument(guids[j], ref tmp[j]);
                                          lock (@lock)
                                          {
                                            tmp[j] = null; // wichtig zu Error-Erkennung
                                            turn[j].Clear();
                                            turn[j] = null; // wichtig zu Error-Erkennung
                                          }
                                        }
                                        catch (Exception ex)
                                        {
                                          InMemoryErrorConsole.Log(ex);
                                        }
                                      });

                         // Aktualisiere act je nach Fall - Wenn kein Fehler (==) dann vergrößere den Wert.
                         // Wenn ein Fehler auftritt, dann reduziere ihn ( /= 3 )
                         if (correct == turn.Length)
                         {
                           act *= 2;
                           return;
                         }

                         act /= 3;

                         // Wenn Tagger total versagt, breche das Dokument ab.
                         // Wenn nur ein Dokument in der Queue und dieses nicht bearbeitbar ist, verwerfe es.
                         if (act <= TaggerContentLengthMin ||
                             turn.Length == 1 && correct < 1)
                           return;

                         // Fehlerhafte Dokumente werden zurück in die Queue gestellt.        
                         for (var j = correct; j < turn.Length && j > 0; j++)
                           Input.Enqueue(turn[j]);
                         // Fehlerhafte Dokumente werden zurück in die Queue gestellt.        
                         foreach (var t in turn.Where(t => t != null))
                           Input.Enqueue(t);
                       });
      }
    }

    protected override AbstractLocatorStrategy LocatorStrategy => new LocatorStrategyParFile();
  }
}