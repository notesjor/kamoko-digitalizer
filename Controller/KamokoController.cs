namespace CorpusExplorer.Tool4.KAMOKO.Controller
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Configuration;
  using System.IO;
  using System.Linq;
  using System.Windows.Forms;
  using System.Xml.Serialization;

  using CorpusExplorer.Sdk.Helper;
  using CorpusExplorer.Sdk.Model.Scraper;
  using CorpusExplorer.Sdk.Utils.DocumentProcessing.Parser;
  using CorpusExplorer.Tool4.KAMOKO.Model;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

  public class KamokoController
  {
    #region Fields

    private Course _model;

    private int _privateIndexPage = -1;

    private int _privateIndexSentence = -1;

    #endregion

    #region Public Properties

    public IEnumerable<AbstractFragment> CurrentFragments
    {
      get
      {
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Fragments;
      }
    }

    public string CurrentPageIndex
    {
      get
      {
        return _model.Documents[_privateIndexPage].Index;
      }
    }

    public string CurrentPageIndexLabel
    {
      get
      {
        return string.Format("{0} / {1}", _privateIndexPage + 1, _model.Documents.Count);
      }
    }

    public string CurrentSentenceIndex
    {
      get
      {
        return _model.Documents[_privateIndexPage].Sentences[_privateIndexSentence].Index;
      }
    }

    public string CurrentSentenceIndexLabel
    {
      get
      {
        return string.Format(
          "{0} / {1}",
          _privateIndexSentence + 1,
          _model.Documents[_privateIndexPage].Sentences.Count);
      }
    }

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

      Save(path + ".kamoko.xml");

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

            IEnumerable<string> items = null;

            if (res == null)
            {
              items = fragment.GetSourceStrings();
            }
            else
            {
              var temp1 = fragment.GetSourceStrings();
              items = from x in res from y in temp1 select x + " " + y;
            }

            res = items;
          }

          sds.AddRange(res.Select(entry => new ScrapDocument(new Dictionary<string, object> { { "Text", entry }, { "Blatt/Satz", string.Format("{0}/{1}", document.Index, sentence.Index) }, { "Blatt", document.Index }, { "Satz", sentence.Index }, })));
        }
      }

      var parser = new TreeTaggerKamokoParser(speakerMax)
                     {
                       CorpusName = Path.GetFileNameWithoutExtension(path),
                       ScraperResults = sds,
                       TreeTaggerBatchFilename = "chunk-french.bat"
                     };

      var corpus = parser.Execute();
      corpus.Save(path);
    }

    public void Load(string path)
    {
      _model = null;

      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        var xml = new XmlSerializer(typeof(Course));
        _model = xml.Deserialize(fs) as Course;
      }

      if (this._model != null)
      {
        _privateIndexPage = 0;
        _privateIndexSentence = 0;
        return;
      }

      MessageBox.Show("Die Datei scheint beschädigt zu sein. Daher wurde ein neues Korpus erstellt.");
      this.New();
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
      if (_privateIndexPage + 1 >= _model.Documents.Count)
      {
        return;
      }
      _privateIndexSentence = 0;
      _privateIndexPage++;
    }

    public void Save(string path)
    {
      using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        var xml = new XmlSerializer(typeof(Course));
        xml.Serialize(fs, _model);
      }
    }

    public void SaveSentence(List<AbstractFragment> fragments, string pageId, string sentenceId)
    {
      var list = new List<AbstractFragment>();
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