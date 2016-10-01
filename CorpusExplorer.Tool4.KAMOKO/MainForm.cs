#region

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Tool4.KAMOKO.Controller;
using CorpusExplorer.Tool4.KAMOKO.Controls;
using CorpusExplorer.Tool4.KAMOKO.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Helper;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Properties;
using Telerik.WinControls;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO
{
  public partial class MainForm : AbstractForm
  {
    private readonly KamokoController _controller;
    private readonly DisposingContainer _dispose = new DisposingContainer();
    private int? _scroll;
    private int _errors = 0;

    public MainForm(ref KamokoController controller)
    {
      InitializeComponent();
      radCollapsiblePanel1.Collapse();

      _controller = controller;
    }

    private void RefreshTree()
    {
      _errors = 0;
      var combiantions = 0;

      tree_course.SuspendLayout();
      tree_course.Nodes.Clear();
      var docs = _controller.GetDocuments();

      foreach (var doc in docs)
      {
        try
        {
          var pNode = tree_course.Nodes.Add(doc);
          if (!_controller.IsDocumentIndexUnique(doc))
          {
            pNode.Image = Resources.warning;
            _errors++;
          }

          var sens = _controller.GetSentences(doc);
          if (sens != null && sens.Any())
            pNode.Tag = $"{doc}|{sens.First()}";

          foreach (var sen in sens)
          {
            var sNode = pNode.Nodes.Add(sen);
            sNode.Tag = $"{doc}|{sen}";
            if (!_controller.IsSentenceIndexUnique(doc, sen))
            {
              sNode.Image = Resources.warning;
              pNode.Image = Resources.warning;
              _errors++;
            }

            var c = _controller.GetSentenceCombinations(doc, sen);
            var cNode = sNode.Nodes.Add($"<html><p>Kombinationen: &nbsp;<strong>{c}</strong></html>");
            cNode.Tag = $"{doc}|{sen}";

            if (c > 1000)
            {
              cNode.Image = Resources.warning;
              sNode.Image = Resources.warning;
              pNode.Image = Resources.warning;
              _errors++;
            }

            combiantions += c;
          }
        }
        catch (Exception ex)
        {
          InMemoryErrorConsole.Log(ex);
        }
      }

      tree_course.CollapseAll();
      tree_course.ResumeLayout();

      lbl_combinations.Text = $"<html><p>Kombinationen: &nbsp;<strong>{combiantions}</strong></html>";
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
        Filter = Resources.FileExtension_KAMOKO_XML,
        CheckFileExists = true,
        CheckPathExists = true
      };

      if (ofd.ShowDialog() == DialogResult.OK) _controller.Load(ofd.FileName);

      CurrentSentenceLoad();
    }

    private void btn_course_save_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      if (string.IsNullOrEmpty(_controller.SavePath) ||
          !File.Exists(_controller.SavePath)) btn_course_saveas_Click(sender, e);
      _controller.Save();
    }

    private void btn_course_saveas_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      var sfd = new SaveFileDialog { Filter = Resources.FileExtension_KAMOKO_XML };
      if (sfd.ShowDialog() != DialogResult.OK) return;
      _controller.SavePath = sfd.FileName;
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

    private void btn_errorConsole_Click(object sender, EventArgs e)
    {
      var form = new ErrorConsole();
      form.ShowDialog();
    }

    private void btn_export_Click(object sender, EventArgs e)
    {
      btn_course_save_Click(sender, e);

      if (_errors > 0)
        if (MessageBox.Show($"Aktuell enthält dieser Kurs {_errors} Fehler. Möchten Sie diese vorher korrigieren? Sätze mit einer Kombinationsvielfalt größer 1000 werden ignoriert, wenn Sie diesen Prozess fortsetzen.", "Kurs enthält Fehler!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          return;

      var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_controller.SavePath);
      if (!string.IsNullOrEmpty(fileNameWithoutExtension))
      {
        var sfd = new SaveFileDialog
        {
          Filter = Resources.FileExtension_CEC5,
          FileName = fileNameWithoutExtension.Replace(".kamoko", "") + ".cec5"
        };

        if (sfd.ShowDialog() != DialogResult.OK) return;

        Processing.SplashShow();
        _controller.Export(sfd.FileName);
      }
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

    private void btn_sentence_source_Click(object sender, EventArgs e)
    {
      var source = _controller.CurrentSource;

      var form = new ChangeSource(_controller.CurrentPageIndexLabel, _controller.CurrentSentenceIndexLabel, source);
      if (form.ShowDialog() != DialogResult.OK) return;

      _controller.CurrentSource = form.Result;
    }

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
          Resources.MainForm_DeltePartSentenceNotAccepted,
          Resources.MainForm_DeltePartSentenceNotAcceptedHead,
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

      RefreshTree();

      txt_index_document.Text = _controller.CurrentPageIndex;
      warn_page.Visibility = _controller.IsDocumentIndexUnique(_controller.CurrentPageIndex)
                               ? ElementVisibility.Collapsed
                               : ElementVisibility.Visible;

      txt_index_sentence.Text = _controller.CurrentSentenceIndex;
      warn_sentence.Visibility = _controller.IsSentenceIndexUnique(_controller.CurrentPageIndex, _controller.CurrentSentenceIndex)
                               ? ElementVisibility.Collapsed
                               : ElementVisibility.Visible;

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
        else throw new StrongTypingException();
      }

      if (_scroll.HasValue)
      {
        radScrollablePanel1.VerticalScrollbar.Value = _scroll.Value > radScrollablePanel1.VerticalScrollbar.Maximum
          ? radScrollablePanel1.VerticalScrollbar.Maximum
          : _scroll.Value;
      }

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
          btn_course_save_Click(null, null);
          break;
        case DialogResult.No:
          return;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      _controller.New();
      CurrentSentenceLoad();
    }

    private void OnFragmentSubAdd(object sender, EventArgs eventArgs)
    {
      CurrentSentenceSave();
      CurrentSentenceLoad();
    }

    private void tree_course_NodeMouseDoubleClick(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
    {
      var indices = e.Node.Tag.ToString().Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
      if (indices.Length != 2)
        return;

      CurrentSentenceSave();
      _controller.Navigate(indices[0], indices[1]);
      _scroll = null;
      CurrentSentenceLoad();
    }
  }
}