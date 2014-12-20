namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

  [XmlRoot]
  [Serializable]
  public class Sentence
  {
    #region Constructors and Destructors

    public Sentence()
    {
      this.Index = "1";
      this.Fragments = new List<AbstractFragment>();
    }

    #endregion

    #region Public Properties

    [XmlArrayItem(typeof(ConstantFragment))]
    [XmlArrayItem(typeof(VariableFragment))]
    public List<AbstractFragment> Fragments { get; set; }

    [XmlAttribute]
    public string Index { get; set; }

    #endregion
  }
}