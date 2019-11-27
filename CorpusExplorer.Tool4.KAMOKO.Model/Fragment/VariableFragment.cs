#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment
{
  [XmlRoot]
  [Serializable]
  public class VariableFragment : AbstractFragment
  {
    public VariableFragment()
    {
      Fragments = new List<AbstractFragment>();
    }

    [XmlArray]
    public List<AbstractFragment> Fragments { get; set; }

    public override Dictionary<string, SpeakerVote[]> GetSourceStrings(HashSet<int> ignoreSpeaker)
    {
      var res = new Dictionary<string, SpeakerVote[]>();

      foreach (var fragment in Fragments)
      {
        var items = fragment.GetSourceStrings(ignoreSpeaker);
        foreach (var item in items)
          if (!res.ContainsKey(item.Key))
            res.Add(item.Key, item.Value);
      }

      return res;
    }

    public override int GetSpeakerMax()
    {
      var res = Fragments.Select(fragment => fragment.GetSpeakerMax()).Concat(new[] {0}).Max();
      return res < 0 ? 0 : res;
    }
  }
}