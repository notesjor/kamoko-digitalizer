#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  [XmlRoot]
  [Serializable]
  public class Document
  {
    public Document()
    {
      Index = "1";
      Sentences = new List<Sentence>();
      DocumentGuid = Guid.NewGuid();
    }

    [XmlAttribute]
    public string Index { get; set; }

    [XmlAttribute]
    public Guid DocumentGuid { get; set; }

    [XmlArray]
    public List<Sentence> Sentences { get; set; }

    public Sentence GetSentence(Guid guid) => Sentences.FirstOrDefault(s => s.SentenceGuid == guid);
  }
}