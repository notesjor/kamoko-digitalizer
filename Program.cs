namespace CorpusExplorer.Tool4.KAMOKO
{
  using System;
  using System.IO;
  using System.Windows.Forms;

  internal static class Program
  {
    #region Methods

    /// <summary>
    ///   Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    #endregion
  }
}