using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CcesStatsCalculator.Stats
{
    class MessagesPerFile : IStatCalculator
    {
        public string StringResult { get; private set; } = string.Empty;

        public Dictionary<string, int> MessageCount { get; } = new Dictionary<string, int>();

        void FormatStringResult()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--- Messages per file");
            
            foreach(var key in MessageCount.Keys)
                sb.AppendLine($"{key} : {MessageCount[key]}");

            StringResult = sb.ToString();
        }

        public void Calculate(IEnumerable<DialogueLine> lines)
        {
            MessageCount.Clear();
            StringResult = string.Empty;

            var files = lines.Select(l => l.Filename).Distinct();

            foreach (var file in files)
            {
                MessageCount[file] = lines.Where(l => l.Filename == file).Count();
            }

            FormatStringResult();
        }
    }
}
