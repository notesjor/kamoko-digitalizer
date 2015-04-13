#region

using System;
using System.Windows.Forms;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Terminal.WinForm.Forms.Error;
using CorpusExplorer.Tool4.KAMOKO.Controller;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  internal static class Program
  {
    #region Methods

    /// <summary>
    ///   Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      var controller = new KamokoController();

      try
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm(ref controller));
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
        var form = new ErrorConsole();
        form.ShowDialog();

        if (controller != null && !string.IsNullOrEmpty(controller.SavePath))
        {
          controller.SavePath += ".emergency";
          controller.Save();
        }
      }
    }

    #endregion
  }
}