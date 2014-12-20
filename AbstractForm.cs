namespace CorpusExplorer.Tool4.KAMOKO
{
  using Telerik.WinControls;
  using Telerik.WinControls.UI;

  public partial class AbstractForm : RadForm
  {
    #region Constructors and Destructors

    public AbstractForm()
    {
      ThemeResolutionService.ApplicationThemeName = "TelerikMetroTouch";
      InitializeComponent();
    }

    #endregion
  }
}