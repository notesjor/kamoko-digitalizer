#region

using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.Controls.Delegates;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Properties;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controls.Abstract
{
  public partial class AbstractFragmentControl : AbstractUserControl
  {
    public AbstractFragmentControl() { InitializeComponent(); }

    public event FragmentManipulationDelegate FragmentAddConstant;
    public event FragmentManipulationDelegate FragmentAddVariable;
    public event FragmentManipulationDelegate FragmentDelete;

    public virtual AbstractFragment GetFragment() { return null; }

    protected void OnAddConstnat(AbstractFragment fragment)
    {
      if (FragmentAddConstant != null) FragmentAddConstant(fragment);
    }

    protected void OnAddVariable(AbstractFragment fragment)
    {
      if (FragmentAddVariable != null) FragmentAddVariable(fragment);
    }

    protected void OnDelete(AbstractFragment fragment)
    {
      if (MessageBox.Show(
        Resources.AbstractFragmentControl_DeleteSentencePart,
        Resources.AbstractFragmentControl_DeleteSentencePartHead,
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question) != DialogResult.Yes) return;

      if (FragmentDelete != null) FragmentDelete(fragment);
    }
  }
}