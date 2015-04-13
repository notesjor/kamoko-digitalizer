#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment
{
  [XmlRoot]
  [Serializable]
  public class ConstantFragment : AbstractFragment
  {
    #region Constructors and Destructors

    public ConstantFragment()
    {
      Content = "";
      SpeakerVotes = new List<SpeakerVote>();
      IsOriginal = false;
    }

    [XmlAttribute]
    public bool IsOriginal { get; set; }

    #endregion

    public override IEnumerable<string> GetSourceStrings()
    {
      if (SpeakerVotes.Count == 0)
      {
        return new[] {Content.Trim()};
      }
      var stb = new StringBuilder("\r\n<M");
      foreach (var vote in SpeakerVotes)
      {
        stb.AppendFormat("S{0}{1}", vote.SpeakerIndex,
          vote.Vote is VoteAccept ? "Z" : vote.Vote is VoteReservation ? "B" : "A");
      }
      stb.Append(this.IsOriginal ? "ORIGINAL" : "");
      return new[] { stb.ToString().Trim() + ">\r\n" + Content.Trim() + "\r\n</M>\r\n" };
    }

    public override int GetSpeakerMax()
    {
      return SpeakerVotes.Select(vote => vote.SpeakerIndex).Concat(new[] {0}).Max();
    }

    #region Public Properties

    [XmlElement]
    public string Content { get; set; }

    [XmlArray]
    public List<SpeakerVote> SpeakerVotes { get; set; }

    #endregion
  }
}