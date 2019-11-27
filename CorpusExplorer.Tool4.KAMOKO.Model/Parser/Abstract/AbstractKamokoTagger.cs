using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.TreeTagger.Abstract;

namespace CorpusExplorer.Tool4.KAMOKO.Model.Parser.Abstract
{
  public abstract class AbstractKamokoTagger : AbstractTreeTagger
  {
    public Course KamokoProperty { get; set; }
    public int SpeakerCount { get; set; } = 8;
  }
}