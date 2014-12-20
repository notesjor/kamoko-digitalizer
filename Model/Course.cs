namespace CorpusExplorer.Tool4.KAMOKO.Model
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  [XmlRoot]
  [Serializable]
  public class Course
  {
    #region Constructors and Destructors

    public Course()
    {
      Documents = new List<Document>();
    }

    #endregion

    #region Public Properties

    [XmlArray]
    public List<Document> Documents { get; set; }

    #endregion
  }
}