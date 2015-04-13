#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Scraper;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Parser;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controller
{
  public class KamokoController
  {
    #region Fields

    private Course _model;

    private int _privateIndexPage = -1;

    private int _privateIndexSentence = -1;

    #endregion

    #region Public Properties

    public string CurrentSource
    {
      get { return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source; }
      set { _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Source = value; }
    }

    public IEnumerable<AbstractFragment> CurrentFragments
    {
      get
      {
        if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage ||
            _model.Documents[_privateIndexPage].Sentences == null ||
            _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence || _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments == null)
        {
          return new List<AbstractFragment>();
        }
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments;
      }
    }

    public string CurrentPageIndex
    {
      get
      {
        if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage)
          return "0";
        return _model.Documents[_privateIndexPage].Index;
      }
    }

    public string CurrentPageIndexLabel
    {
      get
      {
        if (_model == null || _model.Documents == null)
          return "0 / 0";
        return string.Format("{0} / {1}", _privateIndexPage + 1, _model.Documents.Count);
      }
    }

    public string CurrentSentenceIndex
    {
      get
      {
        if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage || _model.Documents[_privateIndexPage].Sentences == null || _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence)
          return "0";
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index;
      }
    }

    public string CurrentSentenceIndexLabel
    {
      get
      {
        if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage || _model.Documents[_privateIndexPage].Sentences == null)
          return "0 / 0";
        return string.Format("{0} / {1}", _privateIndexSentence + 1, _model.Documents[_privateIndexPage].Sentences.Count);
      }
    }

    public string SavePath { get; set; }

    #endregion

    #region Public Methods and Operators

    public void AddFragment(AbstractFragment previewFragment, AbstractFragment newFragment)
    {
      var idx =
        _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.FindIndex(
          x => x.Index == previewFragment.Index);

      if (idx == -1)
      {
        return;
      }

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
      if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage] == null || _model.Documents[_privateIndexPage].Sentences == null)
        return;

      _model.Documents[_privateIndexPage].Sentences.Add(
        new Sentence
        {
          Index = (_model.Documents[_privateIndexPage].Sentences.Count + 1).ToString(),
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
          x => x.Index == fragment.Index);

      if (idx == -1)
      {
        return;
      }

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments.RemoveAt(idx);
    }

    public void Export(string path)
    {
      Configuration.Initialize();

      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");

      Save();

      var sds = new List<ScrapDocument>();
      var speakerMax = 0;

      foreach (var document in _model.Documents)
      {
        foreach (var sentence in document.Sentences)
        {
          IEnumerable<string> res = null;

          foreach (var fragment in sentence.Fragments)
          {
            var max = fragment.GetSpeakerMax();
            if (max > speakerMax) speakerMax = max;

            if (res == null)
            {
              res = fragment.GetSourceStrings();
            }
            else
            {
              var temp1 = res.ToArray();
              var temp2 = fragment.GetSourceStrings();
              res = (from a in temp1 from b in temp2 select a + " " + b);
            }
          }

          if (res == null)
            continue;

          int counter = 0;
          sds.AddRange(from x in res
            where x.Length >= 5
            select new ScrapDocument(new Dictionary<string, object>
            {
              {"Text", x},
              {"Titel", string.Format("{0}/{1}/{2}", Format(document.Index), Format(sentence.Index), (++counter).ToString("D6")) },
              {"Blatt", document.Index},
              {"Satz", sentence.Index},
              {"Quelle", sentence.Source}
            }));
        }
      }

      var parser = new TreeTaggerKamokoTagger(speakerMax)
      {
        CorpusName = Path.GetFileNameWithoutExtension(path),
        ScraperResults = sds,
        TreeTaggerBatchFilename = "chunk-french.bat"
      };

      var corpus = parser.Execute();
      corpus.Save(path);
    }

    public string Format(string index)
    {
      while (index.Length < 3)
      {
        index = "0" + index;
      }
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

      MessageBox.Show("Die Datei scheint beschädigt zu sein. Daher wurde ein neues Korpus erstellt.");
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
      if (_privateIndexPage - 1 < 0)
      {
        return;
      }
      _privateIndexPage--;
      _privateIndexSentence = _model.Documents[_privateIndexPage].Sentences.Count - 1;
    }

    public void PagePlus()
    {
      if (_model == null || _model.Documents == null) return;

      if (_privateIndexPage + 1 >= _model.Documents.Count)
      {
        return;
      }
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
      if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage].Sentences == null ||
          _model.Documents[_privateIndexPage].Sentences.Count <= _privateIndexSentence ||
          _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments == null) return;

      var list = new List<AbstractFragment>();
      if (fragments != null)
        for (var i = fragments.Count - 1; i > -1; i--)
        {
          list.Add(fragments[i]);
        }

      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments = list;
      _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index = sentenceId;
      _model.Documents[_privateIndexPage].Index = pageId;
    }

    public void SentenceMinus()
    {
      if (_privateIndexSentence - 1 < 0)
      {
        if (_privateIndexPage > 0)
        {
          PageMinus();
        }
        return;
      }
      _privateIndexSentence--;
    }

    public void SentencePlus()
    {
      if (_model == null || _model.Documents == null || _model.Documents.Count <= _privateIndexPage ||
          _model.Documents[_privateIndexPage].Sentences == null) return;

      if (_privateIndexSentence + 1 >= _model.Documents[_privateIndexPage].Sentences.Count)
      {
        if (_privateIndexPage + 1 < _model.Documents.Count)
        {
          PagePlus();
        }
        return;
      }
      _privateIndexSentence++;
    }

    #endregion
  }
}