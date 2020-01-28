using System.Collections.Generic;
using System.Linq;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Tool4.KAMOKO.Model;

namespace CorpusExplorer.Tool4.KAMOKO.Addon.Views.Helper
{
  public static class SelectionToCourseHelper
  {
    public static Course Convert(Selection selection)
    {
      var res = new Course {Documents = new List<Document>()};

      foreach (var csel in selection)
      {
        var c = selection.GetCorpus(csel.Key);
        // Lade KAMOKO Daten aus Korpus falls vorhanden
        var k = c.GetCorpusMetadata("KAMOKO");
        var a = k?.ToString();
        if (string.IsNullOrEmpty(a)) continue;
        // Deserialisiere die KAMKO Daten
        var kamoko = Serializer.DeserializeFromBase64String<Course>(a);
        if (kamoko == null) continue;

        foreach (var dsel in csel.Value)
        {
          var meta = c.GetDocumentMetadata(dsel);
          if (meta == null               ||
              !meta.ContainsKey("Blatt") ||
              meta["Blatt"] == null      ||
              !meta.ContainsKey("Satz")
           || meta["Satz"] == null)
            continue;

          // Rufe Blatt/Satz aus Metadaten (Korpus) ab und greife auf KAMOKO-Daten zu (bKam, sKam).
          var b = meta["Blatt"].ToString();
          var bKam = kamoko.Documents.FirstOrDefault(x => x.Index == b);
          if (bKam == null) continue;

          var s = meta["Satz"].ToString();
          var sKam = bKam.Sentences.FirstOrDefault(x => x.Index == s);
          if (sKam == null) continue;

          // Vermeide Dopplungen (Blatt)
          var bRes = res.Documents.FirstOrDefault(x => x.Index == b);
          if (bRes == null)
          {
            res.Documents.Add(
                              new Document
                              {
                                DocumentGuid = bKam.DocumentGuid,
                                Index = b,
                                Sentences = new List<Sentence>() // Sätze werden selektiv übergeben (siehe unten)
                              });
            // AKtualisieren (notwendig)
            bRes = res.Documents.FirstOrDefault(x => x.Index == b);
          }

          // Vermeide Dopplungen (Satz)
          var sRes = bRes?.Sentences.FirstOrDefault(x => x.Index == s);
          if (sRes != null) continue;
          bRes?.Sentences.Add(
                              new Sentence
                              {
                                SentenceGuid = sKam.SentenceGuid,
                                Index = s,
                                Source = sKam.Source,
                                Fragments = sKam.Fragments
                              });
        }
      }

      return res;
    }
  }
}