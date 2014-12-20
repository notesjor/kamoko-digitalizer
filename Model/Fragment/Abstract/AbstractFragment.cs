﻿namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  [XmlRoot]
  [Serializable]
  [XmlInclude(typeof(ConstantFragment))]
  [XmlInclude(typeof(VariableFragment))]
  public abstract class AbstractFragment
  {
    #region Constructors and Destructors

    public AbstractFragment()
    {
      this.Index = -1;
    }

    #endregion

    #region Public Properties

    [XmlAttribute]
    public int Index { get; set; }

    #endregion

    public abstract IEnumerable<string> GetSourceStrings();

    public abstract int GetSpeakerMax();
  }
}