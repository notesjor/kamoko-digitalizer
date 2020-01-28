using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CorpusExplorer.Tool4.KAMOKO.FlowViz;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls
{
  public partial class KamokoFlowVisualizer : AbstractUserControl
  {
    private readonly KamokoFlowDiagram _kfd;

    public KamokoFlowVisualizer()
    {
      InitializeComponent();
      _kfd = new KamokoFlowDiagram
      {
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch
      };
      elementHost1.Child = _kfd;
    }

    public void ShowSentence(Sentence sentence)
    {
      _kfd.CallNew();

      var last = new List<Guid>();
      foreach (var fragment in sentence.Fragments) last = BuildSentence(fragment, last, true);

      _kfd.CallLayoutAsHorizontalTree();
    }

    private List<Guid> BuildSentence(AbstractFragment fragment, List<Guid> last, bool isOriginal)
    {
      var current = new List<Guid>();
      if (fragment is ConstantFragment)
      {
        var node = _kfd.CallAddNode(((ConstantFragment) fragment).Content, ConvertVotes((ConstantFragment) fragment),
                                    isOriginal || ((ConstantFragment) fragment).IsOriginal);
        current.Add(node);

        foreach (var guid in last)
          _kfd.CallAddConnection(guid, node);
      }
      else if (fragment is VariableFragment)
      {
        foreach (var x in ((VariableFragment) fragment).Fragments)
          current.AddRange(BuildSentence(x, last, false));
      }

      return current.Count == 0 ? last : current;
    }

    private FlowVizVote[] ConvertVotes(ConstantFragment fragment)
    {
      return fragment.SpeakerVotes.Select(x =>
                                            x.Vote is VoteAccept      ? FlowVizVote.Accept :
                                            x.Vote is VoteDenied      ? FlowVizVote.Denied :
                                            x.Vote is VoteReservation ? FlowVizVote.PossibleIf : FlowVizVote.None)
                     .ToArray();
    }
  }
}