#region

using System.Windows.Forms;
using Telerik.WinControls;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Controls.Abstract
{
  public partial class AbstractUserControl : UserControl
  {
    public AbstractUserControl()
    {
      ThemeResolutionService.ApplicationThemeName = "TelerikMetroTouch";
      InitializeComponent();
    }
  }
}