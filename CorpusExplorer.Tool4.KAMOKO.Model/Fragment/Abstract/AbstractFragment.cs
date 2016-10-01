#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract
{
  [XmlRoot]
  [Serializable]
  [XmlInclude(typeof(ConstantFragment))]
  [XmlInclude(typeof(VariableFragment))]
  public abstract class AbstractFragment
  {
    protected AbstractFragment() { Index = -1; }

    [XmlAttribute]
    public int Index { get; set; }

    public abstract IEnumerable<string> GetSourceStrings();
    public abstract int GetSpeakerMax();
  }
}