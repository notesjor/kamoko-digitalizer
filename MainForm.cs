namespace CorpusExplorer.Tool4.KAMOKO
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;
  using System.Windows.Forms;

  using CorpusExplorer.Tool4.KAMOKO.Controller;
  using CorpusExplorer.Tool4.KAMOKO.Controls;
  using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
  using CorpusExplorer.Tool4.KAMOKO.Model;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
  using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

  public partial class MainForm : AbstractForm
  {
    #region Fields

    private readonly KamokoController _controller = new KamokoController();

    #endregion

    #region Constructors and Destructors

    public MainForm()
    {
      InitializeComponent();
    }

    #endregion

    #region Methods

    private void ControlOnFragmentAddConstant(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      _controller.AddFragment(
        fragment,
        new ConstantFragment { Content = "", Index = -1, SpeakerVotes = new List<SpeakerVote>() });
      CurrentSentenceLoad();
    }

    private void ControlOnFragmentAddVariable(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      _controller.AddFragment(
        fragment,
        new VariableFragment
          {
            Fragments =
              new List<AbstractFragment>
                {
                  new ConstantFragment
                    {
                      Content = "",
                      Index = 1,
                      SpeakerVotes =
                        new List<SpeakerVote>()
                    }
                }
          });
      CurrentSentenceLoad();
    }

    private void ControlOnFragmentDelete(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      if (_controller.CurrentFragments.Count() < 2)
      {
        MessageBox.Show(
          "Sie können diesen Teilsatz nicht löschen, da es der letzte Teilsatz in diesem Satz ist.",
          "Löschen nicht möglich!",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
        return;
      }

      _controller.DeleteFragment(fragment);
      CurrentSentenceLoad();
    }

    private void CurrentSentenceLoad()
    {
      radScrollablePanel1.Controls.Clear();

      txt_index_document.Text = _controller.CurrentPageIndex;
      txt_index_sentence.Text = _controller.CurrentSentenceIndex;

      lbl_page_view.Text = _controller.CurrentPageIndexLabel;
      lbl_sentence_view.Text = _controller.CurrentSentenceIndexLabel;

      var fragments = _controller.CurrentFragments.ToArray();
      for (var i = fragments.Length - 1; i > -1; i--)
      {
        var fragment = fragments[i];

        if (fragment is ConstantFragment)
        {
          var c = new ConstantFragmentBlockControl(fragment) { Dock = DockStyle.Top };
          c.FragmentAddConstant += ControlOnFragmentAddConstant;
          c.FragmentAddVariable += ControlOnFragmentAddVariable;
          c.FragmentDelete += ControlOnFragmentDelete;

          radScrollablePanel1.Controls.Add(c);
        }
        else if (fragment is VariableFragment)
        {
          var c = new VariusFragmentBlockControl(fragment) { Dock = DockStyle.Top };
          c.FragmentAddConstant += ControlOnFragmentAddConstant;
          c.FragmentAddVariable += ControlOnFragmentAddVariable;
          c.FragmentDelete += ControlOnFragmentDelete;
          c.FragmentSubAdd += OnFragmentSubAdd;

          radScrollablePanel1.Controls.Add(c);
        }
        else
        {
          throw new StrongTypingException();
        }
      }
    }

    private void CurrentSentenceSave()
    {
      _controller.SaveSentence(
        this.radScrollablePanel1.PanelContainer.Controls.OfType<AbstractFragmentControl>()
          .Select(afc => afc.GetFragment())
          .ToList(),
        txt_index_document.Text,
        txt_index_sentence.Text);
    }

    private void OnFragmentSubAdd(object sender, EventArgs eventArgs)
    {
      CurrentSentenceSave();
      CurrentSentenceLoad();
    }

    private void btn_course_new_Click(object sender, EventArgs e)
    {
      _controller.New();
      CurrentSentenceLoad();
    }

    private void btn_course_open_Click(object sender, EventArgs e)
    {
      var ofd = new OpenFileDialog
                  {
                    Filter = "KAMOKO-XML (*.kamoko.xml)|*.kamoko.xml",
                    CheckFileExists = true,
                    CheckPathExists = true
                  };

      if (ofd.ShowDialog() == DialogResult.OK)
      {
        _controller.Load(ofd.FileName);
      }

      CurrentSentenceLoad();
    }

    private void btn_course_save_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      var sfd = new SaveFileDialog { Filter = "KAMOKO-XML (*.kamoko.xml)|*.kamoko.xml" };
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        _controller.Save(sfd.FileName);
      }
    }

    private void btn_document_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.PagePlus();
      CurrentSentenceLoad();
    }

    private void btn_document_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.PageMinus();
      CurrentSentenceLoad();
    }

    private void btn_export_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      var sfd = new SaveFileDialog { Filter = "CorpusExplorer v5-Korpus (*.cec5)|*.cec5" };

      if (sfd.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      _controller.Export(sfd.FileName);
    }

    private void btn_page_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.AddPage();
      CurrentSentenceLoad();
    }

    private void btn_sentence_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.AddSentence();
      CurrentSentenceLoad();
    }

    private void btn_sentence_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.SentencePlus();
      CurrentSentenceLoad();
    }

    private void btn_sentence_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.SentenceMinus();
      CurrentSentenceLoad();
    }

    #endregion

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      var ask = MessageBox.Show(
        "Möchten Sie den Kurs speichern?",
        "Speichern und Beenden",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question);

      switch (ask)
      {
        case DialogResult.Cancel:
          e.Cancel = true;
          break;
        case DialogResult.Yes:
          btn_course_save_Click(null, null);
          break;
        case DialogResult.No:
          return;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}