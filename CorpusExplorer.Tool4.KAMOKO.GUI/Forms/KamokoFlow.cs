using System.Drawing;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model;

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  public partial class KamokoFlow : AbstractForm
  {
    public KamokoFlow(Sentence sentence)
    {
      InitializeComponent();

      SuspendLayout();

      var kf = new KamokoFlowVisualizer
      {
        Size = Size,
        Location = new Point(0, 0),
        Dock = DockStyle.Fill
      };
      Controls.Add(kf);

      kf.ShowSentence(sentence);

      ResumeLayout(false);
    }
  }
}