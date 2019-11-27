#region

using System;
using System.Data;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls
{
  public partial class ConstantFragmentBlockControl : AbstractFragmentControl
  {
    private readonly AbstractFragment _fragment;
    private readonly int _index;

    private bool _isReadOnly;

    public ConstantFragmentBlockControl(AbstractFragment fragment, bool isReadOnly)
    {
      _fragment = fragment;

      InitializeComponent();
      IsReadOnly = isReadOnly;

      var f = fragment as ConstantFragment;
      if (f == null)
        throw new NoNullAllowedException("fragment");

      _index = f.Index;
      radTextBox1.Text = f.Content;
      radCheckBox1.Checked = f.IsOriginal;

      voteBarControl1.SetSpeakers(f.SpeakerVotes);
    }

    public bool IsReadOnly
    {
      get => _isReadOnly;
      set
      {
        _isReadOnly = value;
        btn_item_remove.Visible = btn_item_add.Visible = !value;
        voteBarControl1.IsReadOnly = radCheckBox1.ReadOnly = value;
      }
    }

    public override AbstractFragment GetFragment()
    {
      return new ConstantFragment
      {
        Index = _index,
        Content = radTextBox1.Text,
        IsOriginal = radCheckBox1.Checked,
        SpeakerVotes = voteBarControl1.GetSpeakers()
      };
    }

    private void btn_item_add_const_Click(object sender, EventArgs e)
    {
      btn_item_add.HideDropDown();
      OnAddConstnat(_fragment);
    }

    private void btn_item_add_vars_Click(object sender, EventArgs e)
    {
      btn_item_add.HideDropDown();
      OnAddVariable(_fragment);
    }

    private void btn_item_remove_Click(object sender, EventArgs e)
    {
      OnDelete(_fragment);
    }
  }
}