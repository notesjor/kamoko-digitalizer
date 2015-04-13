#region

using System;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  [XmlRoot]
  [Serializable]
  public class SpeakerVote
  {
    #region Constructors and Destructors

    public SpeakerVote()
    {
      SpeakerIndex = -1;
      Vote = null;
    }

    #endregion

    #region Public Properties

    [XmlAttribute]
    public int SpeakerIndex { get; set; }

    [XmlElement(typeof (VoteAccept))]
    [XmlElement(typeof (VoteDenied))]
    [XmlElement(typeof (VoteReservation))]
    public AbstractVote Vote { get; set; }

    #endregion
  }
}