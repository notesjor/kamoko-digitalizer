namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  using System;
  using System.Xml.Serialization;

  using CorpusExplorer.Tool4.KAMOKO.Model.Vote;
  using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;

  [XmlRoot]
  [Serializable]
  public class SpeakerVote
  {
    #region Constructors and Destructors

    public SpeakerVote()
    {
      this.SpeakerIndex = -1;
      this.Vote = null;
    }

    #endregion

    #region Public Properties

    [XmlAttribute]
    public int SpeakerIndex { get; set; }

    [XmlElement(typeof(VoteAccept))]
    [XmlElement(typeof(VoteDenied))]
    [XmlElement(typeof(VoteReservation))]
    public AbstractVote Vote { get; set; }

    #endregion
  }
}