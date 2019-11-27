#region

using Telerik.WinControls;
using Telerik.WinControls.UI;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract
{
  public partial class AbstractForm : RadForm
  {
    public AbstractForm()
    {
      ThemeResolutionService.ApplicationThemeName = "TelerikMetroTouch";
      InitializeComponent();
    }
  }
}