#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Sdk.Model.Extension;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.Abstract;
using CorpusExplorer.Sdk.ViewModel;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Parser;
using CorpusExplorer.Tool4.KAMOKO.Model.Parser.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Properties;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Controller
{
  public class KamokoController
  {
    private Course _model = new Course();
    private int _privateIndexPage;
    private int _privateIndexSentence;
    private List<Tuple<string, string, string>> _searchIndex = new List<Tuple<string, string, string>>();

    public IEnumerable<AbstractFragment> CurrentFragments
    {
      get
      {
        if (_model?.Documents                                                              == null                  ||
            _model.Documents.Count                                                         <= _privateIndexPage     ||
            _model.Documents[_privateIndexPage].Sentences                                  == null                  ||
            _model.Documents[_privateIndexPage].Sentences.Count                            <= _privateIndexSentence ||
            _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments == null)
          return new List<AbstractFragment>();
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments;
      }
    }

    public string CurrentPageIndex
    {
      get
      {
        if (_model?.Documents      == null ||
            _model.Documents.Count <= _privateIndexPage)
          return "0";
        return _model.Documents[_privateIndexPage].Index;
      }
    }

    public string CurrentPageIndexLabel => _model?.Documents == null
                                             ? "0 / 0"
                                             : $"{_privateIndexPage + 1} / {_model.Documents.Count}";

    public string CurrentSentenceIndex => _model?.Documents == null || _model.Documents.Count <= _privateIndexPage
                                                                    || _model.Documents[_privateIndexPage].Sentences ==
                                                                       null
                                                                    || _model.Documents[_privateIndexPage].Sentences
                                                                             .Count
                                                                    <= _privateIndexSentence
                                            ? "0"
                                            : _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence]
                                                    .Index;

    public string CurrentSentenceIndexLabel => _model?.Documents                             == null
                                            || _model.Documents.Count                        <= _privateIndexPage
                                            || _model.Documents[_privateIndexPage].Sentences == null
                                                 ? "0 / 0"
                                                 : $"{_privateIndexSentence + 1} / {_model.Documents[_privateIndexPage].Sentences.Count}";

    public string CurrentSource
    {
      get => _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source;
      set => _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source = value;
    }

    public bool IsReadOnly { get; set; }

    public bool IsReady => _model != null;

    public IEnumerable<string> LanguagesAvailabel => new TreeTaggerKamokoTagger(0).LanguagesAvailabel;

    public string LanguageSelected { get; set; } = "Französisch";

    public string SavePath { get; set; }

    public int SentenceCombinationMaximum { get; set; } = 1000;

    public void AddFragment(AbstractFragment previewFragment, AbstractFragment newFragment)
    {
      if (IsReadOnly)
        return;

      var idx =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.FindIndex(
                                                                                                 x =>
                                                                                                   x.Index ==
                                                                                                   previewFragment
                                                                                                    .Index);

      if (idx == -1)
        return;

      // Modifikation von newFragment zur Sicherung der Index-Integrität
      newFragment.Index =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.Max(x => x.Index) + 1;

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.Add(newFragment);
    }

    public void AddPage()
    {
      if (IsReadOnly)
        return;

      _model.Documents.Add(
                           new Document
                           {
                             Index = (_model.Documents.Count + 1).ToString(),
                             Sentences =
                               new List<Sentence>
                               {
                                 new Sentence
                                 {
                                   Index = "1",
                                   Fragments =
                                     new List<AbstractFragment>
                                     {
                                       new ConstantFragment
                                       {
                                         Content
                                           =
                                           "",
                                         Index
                                           = 1,
                                         SpeakerVotes
                                           =
                                           new List
                                           <
                                             SpeakerVote
                                           >()
                                       }
                                     }
                                 }
                               }
                           });

      _privateIndexSentence = 0;
      _privateIndexPage = _model.Documents.Count - 1;
    }

    public void AddSentence()
    {
      if (IsReadOnly)
        return;

      if (_model?.Documents                             == null              ||
          _model.Documents.Count                        <= _privateIndexPage ||
          _model.Documents[_privateIndexPage]           == null              ||
          _model.Documents[_privateIndexPage].Sentences == null)
        return;

      _model.Documents[_privateIndexPage].Sentences.Add(
                                                        new Sentence
                                                        {
                                                          Index =
                                                            (_model.Documents[_privateIndexPage].Sentences.Count + 1)
                                                           .ToString(),
                                                          Fragments =
                                                            new List<AbstractFragment>
                                                            {
                                                              new ConstantFragment
                                                              {
                                                                Content = "",
                                                                Index = 1,
                                                                SpeakerVotes = new List<SpeakerVote>()
                                                              }
                                                            }
                                                        });

      _privateIndexSentence = _model.Documents[_privateIndexPage].Sentences.Count - 1;
    }

    public bool CheckAlternativeOriginalFlow(string doc, string sen)
    {
      var sentence = GetSentence(doc, sen);
      if (sentence == null)
        return false;

      foreach (var f in sentence.Fragments)
      {
        var v = f as VariableFragment;
        if (v == null)
          continue;

        var originalSet = false;
        foreach (var s in v.Fragments)
        {
          var c = s as ConstantFragment;
          if (c == null || !c.IsOriginal)
            continue;

          if (c.IsOriginal && originalSet)
            return true;
          originalSet = true;
        }
      }

      return false;
    }

    public bool CheckEmptyVariant(string doc, string sen)
    {
      var sentence = GetSentence(doc, sen);
      if (sentence == null)
        return false;

      foreach (var f in sentence.Fragments)
      {
        var v = f as VariableFragment;
        if (v == null)
          continue;

        var valid = true;
        foreach (var s in v.Fragments)
        {
          var c = s as ConstantFragment;
          if (c == null)
            continue;

          valid = false;
        }

        if (valid)
          return true;
      }

      return false;
    }

    public bool CheckNegativeVariant(string doc, string sen)
    {
      var sentence = GetSentence(doc, sen);
      if (sentence == null)
        return false;

      foreach (var f in sentence.Fragments)
      {
        var v = f as VariableFragment;
        if (v == null)
          continue;

        var cnt = 0; // Verhindert, dass leere Varianten als Fehler eingestuft werden.
        var valid = true;
        foreach (var s in v.Fragments)
        {
          var c = s as ConstantFragment;
          if (c == null)
            continue;

          foreach (var vote in c.SpeakerVotes)
          {
            cnt++; // muss hier aufgeführt werden, damit nur Stellen mit Wertungen berücksichtigt werden.
            if (vote.Vote is VoteDenied)
              continue;

            valid = false;
            break;
          }
        }

        if (valid && cnt > 0)
          return true;
      }

      return false;
    }

    public bool CheckNegativeVoter(string doc, string sen)
    {
      return GetNegativeVoter(GetSentence(doc, sen)).Count > 0;
    }

    public bool CheckRejectedOriginals(string doc, string sen)
    {
      return CheckRejectedOriginals(GetSentence(doc, sen));
    }

    public bool CheckVariantHasNoVotes(string doc, string sen)
    {
      var sentence = GetSentence(doc, sen);
      if (sentence == null)
        return false;

      foreach (var f in sentence.Fragments)
      {
        var v = f as VariableFragment;
        if (v == null)
          continue;

        var cnt = 0; // Verhindert, dass leere Varianten als Fehler eingestuft werden.
        var valid = true;
        foreach (var s in v.Fragments)
        {
          var c = s as ConstantFragment;
          if (c == null)
            continue;

          cnt++;
          if (c.SpeakerVotes != null && c.SpeakerVotes.Count > 0)
          {
            valid = false;
            break;
          }
        }

        if (valid && cnt > 0)
          return true;
      }

      return false;
    }

    public void DeleteFragment(AbstractFragment fragment)
    {
      if (IsReadOnly)
        return;

      var idx =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.FindIndex(
                                                                                                 x =>
                                                                                                   x.Index ==
                                                                                                   fragment.Index);

      if (idx == -1)
        return;

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.RemoveAt(idx);
    }

    public void Export(AbstractTagger tagger, string path)
    {
      CorpusExplorerEcosystem.InitializeMinimal();

      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException(nameof(path));

      Save();

      var sds = new ConcurrentQueue<Dictionary<string, object>>();
      var speakerMax = 0;

      foreach (var document in _model.Documents)
      foreach (var sentence in document.Sentences)
      {
        var com = GetSentenceCombinations(document.Index, sentence.Index);
        if (com == -1 || com > SentenceCombinationMaximum)
          continue;

        var res = GetSentenceCombinations(sentence, out speakerMax);

        if (res == null)
          continue;

        var counter = 0;
        var temp = new List<Dictionary<string, object>>();
        foreach (var x in res)
        {
          if (x.Key.Length < 5)
            continue;

          if (x.Value == null)
            continue;

          var dic = new Dictionary<string, object>
          {
            {
              "Text", x.Key.Replace("[...]", "<EMPTY>")
                       .Replace("[..]", "<EMPTY>")
                       .Replace("[.]", "<EMPTY>")
                       .Replace("[....]", "<EMPTY>")
                       .Replace("[.....]", "<EMPTY>")
                       .Replace("!EMPTY!", "<EMPTY>")
                       .Replace("EMPTY", "<EMPTY>")
            },
            {"Titel", $"{Format(document.Index)}/{Format(sentence.Index)}/{++counter:D6}"},
            {"Blatt", Format(document.Index)},
            {"Satz", Format(sentence.Index)},
            {"Blatt/Satz", $"{Format(document.Index)}/{Format(sentence.Index)}"},
            {"Quelle", sentence.Source},
            {"Original abgelehnt?", CheckRejectedOriginals(sentence) ? "Ja" : "Nein"},
            {"KAMOKO-DIDX", document.DocumentGuid},
            {"KAMOKO-SIDX", sentence.SentenceGuid}
          };

          foreach (var v in x.Value)
            dic.Add(
                    $"Bewertung {v.SpeakerIndex}",
                    v.Vote is VoteDenied      ? "Ablehnung" :
                    v.Vote is VoteReservation ? "Bedingte Zustimmung" : "Zustimmung");

          temp.Add(dic);
        }

        foreach (var x in temp)
          sds.Enqueue(x);
      }

      if (tagger is AbstractKamokoTagger)
      {
        ((AbstractKamokoTagger) tagger).KamokoProperty = _model;
        ((AbstractKamokoTagger) tagger).SpeakerCount = speakerMax;
      }

      tagger.Input = sds;

      tagger.Execute();
      var corpus = tagger.Output.FirstOrDefault();
      if (corpus == null)
        return;

      var selection = corpus.ToSelection();
      var vm = new ValidateSelectionIntegrityViewModel {Selection = selection};
      vm.Execute();
      vm.CleanSelection.ToCorpus()?.Save(path, false);
    }

    public Sentence GetCurrentSentenceForVizPruposes()
    {
      return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence];
    }

    public IEnumerable<string> GetDocuments()
    {
      return _model?.Documents?.Select(d => d.Index);
    }

    public int GetSentenceCombinations(string doc, string sen)
    {
      var sentence = GetSentence(doc, sen);
      if (sentence == null)
        return -1;

      var res = 1;

      foreach (var fragment in sentence.Fragments)
      {
        res *= GetSentenceCombinationsRecursive(fragment);

        if (res > SentenceCombinationMaximum)
          return res;
      }

      return res;
    }

    public IEnumerable<string> GetSentences(string documentIndex)
    {
      return from d in _model.Documents where d.Index == documentIndex from s in d.Sentences select s.Index;
    }

    public bool IsDocumentIndexUnique(string documentIndex)
    {
      return _model.Documents.Count(d => d.Index == documentIndex) == 1;
    }

    /*
    public int GetSentenceCombinations(string documentIndex, string sentenceIndex)
    {
      return (from d in _model.Documents where d.Index == documentIndex from s in d.Sentences where s.Index == sentenceIndex select s.Fragments.Where(f => !(f is ConstantFragment)).Aggregate(1, (current, f) => current*((VariableFragment) f).Fragments.Count)).FirstOrDefault();
    }
    */

    public bool IsSentenceUnique(string documentIndex, string sentenceIndex)
    {
      var first = (from x in _searchIndex where x.Item2 == documentIndex && x.Item3 == sentenceIndex select x.Item1)
       .FirstOrDefault();
      if (string.IsNullOrEmpty(first))
        return true;

      foreach (var tuple in _searchIndex)
        if ((tuple.Item2 != documentIndex || tuple.Item3 != sentenceIndex) && tuple.Item1 == first)
          return false;

      return true;
    }

    public bool IsSentenceIndexUnique(string documentIndex, string sentenceIndex)
    {
      return
        (from d in _model.Documents where d.Index == documentIndex from s in d.Sentences select s).Count(
                                                                                                         s =>
                                                                                                           s.Index ==
                                                                                                           sentenceIndex) ==
        1;
    }

    public void Load(string path)
    {
      _model = null;
      SavePath = path;

      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      using (var bs = new BufferedStream(fs))
      {
        var xml = new XmlSerializer(typeof(Course));
        _model = xml.Deserialize(bs) as Course;
      }

      if (_model != null)
      {
        _privateIndexPage = 0;
        _privateIndexSentence = 0;
        BuildSearchIndex();
        return;
      }

      MessageBox.Show(Resources.KamokoController_FileErrorNoOutput);
      New();
    }

    public void Load(Course course)
    {
      _model = course;
      _privateIndexPage = 0;
      _privateIndexSentence = 0;
      BuildSearchIndex();
    }

    private void BuildSearchIndex()
    {
      _searchIndex = new List<Tuple<string, string, string>>();
      foreach (var doc in _model.Documents)
      foreach (var sen in doc.Sentences)
        _searchIndex.Add(new
                           Tuple<string, string, string
                           >(string.Join(" ", sen.Fragments.OfType<ConstantFragment>().Select(x => x.Content)),
                             doc.Index, sen.Index));
    }

    public List<Tuple<string, string>> SearchStartsWith(string query)
    {
      var res = new List<Tuple<string, string>>();
      foreach (var tuple in _searchIndex)
        if (tuple.Item1.StartsWith(query))
          res.Add(new Tuple<string, string>(tuple.Item2, tuple.Item3));

      return res;
    }

    public List<Tuple<string, string>> SearchContains(string query)
    {
      var res = new List<Tuple<string, string>>();
      foreach (var tuple in _searchIndex)
        if (tuple.Item1.Contains(query))
          res.Add(new Tuple<string, string>(tuple.Item2, tuple.Item3));

      return res;
    }

    public void Navigate(string doc, string sen)
    {
      for (var i = 0; i < _model.Documents.Count; i++)
      {
        if (_model.Documents[i].Index != doc)
          continue;

        for (var j = 0; j < _model.Documents[i].Sentences.Count; j++)
        {
          if (_model.Documents[i].Sentences[j].Index != sen)
            continue;

          _privateIndexPage = i;
          _privateIndexSentence = j;
          return;
        }
      }
    }

    public void New()
    {
      _model = new Course();
      AddPage();

      _privateIndexPage = 0;
      _privateIndexSentence = 0;
    }

    public void PageMinus()
    {
      if (_privateIndexPage - 1 < 0)
        return;
      _privateIndexPage--;
      _privateIndexSentence = _model.Documents[_privateIndexPage].Sentences.Count - 1;
    }

    public void PagePlus()
    {
      if (_model?.Documents == null)
        return;

      if (_privateIndexPage + 1 >= _model.Documents.Count)
        return;
      _privateIndexSentence = 0;
      _privateIndexPage++;
    }

    public void Save()
    {
      if (IsReadOnly)
        return;
      using (var fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write))
      using (var bs = new BufferedStream(fs))
      {
        var xml = new XmlSerializer(typeof(Course));
        xml.Serialize(bs, _model);
      }
    }

    public void SaveSentence(List<AbstractFragment> fragments, string pageId, string sentenceId)
    {
      if (IsReadOnly)
        return;
      if (_model?.Documents == null || _model.Documents.Count                              <= _privateIndexPage
                                    || _model.Documents[_privateIndexPage].Sentences       == null
                                    || _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence
                                    || _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments ==
                                       null)
        return;

      var list = new List<AbstractFragment>();
      if (fragments != null)
        for (var i = fragments.Count - 1; i > -1; i--)
          list.Add(fragments[i]);

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments = list;
      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index = sentenceId;
      _model.Documents[_privateIndexPage].Index = pageId;
    }

    public void SentenceMinus()
    {
      if (_privateIndexSentence - 1 < 0)
      {
        if (_privateIndexPage > 0)
          PageMinus();
        return;
      }

      _privateIndexSentence--;
    }

    public void SentencePlus()
    {
      if (_model?.Documents == null || _model.Documents.Count                        <= _privateIndexPage
                                    || _model.Documents[_privateIndexPage].Sentences == null)
        return;

      if (_privateIndexSentence + 1 >= _model.Documents[_privateIndexPage].Sentences.Count)
      {
        if (_privateIndexPage + 1 < _model.Documents.Count)
          PagePlus();
        return;
      }

      _privateIndexSentence++;
    }

    private static bool CheckRejectedOriginals(Sentence sentence)
    {
      return sentence != null &&
             sentence.Fragments
                     .OfType<VariableFragment>()
                     .SelectMany(v => v.Fragments, (v, s) => s as ConstantFragment)
                     .Where(c => c != null && c.IsOriginal)
                     .Any(c => c.SpeakerVotes.All(vote => vote.Vote is VoteDenied));
    }

    private string Format(string index)
    {
      while (index.Length < 3)
        index = "0" + index;
      return index.ToUpper();
    }

    private HashSet<int> GetNegativeVoter(Sentence sentence)
    {
      if (sentence == null)
        return new HashSet<int>();

      var match = new Dictionary<int, bool>();

      foreach (var f in sentence.Fragments)
      {
        var v = f as VariableFragment;
        if (v == null)
          continue;

        foreach (var s in v.Fragments)
        {
          var c = s as ConstantFragment;
          if (c == null)
            continue;

          foreach (var vote in c.SpeakerVotes)
          {
            if (!match.ContainsKey(vote.SpeakerIndex))
              match.Add(vote.SpeakerIndex, false);

            if (!(vote.Vote is VoteDenied))
              match[vote.SpeakerIndex] = true;
          }
        }
      }

      return new HashSet<int>(match.Where(x => !x.Value).Select(x => x.Key));
    }

    private Sentence GetSentence(string doc, string sen)
    {
      return (from d in _model.Documents
              from s in d.Sentences
              where d.Index == doc && s.Index == sen
              select s).FirstOrDefault();
    }

    private Dictionary<string, SpeakerVote[]> GetSentenceCombinations(Sentence sentence, out int speakerMax)
    {
      speakerMax = 0;
      Dictionary<string, SpeakerVote[]> res = null;
      var removeSpeaker = GetNegativeVoter(sentence);

      foreach (var fragment in sentence.Fragments)
      {
        var max = fragment.GetSpeakerMax();
        if (max > speakerMax)
          speakerMax = max;

        var fs = fragment.GetSourceStrings(removeSpeaker);
        if (res == null || res.Count == 0)
        {
          res = fs;
        }
        else
        {
          var temp = new Dictionary<string, SpeakerVote[]>();
          foreach (var a in res)
          foreach (var b in fs)
          {
            var key = $"{a.Key} {b.Key}";
            if (temp.ContainsKey(key))
              continue;

            temp.Add(key, MergeVotes(a.Value, b.Value));
          }

          res = temp;
        }
      }

      speakerMax -= removeSpeaker.Count;
      return res;
    }

    private int GetSentenceCombinationsRecursive(AbstractFragment fragment)
    {
      if (fragment is ConstantFragment)
        return 1;

      var res = 0;
      foreach (var f in ((VariableFragment) fragment).Fragments)
      {
        if (f is ConstantFragment)
          res++;
        if (f is VariableFragment)
          res += GetSentenceCombinationsRecursive(f);
      }

      return res == 0 ? 1 : res;
    }

    private SpeakerVote[] MergeVotes(SpeakerVote[] aValue, SpeakerVote[] bValue)
    {
      if (aValue == null)
        return bValue;
      if (bValue == null)
        return aValue;

      var res = new List<SpeakerVote>();
      // Bestehende hinzufügen und ggf. abändern
      foreach (var a in aValue)
      {
        var b = (from x in bValue where x.SpeakerIndex == a.SpeakerIndex select x).FirstOrDefault();
        if (b == null)
          res.Add(a);
        else
          res.Add(a.Vote.Level > b.Vote.Level ? a : b);
      }

      // Neue hinzufügen
      res.AddRange(from b in bValue
                   let a = (from x in aValue where x.SpeakerIndex == b.SpeakerIndex select x).FirstOrDefault()
                   where a == null
                   select b);
      return res.ToArray();
    }

    public void RemoveSentence()
    {
      _model.Documents[_privateIndexPage].Sentences.RemoveAt(_privateIndexSentence);
      SentenceMinus();
    }

    public void RemovePage()
    {
      _model.Documents.RemoveAt(_privateIndexPage);
      PageMinus();
    }
  }
}