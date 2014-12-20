namespace CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract
{
  using System;
  using System.Xml.Serialization;

  [XmlRoot]
  [Serializable]
  [XmlInclude(typeof(VoteAccept))]
  [XmlInclude(typeof(VoteDenied))]
  [XmlInclude(typeof(VoteReservation))]
  public abstract class AbstractVote
  {
    #region Public Properties

    [XmlAttribute]
    public abstract string Label { get; }

    #endregion
  }
}