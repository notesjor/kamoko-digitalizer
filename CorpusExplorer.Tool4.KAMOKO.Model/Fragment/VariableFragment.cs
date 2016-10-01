#region

using System;
using System.Collections.Concurrent;
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
    public VariableFragment() { Fragments = new List<AbstractFragment>(); }

    [XmlArray]
    public List<AbstractFragment> Fragments { get; set; }

    public override IEnumerable<string> GetSourceStrings()
    {
      var res = new ConcurrentBag<string>();

      foreach (var fragment in Fragments)
      {
        IEnumerable<string> items;

        if (res.Count == 0) items = fragment.GetSourceStrings();
        else
        {
          var temp1 = fragment.GetSourceStrings();
          items = from x in res from y in temp1 select x + " " + y;
        }

        foreach (var item in items) res.Add(item);
      }

      return res;
    }

    public override int GetSpeakerMax()
    {
      var res = Fragments.Select(fragment => fragment.GetSpeakerMax()).Concat(new[] {0}).Max();
      return res;
    }
  }
}