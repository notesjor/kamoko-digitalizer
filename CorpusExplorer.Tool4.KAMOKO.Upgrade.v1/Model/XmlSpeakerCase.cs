using System;
using System.Xml.Serialization;

namespace jor.CorpusExplorer.Kamoko.RawAnnotate.Model
{
  [XmlRoot]
  [Serializable]
  public sealed class XmlSpeakerCase : ICloneable
  {
    [XmlAttribute]
    public string DocumentId { get; set; }

    [XmlElement]
    public string Text { get; set; }

    [XmlElement]
    public string Meta { get; set; }

    [XmlAttribute]
    public bool Original { get; set; }

    [XmlAttribute]
    public bool S1_Bedingt { get; set; }

    [XmlAttribute]
    public bool S1_Zustimmung { get; set; }

    [XmlAttribute]
    public bool S2_Bedingt { get; set; }

    [XmlAttribute]
    public bool S2_Zustimmung { get; set; }

    [XmlAttribute]
    public bool S3_Bedingt { get; set; }

    [XmlAttribute]
    public bool S3_Zustimmung { get; set; }

    [XmlAttribute]
    public bool S4_Bedingt { get; set; }

    [XmlAttribute]
    public bool S4_Zustimmung { get; set; }

    [XmlAttribute]
    public string SentenceId { get; set; }


    [XmlAttribute]
    public bool Verwerfen { get; set; }

    /// <summary>
    ///   Erstellt ein neues Objekt, das eine Kopie der aktuellen Instanz darstellt.
    /// </summary>
    /// <returns>
    ///   Ein neues Objekt, das eine Kopie dieser Instanz darstellt.
    /// </returns>
    public object Clone()
    {
      var res = new XmlSpeakerCase
      {
        DocumentId = DocumentId,
        Meta = Meta,
        Original = Original,
        S1_Bedingt = S1_Bedingt,
        S1_Zustimmung = S1_Zustimmung,
        S2_Bedingt = S2_Bedingt,
        S2_Zustimmung = S2_Zustimmung,
        S3_Bedingt = S3_Bedingt,
        S3_Zustimmung = S3_Zustimmung,
        S4_Bedingt = S4_Bedingt,
        S4_Zustimmung = S4_Zustimmung,
        Text = Text,
        SentenceId = SentenceId,
        Verwerfen = Verwerfen
      };
      return res;
    }
  }
}