#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  [XmlRoot]
  [Serializable]
  public class Document
  {
    #region Constructors and Destructors

    public Document()
    {
      Index = "1";
      Sentences = new List<Sentence>();
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