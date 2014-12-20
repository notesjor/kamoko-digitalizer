namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  using System;
  using System.Data;

  using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

  public partial class ConstantFragmentBlockControl : AbstractFragmentControl
  {
    #region Fields

    private readonly AbstractFragment _fragment;

    private readonly int _index;

    #endregion

    #region Constructors and Destructors

    public ConstantFragmentBlockControl(AbstractFragment fragment)
    {
      _fragment = fragment;

      InitializeComponent();
      var f = fragment as ConstantFragment;
      if (f == null)
      {
        throw new NoNullAllowedException("fragment");
      }

      _index = f.Index;
      radTextBox1.Text = f.Content;
      voteBarControl1.SetSpeakers(f.SpeakerVotes);
    }

    #endregion

    #region Public Methods and Operators

    public override AbstractFragment GetFragment()
    {
      return new ConstantFragment
               {
                 Index = _index,
                 Content = radTextBox1.Text,
                 SpeakerVotes = voteBarControl1.GetSpeakers()
               };
    }

    #endregion

    #region Methods

    private void btn_item_add_const_Click(object sender, EventArgs e)
    {
      OnAddConstnat(_fragment);
    }

    private void btn_item_add_vars_Click(object sender, EventArgs e)
    {
      OnAddVariable(_fragment);
    }

    private void btn_item_remove_Click(object sender, EventArgs e)
    {
      OnDelete(_fragment);
    }

    #endregion
  }
}