#region

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Terminal.WinForm.Forms.Error;
using CorpusExplorer.Terminal.WinForm.Forms.Splash;
using CorpusExplorer.Terminal.WinForm.Helper;
using CorpusExplorer.Tool4.KAMOKO.Controller;
using CorpusExplorer.Tool4.KAMOKO.Controls;
using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  public partial class MainForm : AbstractForm
  {
    #region Constructors and Destructors

    public MainForm(ref KamokoController controller)
    {
      InitializeComponent();

      _controller = controller;
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

    private void btn_course_saveas_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      var sfd = new SaveFileDialog { Filter = "KAMOKO-XML (*.kamoko.xml)|*.kamoko.xml" };
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        _controller.SavePath = sfd.FileName;
        _controller.Save();
      }
    }

    private void btn_errorConsole_Click(object sender, EventArgs e)
    {
      var form = new ErrorConsole();
      form.ShowDialog();
    }

    private void btn_sentence_source_Click(object sender, EventArgs e)
    {
      var source = _controller.CurrentSource;

      var form = new ChangeSource(_controller.CurrentPageIndexLabel, _controller.CurrentSentenceIndexLabel, source);
      if (form.ShowDialog() != DialogResult.OK) return;

      _controller.CurrentSource = form.Result;
    }

    #region Fields

    private readonly KamokoController _controller;

    private readonly DisposingContainer _dispose = new DisposingContainer();
    private int? _scroll;

    #endregion

    #region Methods

    private void ControlOnFragmentAddConstant(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      _controller.AddFragment(
        fragment,
        new ConstantFragment { Content = "", Index = -1, SpeakerVotes = new List<SpeakerVote>() });

      if (_scroll.HasValue) _scroll += 195;

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

      if (_scroll.HasValue) _scroll += 525;

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
      SuspendLayout();

      _dispose.Dispose<UserControl>(
        x =>
        {
          radScrollablePanel1.Controls.Remove(x);
          x.Dispose();
        });

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
          _dispose.Add(c);
        }
        else if (fragment is VariableFragment)
        {
          var c = new VariusFragmentBlockControl(fragment) { Dock = DockStyle.Top };
          c.FragmentAddConstant += ControlOnFragmentAddConstant;
          c.FragmentAddVariable += ControlOnFragmentAddVariable;
          c.FragmentDelete += ControlOnFragmentDelete;
          c.FragmentSubAdd += OnFragmentSubAdd;

          radScrollablePanel1.Controls.Add(c);
          _dispose.Add(c);
        }
        else
        {
          throw new StrongTypingException();
        }
      }

      if (_scroll.HasValue)
        radScrollablePanel1.VerticalScrollbar.Value = _scroll.Value > radScrollablePanel1.VerticalScrollbar.Maximum
          ? radScrollablePanel1.VerticalScrollbar.Maximum
          : _scroll.Value;

      ResumeLayout(true);
    }

    private void CurrentSentenceSave()
    {
      _scroll = radScrollablePanel1.VerticalScrollbar.Value;

      _controller.SaveSentence(
        radScrollablePanel1.PanelContainer.Controls.OfType<AbstractFragmentControl>()
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
      if (string.IsNullOrEmpty(_controller.SavePath) || !File.Exists(_controller.SavePath))
      {
        btn_course_saveas_Click(sender, e);
      }
      _controller.Save();
    }

    private void btn_document_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.PagePlus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_document_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.PageMinus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_export_Click(object sender, EventArgs e)
    {
      btn_course_save_Click(sender, e);
      
      var sfd = new SaveFileDialog { Filter = "CorpusExplorer v5-Korpus (*.cec5)|*.cec5", FileName = Path.GetFileNameWithoutExtension(_controller.SavePath).Replace(".kamoko","") + ".cec5" };

      if (sfd.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      Processing.SplashShow();
      _controller.Export(sfd.FileName);
      Processing.SplashClose("");
    }

    private void btn_page_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.AddPage();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.AddSentence();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.SentencePlus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      _controller.SentenceMinus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    #endregion

    private void MainForm_Load(object sender, EventArgs e)
    {
      _controller.New();
      CurrentSentenceLoad();
    }
  }
}