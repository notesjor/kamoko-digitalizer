#region

using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.Controls.Delegates;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controls.Abstract
{
  public partial class AbstractFragmentControl : AbstractUserControl
  {
    #region Constructors and Destructors

    public AbstractFragmentControl()
    {
      InitializeComponent();
    }

    #endregion

    #region Public Methods and Operators

    public virtual AbstractFragment GetFragment()
    {
      return null;
    }

    #endregion

    #region Public Events

    public event FragmentManipulationDelegate FragmentAddConstant;

    public event FragmentManipulationDelegate FragmentAddVariable;

    public event FragmentManipulationDelegate FragmentDelete;

    #endregion

    #region Methods

    protected void OnAddConstnat(AbstractFragment fragment)
    {
      if (FragmentAddConstant != null)
      {
        FragmentAddConstant(fragment);
      }
    }

    protected void OnAddVariable(AbstractFragment fragment)
    {
      if (FragmentAddVariable != null)
      {
        FragmentAddVariable(fragment);
      }
    }

    protected void OnDelete(AbstractFragment fragment)
    {
      if (MessageBox.Show(
        "Möchten Sie wirklich diesen Teilsatz löschen?",
        "Löschen?",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question) != DialogResult.Yes)
      {
        return;
      }

      if (FragmentDelete != null)
      {
        FragmentDelete(fragment);
      }
    }

    #endregion
  }
}