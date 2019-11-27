using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Tool4.KAMOKO.Addon.Properties;
using CorpusExplorer.Tool4.KAMOKO.Addon.Views.Helper;

namespace CorpusExplorer.Tool4.KAMOKO.Addon.Views.Editor
{
  public class KamokoEditorAddon : IAddonView
  {
    private readonly GUI.Controls.Editor _editor = new GUI.Controls.Editor {IsReadOnly = true};

    public Image Image24X24 => Resources.note_text_edit;
    public Image Image48X48 => Resources.note_text_edit2;
    public string Label => "KAMOKO-Betrachter";

    public void Initialize(IAddonViewContainer addonViewContainer)
    {
      addonViewContainer.Add(_editor);
    }

    public void Refresh(Selection selection)
    {
      var course = SelectionToCourseHelper.Convert(selection);

      if (course?.Documents?.Any(d => d.Sentences != null && d.Sentences.Count > 0) ?? false)
        _editor.LoadCourseInReadOnlyMode(course);
      else
        MessageBox.Show(
                        "Der Schnappschuss enthält kein Korpumaterial, aus dem KAMOKO-Projekt (http://kamoko.uni-kassel.de)");
    }
  }
}