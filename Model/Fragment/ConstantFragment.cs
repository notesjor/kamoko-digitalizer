namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Xml.Serialization;

  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
  using CorpusExplorer.Tool4.KAMOKO.Model.Vote;

  [XmlRoot]
  [Serializable]
  public class ConstantFragment : AbstractFragment
  {
    #region Constructors and Destructors

    public ConstantFragment()
    {
      this.Content = "";
      this.SpeakerVotes = new List<SpeakerVote>();
    }

    #endregion

    #region Public Properties

    [XmlElement]
    public string Content { get; set; }

    [XmlArray]
    public List<SpeakerVote> SpeakerVotes { get; set; }

    #endregion

    public override IEnumerable<string> GetSourceStrings()
    {
      if (SpeakerVotes.Count == 0)
      {
        return new[] { Content.Trim() };
      }
      else
      {
        var stb = new StringBuilder("<M ");
        foreach (var vote in SpeakerVotes)
        {
          stb.AppendFormat(" S{0}{1} ",vote.SpeakerIndex, vote.Vote is VoteAccept ? "Z" : vote.Vote is VoteReservation ? "B" : "A");
        }
        return new[] { stb.ToString().Trim() + "> " + Content.Trim() + " </M>" };
      }
    }

    public override int GetSpeakerMax()
    {
      return this.SpeakerVotes.Select(vote => vote.SpeakerIndex).Concat(new[] { 0 }).Max();
    }
  }
}