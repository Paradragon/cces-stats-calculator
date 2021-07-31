using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CcesStatsCalculator.Stats
{
    class WordsPerFile : IStatCalculator
    {
        public string StringResult { get; private set; } = string.Empty;

        public Dictionary<string, int> WordCount { get; } = new Dictionary<string, int>();

        void FormatStringResult()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--- Words per file");
            
            foreach(var key in WordCount.Keys)
                sb.AppendLine($"{key} : {WordCount[key]}");

            StringResult = sb.ToString();
        }

        public void Calculate(IEnumerable<DialogueLine> lines)
        {
            WordCount.Clear();
            StringResult = string.Empty;

            var files = lines.Select(l => l.Filename).Distinct();

            foreach (var file in files)
            {
                WordCount[file] = lines.Where(l => l.Filename == file)
                    .Sum(l => l.Line.Split(' ')
                    .Count());
            }

            FormatStringResult();
        }
    }
}
