using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote.Abstract;
using jor.CorpusExplorer.Kamoko.RawAnnotate.Model;

namespace jor.CorpusExplorer.Kamoko.RawAnnotate.Controler
{
  public static class LayerConversionControler
  {
    private static readonly HashSet<char> _isNumber = new HashSet<char>
    {
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      '0'
    };

    private static Dictionary<int, bool> _speaker;

    public static void Start(string inputPath, string outputPath)
    {
      var docs = ScrapDocuments(inputPath);

      using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
      using (var bs = new BufferedStream(fs))
      {
        var xml = new XmlSerializer(typeof(Course));
        xml.Serialize(bs, docs);
      }
    }

    private static Course ClenupEmptySpeakers(Course course)
    {
      _speaker = new Dictionary<int, bool>();
      foreach (var d in course.Documents)
      foreach (var s in d.Sentences)
      foreach (var f in s.Fragments)
        RecursiveAnalyseFragment(f);

      var remove = (from s in _speaker where !s.Value select s.Key).ToArray();
      if (remove.Length == 0)
        return course;

      var fragments = new Queue<AbstractFragment>();
      foreach (var d in course.Documents)
      foreach (var s in d.Sentences)
      foreach (var f in s.Fragments)
        fragments.Enqueue(f);

      while (fragments.Count > 0)
      {
        var f = fragments.Dequeue();
        if (f is VariableFragment)
        {
          var v = (VariableFragment) f;
          foreach (var x in v.Fragments)
            fragments.Enqueue(x);
        }
        else if (f is ConstantFragment)
        {
          var c = (ConstantFragment) f;
          foreach (var s in remove)
          {
            var first = (from x in c.SpeakerVotes where x.SpeakerIndex == s select x).FirstOrDefault();
            if (first != null)
              c.SpeakerVotes.Remove(first);
          }
        }
      }

      return course;
    }

    private static Sentence Convert(XmlSpeakerCase[] cases)
    {
      var res = new Sentence();

      var items = new List<Dictionary<string, Tuple<int, int, int, int, bool>>>();

      // Parsen
      foreach (var c in cases)
      {
        if (c.Verwerfen)
          continue;

        if (!string.IsNullOrEmpty(c.Meta) && c.Meta.Length > res.Source.Length)
          res.Source = c.Meta;

        var idx = 0;
        var current = 0;

        while (true)
        {
          if (current + 1 >= c.Text.Length)
            break;
          var next = c.Text.IndexOf("<strong>", current, StringComparison.Ordinal);

          if (next > current)
          {
            Insert(ref items, idx, c, current, next);
            idx++;
            current = next;
          }
          else if (next == current)
          {
            current += "<strong>".Length;
            next = c.Text.IndexOf("</strong>", current, StringComparison.Ordinal);

            if (CutOut(c.Text, current, next) != "{XXX}")
              Insert(ref items, idx, c, current, next);
            idx++;
            current = next + "</strong>".Length;
          }
          else if (next == -1)
          {
            Insert(ref items, idx, c, current, c.Text.Length);
            break;
          }
          else
          {
            Console.WriteLine("UNEXPEXTED");
          }
        }
      }

      // Konvertieren
      for (var i = 0; i < items.Count; i++)
        if (items[i].Count == 1)
        {
          // IsOriginal und SpeakerVotes sollten bei konstanten Fragmenten den folgenden default-Wert haben.
          res.Fragments.Add(
                            new ConstantFragment
                            {
                              Content = items[i].FirstOrDefault().Key,
                              Index = i + 1,
                              IsOriginal = false,
                              SpeakerVotes = new List<SpeakerVote>()
                            });
        }
        else
        {
          var container = new VariableFragment();
          foreach (var tuple in items[i])
            container.Fragments.Add(
                                    new ConstantFragment
                                    {
                                      Content = tuple.Key,
                                      Index = i + 1,
                                      IsOriginal = tuple.Value.Item5,
                                      SpeakerVotes = new List<SpeakerVote>
                                      {
                                        new SpeakerVote
                                        {
                                          SpeakerIndex = 1,
                                          Vote =
                                            tuple.Value.Item1 == 30
                                              ? new VoteReservation()
                                              : tuple.Value.Item1 == 20
                                                ? (AbstractVote) new VoteAccept()
                                                : new VoteDenied()
                                        },
                                        new SpeakerVote
                                        {
                                          SpeakerIndex = 2,
                                          Vote =
                                            tuple.Value.Item2 == 30
                                              ? new VoteReservation()
                                              : tuple.Value.Item2 == 20
                                                ? (AbstractVote) new VoteAccept()
                                                : new VoteDenied()
                                        },
                                        new SpeakerVote
                                        {
                                          SpeakerIndex = 3,
                                          Vote =
                                            tuple.Value.Item3 == 30
                                              ? new VoteReservation()
                                              : tuple.Value.Item3 == 20
                                                ? (AbstractVote) new VoteAccept()
                                                : new VoteDenied()
                                        },
                                        new SpeakerVote
                                        {
                                          SpeakerIndex = 4,
                                          Vote =
                                            tuple.Value.Item4 == 30
                                              ? new VoteReservation()
                                              : tuple.Value.Item4 == 20
                                                ? (AbstractVote) new VoteAccept()
                                                : new VoteDenied()
                                        }
                                      }
                                    });
          res.Fragments.Add(container);
        }

      return res;
    }

