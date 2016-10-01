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
  public class Course
  {
    public Course() { Documents = new List<Document>(); }

    [XmlArray]
    public List<Document> Documents { get; set; }

    public Document GetDocument(Guid guid) => Documents.FirstOrDefault(d => d.DocumentGuid == guid);
  }
}