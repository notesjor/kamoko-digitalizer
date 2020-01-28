using System.Collections.Generic;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Utils.DataTableWriter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Addon.Views.Editor;

namespace CorpusExplorer.Tool4.KAMOKO.Addon
{
  public class AddonRepository : AbstractAddonRepository
  {
    public override IEnumerable<AbstractAdditionalTagger> AddonAdditionalTagger => null;
    public override IEnumerable<KeyValuePair<string, AbstractCorpusBuilder>> AddonBackends => null;
    public override IEnumerable<KeyValuePair<string, AbstractExporter>> AddonExporters => null;
    public override IEnumerable<KeyValuePair<string, AbstractImporter>> AddonImporter => null;
    public override IEnumerable<KeyValuePair<string, AbstractScraper>> AddonScrapers => null;
    public override IEnumerable<KeyValuePair<string, AbstractTableWriter>> AddonTableWriter { get; }
    public override IEnumerable<object> AddonSideloadFeature => null;
    public override IEnumerable<AbstractTagger> AddonTagger => null;
    public override IEnumerable<IAction> AddonConsoleActions => null;

    public override IEnumerable<IAddonView> AddonViews => new IAddonView[]
    {
      new KamokoEditorAddon()
    };

    public override string Guid => "KAMOKO";
  }
}