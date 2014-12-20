namespace CorpusExplorer.Tool4.KAMOKO.Model.Fragment
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Serialization;

  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

  [XmlRoot]
  [Serializable]
  public class VariableFragment : AbstractFragment
  {
    #region Constructors and Destructors

    public VariableFragment()
    {
      this.Fragments = new List<AbstractFragment>();
    }

    #endregion

    #region Public Properties

    [XmlArray]
    public List<AbstractFragment> Fragments { get; set; }

    #endregion

    public override IEnumerable<string> GetSourceStrings()
    {
      var res = new ConcurrentBag<string>();

      foreach (var fragment in Fragments)
      {
        IEnumerable<string> items = null;

        if (res.Count == 0)
        {
          items = fragment.GetSourceStrings();
        }
        else
        {
          var temp1 = fragment.GetSourceStrings();
          items = from x in res from y in temp1 select x + " " + y;
        }

        foreach (var item in items)
        {
          res.Add(item);
        }
      }

      return res;
    }

    public override int GetSpeakerMax()
    {
      var res = this.Fragments.Select(fragment => fragment.GetSpeakerMax()).Concat(new[] { 0 }).Max();
      return res;
    }
  }
}