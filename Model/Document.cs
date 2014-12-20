namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  [XmlRoot]
  [Serializable]
  public class Document
  {
    #region Constructors and Destructors

    public Document()
    {
      this.Index = "1";
      this.Sentences = new List<Sentence>();
    }

    #endregion

    #region Public Properties

    [XmlAttribute]
    public string Index { get; set; }

    [XmlArray]
    public List<Sentence> Sentences { get; set; }

    #endregion
  }
}