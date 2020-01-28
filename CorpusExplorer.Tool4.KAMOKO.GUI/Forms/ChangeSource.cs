#region

using System;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  public partial class ChangeSource : AbstractForm
  {
    public ChangeSource(string page, string sentence, string source)
    {
      InitializeComponent();
      txt_source.Text = source;
      radLabel2.Text = $"Folgende Angabe sind für Blatt {page} / Satz {sentence} hinterlegt.";
    }

    public string Result { get; private set; }

    private void btn_abort_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Abort;
      Close();
    }

    private void btn_ok_Click(object sender, EventArgs e)
    {
      Result = txt_source.Text;
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}