#region

using System;
using System.Windows.Forms;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms;
using CorpusExplorer.Tool4.KAMOKO.Model.Controller;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  internal static class Program
  {
    /// <summary>
    ///   Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      CorpusExplorerEcosystem.InitializeMinimal();
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

        if (!string.IsNullOrEmpty(controller?.SavePath))
        {
          controller.SavePath += ".emergency";
          controller.Save();
        }
      }
    }
  }
}