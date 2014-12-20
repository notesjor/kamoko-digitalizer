namespace CorpusExplorer.Tool4.KAMOKO.Controls.Abstract
{
  using System.Windows.Forms;

  using Telerik.WinControls;

  public partial class AbstractUserControl : UserControl
  {
    #region Constructors and Destructors

    public AbstractUserControl()
    {
      ThemeResolutionService.ApplicationThemeName = "TelerikMetroTouch";
      InitializeComponent();
    }

    #endregion
  }
}