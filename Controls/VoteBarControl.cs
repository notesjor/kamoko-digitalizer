namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;

  using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
  using CorpusExplorer.Tool4.KAMOKO.Model;

  public partial class VoteBarControl : AbstractUserControl
  {
    #region Fields

    private List<SpeakerVote> _votes;

    #endregion

    #region Constructors and Destructors

    public VoteBarControl()
    {
      InitializeComponent();
    }

    #endregion

    #region Public Methods and Operators

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

    #endregion

    #region Methods

    private void LoadData()
    {
      radScrollablePanel1.Controls.Clear();

      foreach (var vote in _votes)
      {
        radScrollablePanel1.PanelContainer.Controls.Add(new VoteControl(vote) { Dock = DockStyle.Left });
      }
    }

    private void SaveData()
    {
      _votes =
        this.radScrollablePanel1.PanelContainer.Controls.OfType<VoteControl>()
          .Select(vc => vc.GetSpeakerVote())
          .Where(sv => sv.Vote != null)
          .ToList();
    }

    private void btn_item_add_Click(object sender, EventArgs e)
    {
      SaveData();
      _votes.Add(new SpeakerVote { SpeakerIndex = _votes.Count == 0 ? 1 : _votes.Last().SpeakerIndex + 1 });
      LoadData();
    }

    #endregion
  }
}