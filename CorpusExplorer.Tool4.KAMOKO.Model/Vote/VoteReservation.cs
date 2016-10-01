#region

using System;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Vote
{
  [XmlRoot]
  [Serializable]
  public class VoteReservation : AbstractVote
  {
    [XmlAttribute]
    public override string Label { get { return "Unter Vorbehalt"; } }
  }
}