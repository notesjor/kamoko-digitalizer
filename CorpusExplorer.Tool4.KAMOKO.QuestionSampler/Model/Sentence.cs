using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusExplorer.Tool4.KAMOKO.QuestionSampler.Model
{
  public class QuestSentence
  {
    public List<AbstractFragment> Fragments { get; set; } = new List<AbstractFragment>();

    public void AddConstant(string text) => Fragments.Add(new Constant { Id = Fragments.Count, Text = text });

    public void AddOption(string[] texts, int[] votes) => Fragments.Add(new Option {Id = Fragments.Count, Texts = texts, Votes = votes});
  }
}
