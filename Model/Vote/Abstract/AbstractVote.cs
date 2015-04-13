#region

using System;
using System.Xml.Serialization;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract
{
  [XmlRoot]
  [Serializable]
  [XmlInclude(typeof (VoteAccept))]
  [XmlInclude(typeof (VoteDenied))]
  [XmlInclude(typeof (VoteReservation))]
  public abstract class AbstractVote
  {
    #region Public Properties

    [XmlAttribute]
    public abstract string Label { get; }

    #endregion
  }
}