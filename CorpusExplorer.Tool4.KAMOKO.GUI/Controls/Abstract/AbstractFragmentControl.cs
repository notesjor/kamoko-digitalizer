#region

using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Delegates;
using CorpusExplorer.Tool4.KAMOKO.GUI.Properties;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Abstract
{
  public partial class AbstractFragmentControl : AbstractUserControl
  {
    public AbstractFragmentControl()
    {
      InitializeComponent();
    }

    public event FragmentManipulationDelegate FragmentAddConstant;
    public event FragmentManipulationDelegate FragmentAddVariable;
    public event FragmentManipulationDelegate FragmentDelete;

    public virtual AbstractFragment GetFragment()
    {
      return null;
    }

    protected void OnAddConstnat(AbstractFragment fragment)
    {
      FragmentAddConstant?.Invoke(fragment);
    }

    protected void OnAddVariable(AbstractFragment fragment)
    {
      FragmentAddVariable?.Invoke(fragment);
    }

    protected void OnDelete(AbstractFragment fragment)
    {
      if (MessageBox.Show(
                          Resources.AbstractFragmentControl_DeleteSentencePart,
                          Resources.AbstractFragmentControl_DeleteSentencePartHead,
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question) != DialogResult.Yes)
        return;

      FragmentDelete?.Invoke(fragment);
    }
  }
}