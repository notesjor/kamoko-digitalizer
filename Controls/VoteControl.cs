#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;
using Telerik.WinControls;
using Telerik.WinControls.UI;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  public partial class VoteControl : AbstractUserControl
  {
    #region Fields

    public Dictionary<AbstractVote, RadRadioButton> _buttons;

    #endregion

    private readonly HashSet<string> _validLabels = new HashSet<string>
    {
      "",
      "1",
      "2",
      "3",
      "4",
      "5",
      "6",
      "7",
      "8"
    };

    #region Constructors and Destructors

    public VoteControl(SpeakerVote vote)
    {
      InitializeComponent();
      _buttons = new Dictionary<AbstractVote, RadRadioButton>
      {
        {new VoteAccept(), check_accept},
        {new VoteDenied(), check_denied},
        {new VoteReservation(), check_reservation}
      };
      if (vote != null)
      {
        SetSpeakerVote(vote);
      }
    }

    #endregion

    private void radTextBox1_TextChanging(object sender, TextChangingEventArgs e)
    {
      if (!_validLabels.Contains(((RadTextBox) sender).Text))
      {
        MessageBox.Show("Für die Sprecherbezeichnungen sind nur Zahlen zwischen 1-8 zulässig.");
        e.Cancel = true;
      }
    }

    #region Public Methods and Operators

    public SpeakerVote GetSpeakerVote()
    {
      AbstractVote vote = null;
      foreach (var button in _buttons.Where(button => button.Value.IsChecked))
      {
        vote = button.Key;
      }

      var speaker = 0;
      int.TryParse(radTextBox1.Text, out speaker);

      return new SpeakerVote {SpeakerIndex = speaker, Vote = vote};
    }

    public void SetSpeakerVote(SpeakerVote speakerVote)
    {
      if (speakerVote == null)
      {
        return;
      }

      radTextBox1.Text = speakerVote.SpeakerIndex.ToString();

      if (speakerVote.Vote == null)
      {
        return;
      }

      foreach (var button in _buttons)
      {
        button.Value.IsChecked = (speakerVote.Vote.Label == button.Key.Label);
      }
    }

    #endregion
  }
}