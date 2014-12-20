namespace CorpusExplorer.Tool4.KAMOKO.Model.Vote
{
  using System;
  using System.Xml.Serialization;

  using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;

  [XmlRoot]
  [Serializable]
  public class VoteReservation : AbstractVote
  {
    #region Public Properties

    [XmlAttribute]
    public override string Label
    {
      get
      {
        return "Unter Vorbehalt";
      }
    }

    #endregion
  }
}