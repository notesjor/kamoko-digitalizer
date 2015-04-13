#region

using Telerik.WinControls;
using Telerik.WinControls.UI;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
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