#region

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Parser;
using CorpusExplorer.Tool4.KAMOKO.Properties;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controller
{
  public class KamokoController
  {
    private Course _model;
    private int _privateIndexPage = -1;
    private int _privateIndexSentence = -1;

    public IEnumerable<AbstractFragment> CurrentFragments
    {
      get
      {
        if (_model?.Documents == null ||
            _model.Documents.Count <= _privateIndexPage ||
            _model.Documents[_privateIndexPage].Sentences == null ||
            _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence ||
            _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments == null) return new List<AbstractFragment>();
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments;
      }
    }

    public string CurrentPageIndex
    {
      get
      {
        if (_model?.Documents == null ||
            _model.Documents.Count <= _privateIndexPage) return "0";
        return _model.Documents[_privateIndexPage].Index;
      }
    }

    public string CurrentPageIndexLabel
    {
      get
      {
        if (_model == null ||
            _model.Documents == null) return "0 / 0";
        return $"{_privateIndexPage + 1} / {_model.Documents.Count}";
      }
    }

    public string CurrentSentenceIndex
    {
      get
      {
        if (_model == null ||
            _model.Documents == null ||
            _model.Documents.Count <= _privateIndexPage ||
            _model.Documents[_privateIndexPage].Sentences == null ||
            _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence) return "0";
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index;
      }
    }

    public string CurrentSentenceIndexLabel
    {
      get
      {
        if (_model == null ||
            _model.Documents == null ||
            _model.Documents.Count <= _privateIndexPage ||
            _model.Documents[_privateIndexPage].Sentences == null) return "0 / 0";
        return $"{_privateIndexSentence + 1} / {_model.Documents[_privateIndexPage].Sentences.Count}";
      }
    }

    public string CurrentSource
    {
      get { return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source; }
      set { _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source = value; }
    }

    public string SavePath { get; set; }

    public IEnumerable<string> GetDocuments()
    {
      return _model.Documents.Select(d => d.Index);
    }

    public bool IsDocumentIndexUnique(string documentIndex)
    {
      return _model.Documents.Count(d => d.Index == documentIndex) == 1;
    }

    public IEnumerable<string> GetSentences(string documentIndex)
    {
      return (from d in _model.Documents where d.Index == documentIndex from s in d.Sentences select s.Index);
    }

    public int GetSentenceCombinations(string documentIndex, string sentenceIndex)
    {
      return (from d in _model.Documents where d.Index == documentIndex from s in d.Sentences where s.Index == sentenceIndex select s.Fragments.Where(f => !(f is ConstantFragment)).Aggregate(1, (current, f) => current*((VariableFragment) f).Fragments.Count)).FirstOrDefault();
    }

    public bool IsSentenceIndexUnique(string documentIndex, string sentenceIndex)
    {
      return (from d in _model.Documents where d.Index == documentIndex from s in d.Sentences select s).Count(s => s.Index == sentenceIndex) == 1;
    }

    public void AddFragment(AbstractFragment previewFragment, AbstractFragment newFragment)
    {
      var idx =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.FindIndex(
          x =>
            x.Index ==
            previewFragment.Index);

      if (idx == -1) return;

      // Modifikation von newFragment zur Sicherung der Index-Integrität
      newFragment.Index =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.Max(x => x.Index) + 1;

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.Add(newFragment);
    }

    public void AddPage()
    {
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
      if (_model?.Documents == null ||
          _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage] == null ||
          _model.Documents[_privateIndexPage].Sentences == null) return;

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

    public void DeleteFragment(AbstractFragment fragment)
    {
      var idx =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.FindIndex(
          x =>
            x.Index ==
            fragment.Index);

      if (idx == -1) return;

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.RemoveAt(idx);
    }

    public void Export(string path)
    {
      CorpusExplorerEcosystem.InitializeMinimal();

      if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");

      Save();

      var sds = new ConcurrentQueue<Dictionary<string, object>>();
      var speakerMax = 0;

      foreach (var document in _model.Documents)
      {
        foreach (var sentence in document.Sentences)
        {
          var c = GetSentenceCombinations(document.Index, sentence.Index);
          if (c > 1000)
            continue;

          IEnumerable<string> res = null;

          foreach (var fragment in sentence.Fragments)
          {
            var max = fragment.GetSpeakerMax();
            if (max > speakerMax) speakerMax = max;

            if (res == null) res = fragment.GetSourceStrings();
            else
            {
              var temp1 = res.ToArray();
              var temp2 = fragment.GetSourceStrings();
              res = from a in temp1 from b in temp2 select a + " " + b;
            }
          }

          if (res == null) continue;

          var counter = 0;
          var temp = from x in res
                     where x.Length >= 5
                     select new Dictionary<string, object>
                     {
                       {"Text", x.Replace("[...]", "<EMPTY>").Replace("[..]", "<EMPTY>").Replace("[.]", "<EMPTY>").Replace("[....]", "<EMPTY>").Replace("[.....]", "<EMPTY>").Replace("!EMPTY!", "<EMPTY>").Replace("EMPTY", "<EMPTY>")},
                       {"Titel", $"{Format(document.Index)}/{Format(sentence.Index)}/{(++counter):D6}"},
                       {"Blatt", document.Index},
                       {"Satz", sentence.Index},
                       {"Quelle", sentence.Source},
                       {"KAMOKO-DIDX", document.DocumentGuid},
                       {"KAMOKO-SIDX", sentence.SentenceGuid},
                     };
          foreach (var x in temp) { sds.Enqueue(x); }
        }
      }

      var parser = new TreeTaggerKamokoTagger(speakerMax)
      {
        OutputPath = path,
        KamokoProperty = _model,
        Input = sds
      };

      parser.Execute();
    }

    public string Format(string index)
    {
      while (index.Length < 3) index = "0" + index;
      return index.ToUpper();
    }

    public void Load(string path)
    {
      _model = null;

      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        var xml = new XmlSerializer(typeof(Course));
        _model = xml.Deserialize(fs) as Course;
      }

      SavePath = path;

      if (_model != null)
      {
        _privateIndexPage = 0;
        _privateIndexSentence = 0;
        return;
      }

      MessageBox.Show(Resources.KamokoController_FileErrorNoOutput);
      New();
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
      if (_privateIndexPage - 1 < 0) return;
      _privateIndexPage--;
      _privateIndexSentence = _model.Documents[_privateIndexPage].Sentences.Count - 1;
    }

    public void PagePlus()
    {
      if (_model == null ||
          _model.Documents == null) return;

      if (_privateIndexPage + 1 >= _model.Documents.Count) return;
      _privateIndexSentence = 0;
      _privateIndexPage++;
    }

    public void Save()
    {
      using (var fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write))
      {
        var xml = new XmlSerializer(typeof(Course));
        xml.Serialize(fs, _model);
      }
    }

    public void SaveSentence(List<AbstractFragment> fragments, string pageId, string sentenceId)
    {
      if (_model == null ||
          _model.Documents == null ||
          _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage].Sentences == null ||
          _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence ||
          _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments == null) return;

      var list = new List<AbstractFragment>();
      if (fragments != null)
      {
        for (var i = fragments.Count - 1; i > -1; i--) list.Add(fragments[i]);
      }

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments = list;
      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index = sentenceId;
      _model.Documents[_privateIndexPage].Index = pageId;
    }

    public void SentenceMinus()
    {
      if (_privateIndexSentence - 1 < 0)
      {
        if (_privateIndexPage > 0) PageMinus();
        return;
      }
      _privateIndexSentence--;
    }

    public void SentencePlus()
    {
      if (_model == null ||
          _model.Documents == null ||
          _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage].Sentences == null) return;

      if (_privateIndexSentence + 1 >= _model.Documents[_privateIndexPage].Sentences.Count)
      {
        if (_privateIndexPage + 1 < _model.Documents.Count) PagePlus();
        return;
      }
      _privateIndexSentence++;
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
  }
}