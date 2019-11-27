#region

using System.Windows.Forms;
using Telerik.WinControls;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Addon.Views.Abstract
{
  public partial class AbstractViewAddon : UserControl
  {
    public AbstractViewAddon()
    {
      ThemeResolutionService.ApplicationThemeName = "TelerikMetroTouch";
      InitializeComponent();
    }
  }
}