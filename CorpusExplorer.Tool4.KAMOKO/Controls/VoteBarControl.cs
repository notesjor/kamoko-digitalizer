#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Helper;
using CorpusExplorer.Tool4.KAMOKO.Model;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  public partial class VoteBarControl : AbstractUserControl
  {
    private List<SpeakerVote> _votes;

    public VoteBarControl()
    {
      InitializeComponent();
      components = new DisposingContainer();
    }

    public List<SpeakerVote> GetSpeakers()
    {
      SaveData();
      return _votes;
    }

    public void SetSpeakers(List<SpeakerVote> votes)
    {
      _votes = votes;
      LoadData();
    }

    private void btn_item_add_Click(object sender, EventArgs e)
    {
      SaveData();

      _votes.Add(new SpeakerVote {SpeakerIndex = _votes.Count == 0 ? 1 : _votes.Last().SpeakerIndex + 1});

      LoadData();
    }

    private void LoadData()
    {
      radScrollablePanel1.Controls.Clear();
      components.Dispose();

      foreach (var control in _votes.Select(vote => new VoteControl(vote) {Dock = DockStyle.Left}))
      {
        components.Add(control);
        radScrollablePanel1.PanelContainer.Controls.Add(control);
      }
    }

    private void SaveData()
    {
      _votes =
        radScrollablePanel1.PanelContainer.Controls.OfType<VoteControl>()
                           .Select(vc => vc.GetSpeakerVote())
                           .Where(sv => sv.Vote != null)
                           .ToList();
    }
  }
}