    private static string CutOut(string text, int start, int end)
    {
      return text.Substring(start, end - start).Trim();
    }

    private static string FormatNumber(string number)
    {
      number = number.ToUpper();
      var index = '0';

      // Wenn es eine A,B,C,...-Variante gibt dann ersetzt diese den 0-Index
      if (!_isNumber.Contains(number[number.Length - 1]))
      {
        index = number[number.Length               - 1];
        number = number.Substring(0, number.Length - 1);
      }

      while (number.Length < 4)
        number = "0" + number;
      number = index + number;

      return number;
    }

    private static void Insert(
      ref List<Dictionary<string, Tuple<int, int, int, int, bool>>> items,
      int index,
      XmlSpeakerCase c,
      int start,
      int next)
    {
      try
      {
        var key = CutOut(c.Text, start, next);

        while (index >= items.Count)
          items.Add(new Dictionary<string, Tuple<int, int, int, int, bool>>());

        if (!items[index].ContainsKey(key))
          items[index].Add(
                           key,
                           new Tuple<int, int, int, int, bool>(
                                                               c.S1_Bedingt ? 30 : c.S1_Zustimmung ? 20 : 10,
                                                               c.S2_Bedingt ? 30 : c.S2_Zustimmung ? 20 : 10,
                                                               c.S3_Bedingt ? 30 : c.S3_Zustimmung ? 20 : 10,
                                                               c.S4_Bedingt ? 30 : c.S4_Zustimmung ? 20 : 10,
                                                               c.Original
                                                              ));
      }
      catch
      {
        // ignore
      }
    }

    private static Course MakeRemix(List<XmlSpeakerCase> cases)
    {
      var res = new Course();

      var docs = new HashSet<string>();
      foreach (var c in cases.Where(c => !docs.Contains(c.DocumentId)))
        docs.Add(c.DocumentId);

      foreach (var doc in docs)
      {
        var nDoc = new Document {Index = string.IsNullOrEmpty(doc) ? "00" : doc.Length == 1 ? "0" + doc : doc};
        var sentences = cases.Where(c => c.DocumentId == doc && !c.Verwerfen).ToList();

        var sens = new HashSet<string>();
        foreach (var c in sentences.Where(c => !sens.Contains(c.SentenceId)))
          sens.Add(c.SentenceId);

        foreach (var sen in sens)
        {
          var nSent = Convert(sentences.Where(c => c.SentenceId == sen).ToArray());
          nSent.Index = sen.Length == 1 ? "0" + sen : sen;

          nDoc.Sentences.Add(nSent);
        }

        nDoc.Sentences.Sort((x, y) => string.Compare(x.Index, y.Index, StringComparison.Ordinal));

        res.Documents.Add(nDoc);
      }

      res.Documents.Sort((x, y) => string.Compare(x.Index, y.Index, StringComparison.Ordinal));

      return res;
    }

    private static void RecursiveAnalyseFragment(AbstractFragment abstractFragment)
    {
      if (abstractFragment is ConstantFragment)
      {
        var cf = (ConstantFragment) abstractFragment;
        foreach (var v in cf.SpeakerVotes)
        {
          if (!_speaker.ContainsKey(v.SpeakerIndex))
          {
            _speaker.Add(v.SpeakerIndex, !(v.Vote is VoteDenied));
            continue;
          }

          if (!(v.Vote is VoteDenied))
            _speaker[v.SpeakerIndex] = true;
        }
      }
      else if (abstractFragment is VariableFragment)
      {
        var vf = (VariableFragment) abstractFragment;
        foreach (var f in vf.Fragments)
          RecursiveAnalyseFragment(f);
      }
    }

    private static Course ScrapDocuments(string path)
    {
      List<XmlSpeakerCase> cases;
      // Lade annotierte Daten...", 20);

      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      using (var bs = new BufferedStream(fs))
      {
        var xml = new XmlSerializer(typeof(List<XmlSpeakerCase>));
        cases = xml.Deserialize(bs) as List<XmlSpeakerCase>;
      }

      // Korrektur erfolgt

      foreach (var c in cases)
        c.Text =
          c.Text
           .Replace("!EMPTY!", "[...]")
           .Replace("<html>", "")
           .Replace("</html>", "");

      // Erstelle Remix (da da da)

      return ClenupEmptySpeakers(MakeRemix(cases));
    }
  }
}