#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  [XmlRoot]
  [Serializable]
  public class Sentence
  {
    public Sentence()
    {
      Index = "1";
      Fragments = new List<AbstractFragment>();
      SentenceGuid = Guid.NewGuid();
      Source = "";
    }

    [XmlArrayItem(typeof(ConstantFragment))]
    [XmlArrayItem(typeof(VariableFragment))]
    public List<AbstractFragment> Fragments { get; set; }

    [XmlAttribute]
    public string Index { get; set; }

    [XmlAttribute]
    public Guid SentenceGuid { get; set; }

    [XmlAttribute]
    public string Source { get; set; }
  }
}