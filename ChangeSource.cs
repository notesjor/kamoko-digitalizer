#region

using System;
using System.Windows.Forms;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  public partial class ChangeSource : AbstractForm
  {
    public ChangeSource(string page, string sentence, string source)
    {
      InitializeComponent();
      txt_source.Text = source;
      radLabel2.Text = string.Format("Folgende Angabe sind für Blatt {0} / Satz {1} hinterlegt.", page, sentence);
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