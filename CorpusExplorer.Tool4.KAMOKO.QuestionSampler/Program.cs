using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Vote;
using CorpusExplorer.Tool4.KAMOKO.QuestionSampler.Model;
using Newtonsoft.Json;

namespace CorpusExplorer.Tool4.KAMOKO.QuestionSampler
{
  class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      var ofd = new OpenFileDialog();
      ofd.ShowDialog();

      var sen = new List<Sentence>();

      foreach (var path in ofd.FileNames)
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
          var xml = new XmlSerializer(typeof(Course));
          var course = xml.Deserialize(fs) as Course;

          foreach (var d in course.Documents)
            foreach (var s in d.Sentences)
            {
              var valid = true;
              foreach (var f in s.Fragments)
              {
                if (f is ConstantFragment)
                  continue;

                if (f is VariableFragment v)
                {
                  if (v.Fragments.Any(x => x is VariableFragment)) // Keine zu komplexen Beispiele auswählen
                  {
                    valid = false;
                    break;
                  }

                  foreach (var o in v.Fragments) // optionen
                  {
                    var option = o as ConstantFragment;
                    if (option == null)
                      continue;

                    if (option.SpeakerVotes.Count < 2)
                    {
                      valid = false;
                      break;
                    }

                    var first = option.SpeakerVotes.First().Vote; // Wird (A) für die Überprüfung für gleiches Urteil UND (B) als Wert für QuestionSentence verwendet.
                    if (first is VoteReservation) // Akzeptiere keine "Bedingte Zustimmung"
                    {
                      valid = false;
                      break;
                    }

                    for (var i = 1; i < option.SpeakerVotes.Count; i++) // Alle Muttersprachler müssen das gleiche Urteil fällen.
                    {
                      if (option.SpeakerVotes[i].Vote.Level != first.Level)
                      {
                        valid = false;
                        break;
                      }
                    }

                    if (!valid)
                      break;
                  }

                  if (!valid)
                    break;
                }
              }

              sen.Add(s);
            }
        }

      var sfd = new SaveFileDialog { Filter = "KAMOKO-QuestionSample (*.kamokoQuest)|*.kamokoQuest" };
      sfd.ShowDialog();

      var quests = new List<QuestSentence>();
      foreach (var s in sen)
      {
        var ns = new QuestSentence();
        foreach (var f in s.Fragments)
        {
          if (f is ConstantFragment c)
            ns.AddConstant(c.Content);

          if (!(f is VariableFragment v))
            continue;

          var texts = new List<string>();
          var votes = new List<int>();

          foreach (var cf in v.Fragments.OfType<ConstantFragment>())
          {
            if (cf.SpeakerVotes.Count < 2)
              continue;

            texts.Add(cf.Content);
            votes.Add(cf.SpeakerVotes.First().Vote.Level == 10 ? 1 : -1);
          }

          if (votes.Count == 0)
          {
            ns = null;
            break;
          }

          ns.AddOption(texts.ToArray(), votes.ToArray());
        }

        if (ns != null)
          quests.Add(ns);
      }

      File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(quests.ToArray()), Encoding.UTF8);
    }
  }
}
