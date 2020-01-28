#region

using System;
using System.Drawing;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract;
using CorpusExplorer.Tool4.KAMOKO.GUI.Properties;
using CorpusExplorer.Tool4.KAMOKO.Model.Controller;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  public partial class MainForm : AbstractForm
  {
    private readonly Editor _editor;

    public MainForm(ref KamokoController controller)
    {
      controller.New();

      InitializeComponent();
      _editor = new Editor {Size = Size, Location = new Point(0, 0), Dock = DockStyle.Fill, IsReadOnly = false};
      _editor.CurrentSentenceLoad();

      Controls.Add(_editor);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      var ask = MessageBox.Show(
                                Resources.MainForm_AskSave,
                                Resources.MainForm_AskSaveHead,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question);

      switch (ask)
      {
        case DialogResult.Cancel:
          e.Cancel = true;
          break;
        case DialogResult.Yes:
          _editor.Save();
          break;
        case DialogResult.No:
          return;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}