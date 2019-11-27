#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  public partial class SelectLanguage : AbstractForm
  {
    public SelectLanguage(IEnumerable<string> availableLanguages)
    {
      InitializeComponent();
      radDropDownList1.DataSource = availableLanguages;
    }

    public string Result { get; private set; }

    private void btn_abort_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Abort;
      Close();
    }

    private void btn_ok_Click(object sender, EventArgs e)
    {
      Result = radDropDownList1.SelectedItem.Text;
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}