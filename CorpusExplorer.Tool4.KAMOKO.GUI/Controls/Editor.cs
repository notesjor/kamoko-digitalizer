#region

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Properties;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Controller;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Helper;
using CorpusExplorer.Tool4.KAMOKO.Model.Parser;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Data;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls
{
  public partial class Editor : AbstractUserControl
  {
    private readonly DisposingContainer _dispose = new DisposingContainer();
    private int _errors;
    private List<Tuple<string, string>> _matches;
    private bool _refreshLock;
    private int? _scroll;

    public Editor()
    {
      InitializeComponent();
      radCollapsiblePanel1.Collapse();
    }

    public KamokoController Controller { get; set; } = new KamokoController();

    public bool IsReadOnly
    {
      get => Controller.IsReadOnly;
      set
      {
        Controller.IsReadOnly = value;
        btn_page_add.Visibility =
          btn_sentence_add.Visibility =
            btn_course_new.Visibility =
              btn_course_open.Visibility =
                btn_course_save.Visibility =
                  btn_course_saveas.Visibility =
                    commandBarSeparator1.Visibility =
                      commandBarSeparator2.Visibility =
                        commandBarSeparator3.Visibility =
                          btn_export.Visibility =
                            btn_errorConsole.Visibility =
                              value ? ElementVisibility.Collapsed : ElementVisibility.Visible;
      }
    }

    public void CurrentSentenceLoad()
    {
      if (!Controller.IsReady)
        return;

      SuspendLayout();

      _dispose.Dispose<UserControl>(
                                    x =>
                                    {
                                      radScrollablePanel1.Controls.Remove(x);
                                      x.Dispose();
                                    });

      RefreshTree();

      txt_index_document.Text = Controller.CurrentPageIndex;
      warn_page.Visibility = Controller.IsDocumentIndexUnique(Controller.CurrentPageIndex)
                               ? ElementVisibility.Collapsed
                               : ElementVisibility.Visible;

      txt_index_sentence.Text = Controller.CurrentSentenceIndex;
      warn_sentence.Visibility = Controller.IsSentenceIndexUnique(
                                                                  Controller.CurrentPageIndex,
                                                                  Controller.CurrentSentenceIndex)
                                   ? ElementVisibility.Collapsed
                                   : ElementVisibility.Visible;

      lbl_page_view.Text = Controller.CurrentPageIndexLabel;
      lbl_sentence_view.Text = Controller.CurrentSentenceIndexLabel;

      var fragments = Controller.CurrentFragments.ToArray();
      for (var i = fragments.Length - 1; i > -1; i--)
      {
        var fragment = fragments[i];

        if (fragment is ConstantFragment)
        {
          var c = new ConstantFragmentBlockControl(fragment, IsReadOnly)
          {
            Dock = DockStyle.Top,
            IsReadOnly = IsReadOnly
          };
          c.FragmentAddConstant += ControlOnFragmentAddConstant;
          c.FragmentAddVariable += ControlOnFragmentAddVariable;
          c.FragmentDelete += ControlOnFragmentDelete;

          radScrollablePanel1.Controls.Add(c);
          _dispose.Add(c);
        }
        else if (fragment is VariableFragment)
        {
          var c = new VariusFragmentBlockControl(fragment, IsReadOnly) {Dock = DockStyle.Top, IsReadOnly = IsReadOnly};
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

    public void LoadCourseInReadOnlyMode(Course course)
    {
      Controller.New();
      CurrentSentenceLoad();
      IsReadOnly = true;
      Controller.Load(course);
      CurrentSentenceLoad();
    }

    public void Save()
    {
      CurrentSentenceSave();
      if (string.IsNullOrEmpty(Controller.SavePath) || !File.Exists(Controller.SavePath))
        btn_course_saveas_Click(null, null);
      Controller.Save();
    }

    private void AddErrorNode(RadTreeNode dn, string doc, RadTreeNode sn, string sen, string message)
    {
      var eNode = sn.Nodes.Add($"<html><p>{message}</p></html>");
      eNode.Tag = $"{doc}|{sen}";
      eNode.Image = Resources.warning;
      sn.Image = Resources.warning;
      dn.Image = Resources.warning;
      _errors++;
    }

    private void btn_course_new_Click(object sender, EventArgs e)
    {
      Controller.New();
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

      if (ofd.ShowDialog() == DialogResult.OK)
        Controller.Load(ofd.FileName);

      CurrentSentenceLoad();
    }

    private void btn_course_save_Click(object sender, EventArgs e)
    {
      Save();
    }

    private void btn_course_saveas_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      var sfd = new SaveFileDialog {Filter = Resources.FileExtension_KAMOKO_XML};
      if (sfd.ShowDialog() != DialogResult.OK)
        return;
      Controller.SavePath = sfd.FileName;
      Controller.Save();
    }

    private void btn_document_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.PagePlus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_document_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.PageMinus();
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
        if (
          MessageBox.Show(
                          $"Aktuell enthält dieser Kurs {_errors} Fehler. Möchten Sie diese vorher korrigieren? Sätze mit Fehlern werden möglicherweise ignoriert, wenn Sie fortfahren.",
                          "Kurs enthält Fehler!",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question) == DialogResult.Yes)
          return;

      var language = new SelectLanguage(new TreeTaggerKamokoTagger(0).LanguagesAvailabel);
      if (language.ShowDialog() != DialogResult.OK)
        return;

      var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Controller.SavePath);
      if (!string.IsNullOrEmpty(fileNameWithoutExtension))
      {
        var sfd = new SaveFileDialog
        {
          Filter = Resources.FileExtension_CEC5,
          FileName = fileNameWithoutExtension.Replace(".kamoko", "") + ".cec6"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
          return;

        Processing.SplashShow();
        Controller.Export(new SimpleKamokoTagger {LanguageSelected = language.Result}, sfd.FileName);
      }

      Processing.SplashClose("");
    }

    private void btn_page_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.AddPage();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_add_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.AddSentence();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_next_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.SentencePlus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_prev_Click(object sender, EventArgs e)
    {
      CurrentSentenceSave();
      Controller.SentenceMinus();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_source_Click(object sender, EventArgs e)
    {
      var source = Controller.CurrentSource;

      var form = new ChangeSource(Controller.CurrentPageIndexLabel, Controller.CurrentSentenceIndexLabel, source);
      if (form.ShowDialog() != DialogResult.OK)
        return;

      Controller.CurrentSource = form.Result;
    }

    private void btn_visualize_Click(object sender, EventArgs e)
    {
      var form = new KamokoFlow(Controller.GetCurrentSentenceForVizPruposes());
      form.ShowDialog();
    }

    private void cmb_filter_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
    {
      tree_course.Filter = string.Empty; // wichtig sonst kein RESET
      switch (e.Position)
      {
        case 1:
          tree_course.Filter = "mehrfach vergeben";
          break;
        case 2:
          tree_course.Filter = "Blatt Nr. mehrfach vergeben";
          break;
        case 3:
          tree_course.Filter = "Satz Nr. mehrfach vergeben";
          break;
        case 4:
          tree_course.Filter = "Konkurrierende Originale";
          break;
        case 5:
          tree_course.Filter = "Negativer Sprecher";
          break;
        case 6:
          tree_course.Filter = "Negative Variante";
          break;
        case 7:
          tree_course.Filter = "Originalstellen abgelehnt";
          break;
        case 8:
          tree_course.Filter = "Keine Kombinationsmöglichkeiten";
          break;
        case 9:
          tree_course.Filter = "(x > 1000)";
          break;
        case 10:
          tree_course.Filter = "Variante ohne Wertung";
          break;
        case 11:
          tree_course.Filter = "Leere Variante (löschbar?)";
          break;
        default:
          tree_course.Filter = string.Empty;
          break;
      }
    }

    private void ControlOnFragmentAddConstant(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      Controller.AddFragment(
                             fragment,
                             new ConstantFragment {Content = "", Index = -1, SpeakerVotes = new List<SpeakerVote>()});

      if (_scroll.HasValue)
        _scroll += 195;

      CurrentSentenceLoad();
    }

    private void ControlOnFragmentAddVariable(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      Controller.AddFragment(
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

      if (_scroll.HasValue)
        _scroll += 525;

      CurrentSentenceLoad();
    }

    private void ControlOnFragmentDelete(AbstractFragment fragment)
    {
      CurrentSentenceSave();
      if (Controller.CurrentFragments.Count() < 2)
      {
        MessageBox.Show(
                        Resources.MainForm_DeltePartSentenceNotAccepted,
                        Resources.MainForm_DeltePartSentenceNotAcceptedHead,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        return;
      }

      Controller.DeleteFragment(fragment);
      CurrentSentenceLoad();
    }

    private void CurrentSentenceSave()
    {
      _scroll = radScrollablePanel1.VerticalScrollbar.Value;

      Controller.SaveSentence(
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

    private void RefreshTree()
    {
      _errors = 0;
      var combiantions = 0;

      tree_course.SuspendLayout();
      tree_course.Nodes.Clear();
      var docs = Controller.GetDocuments();
      if (docs == null) return;

      foreach (var doc in docs)
        try
        {
          var dNode = tree_course.Nodes.Add(doc);
          // Überprüft, ob die DocumentId eindeutig ist.
          if (!Controller.IsDocumentIndexUnique(doc))
          {
            dNode.Image = Resources.warning;
            dNode.Text = $"<html><p>{dNode.Text} Blatt Nr. mehrfach</p></html>";
            _errors++;
          }

          var sens = Controller.GetSentences(doc);
          if (sens != null && sens.Any())
            dNode.Tag = $"{doc}|{sens.FirstOrDefault()}";

          foreach (var sen in sens)
          {
            var sNode = dNode.Nodes.Add(sen);
            sNode.Tag = $"{doc}|{sen}";
            // Überprüft, ob der Satz mehrmals im Kurs vorkommt
            if (!Controller.IsSentenceUnique(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Satz mehrfach digitalisiert");

            // Überprüft, ob die SentenceId eindeutig ist.
            if (!Controller.IsSentenceIndexUnique(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Satz Nr. mehrfach vergeben");

            // Überprüft, ob es alternative Originalstellen gibt.
            if (Controller.CheckAlternativeOriginalFlow(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Konkurrierende Originale");

            // Überprüft, ob es Bewerter gibt, die nur negativ werten.
            if (Controller.CheckNegativeVoter(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Negativer Sprecher");

            // Überprüft, ob es variante Stellen gibt die komplett negativ bewertet werden.
            if (Controller.CheckNegativeVariant(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Negative Variante");

            // Überprüft, ob es Originale gibt, die einstimmig abgelehnt werden.
            if (Controller.CheckRejectedOriginals(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Originalstellen abgelehnt");

            // Überprüft, ob es Originale gibt, die einstimmig abgelehnt werden.
            if (Controller.CheckVariantHasNoVotes(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Variante ohne Wertung");

            // Überprüft, ob es Originale gibt, die einstimmig abgelehnt werden.
            if (Controller.CheckEmptyVariant(doc, sen))
              AddErrorNode(dNode, doc, sNode, sen, "Leere Variante (löschbar?)");

            // Überprüft, ob ein Überlauf der Kombinationen zu erwarten ist.
            var c = Controller.GetSentenceCombinations(doc, sen);
            if (c < 2)
              AddErrorNode(dNode, doc, sNode, sen, "Keine Kombinationsmöglichkeiten");
            else if (c > 1000) AddErrorNode(dNode, doc, sNode, sen, "Mehr als 1000 mögliche Kombinationen");
            var cNode = sNode.Nodes.Add($"<html><p><strong>{c}</strong> mögliche Kombinationen</p></html>");
            cNode.Tag = $"{doc}|{sen}";

            if (c < 1000 && c > 1) combiantions += c;
          }
        }
        catch (Exception ex)
        {
          InMemoryErrorConsole.Log(ex);
        }

      tree_course.CollapseAll();
      tree_course.ResumeLayout();

      lbl_errors.Text = $"<html><p>Fehler: &nbsp;<strong>{_errors}</strong></html>";
      lbl_combinations.Text = $"<html><p>Kombinationen: &nbsp;<strong>{combiantions}</strong></html>";
    }

    private void tree_course_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e)
    {
      var indices = e.Node.Tag.ToString().Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
      if (indices.Length != 2)
        return;

      CurrentSentenceSave();
      Controller.Navigate(indices[0], indices[1]);
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void Navigate(string doc, string sen)
    {
      CurrentSentenceSave();
      Controller.Navigate(doc, sen);
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void list_results_ItemMouseClick(object sender, ListViewItemEventArgs e)
    {
      if (_refreshLock)
        return;

      if (_matches == null || list_results.SelectedIndex < 0 || list_results.SelectedIndex >= _matches.Count)
        return;

      Navigate(_matches[list_results.SelectedIndex].Item1, _matches[list_results.SelectedIndex].Item2);
    }

    private void btn_search_Click(object sender, EventArgs e)
    {
      _refreshLock = true;
      list_results.Items.Clear();

      _matches = radio_startsWith.IsChecked
                   ? Controller.SearchStartsWith(txt_query.Text)
                   : Controller.SearchContains(txt_query.Text);

      if (_matches == null)
        return;

      list_results.SuspendLayout();
      list_results.SuspendUpdate();
      foreach (var match in _matches)
        list_results.Items.Add(new ListViewDataItem($"Blatt: {match.Item1} / Satz: {match.Item2}"));
      list_results.ResumeUpdate();
      list_results.ResumeLayout(false);
      _refreshLock = false;
    }

    private void btn_page_del_Click(object sender, EventArgs e)
    {
      if(!DeleteQuestion("das Blatt"))
        return;

      Controller.RemovePage();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private void btn_sentence_del_Click(object sender, EventArgs e)
    {
      if(!DeleteQuestion("den Satz"))
        return;

      Controller.RemoveSentence();
      _scroll = null;
      CurrentSentenceLoad();
    }

    private bool DeleteQuestion(string what)
    => MessageBox.Show($"Möchten Sie {what} wirklich löschen?", "Löschen?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
  }
}