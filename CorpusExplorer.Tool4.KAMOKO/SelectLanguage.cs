#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  public partial class SelectLanguage : AbstractForm
  {
    private readonly IEnumerable<string> _availableLanguages;

    public SelectLanguage(IEnumerable<string> availableLanguages)
    {
      _availableLanguages = availableLanguages;
      InitializeComponent();
      radDropDownList1.DataSource = _availableLanguages;
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