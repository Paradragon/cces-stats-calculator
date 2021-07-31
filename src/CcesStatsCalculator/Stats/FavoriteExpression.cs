using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CcesStatsCalculator.Stats
{
    class FavoriteExpression : IStatCalculator
    {
        public string CharacterName { get; }

        public int MaxFavorites { get; }

        public string StringResult { get; private set; } = string.Empty;

        public Dictionary<string, int> ExpressionsUsage { get; } = new Dictionary<string, int>();

        public FavoriteExpression(string characterName, int maxFavorites)
        {
            CharacterName = characterName;
            MaxFavorites = maxFavorites;
        }

        void FormatStringResult()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--- Favorite expression ({CharacterName})");

            var orderedExpressionsUsage = new SortedDictionary<string, int>(ExpressionsUsage).OrderByDescending(x => x.Value);
             
            for (int i = 0; i < MaxFavorites && i < orderedExpressionsUsage.Count(); i++)
                sb.AppendLine($"{orderedExpressionsUsage.ElementAt(i).Key} - {orderedExpressionsUsage.ElementAt(i).Value}");

            StringResult = sb.ToString();
        }

        public void Calculate(IEnumerable<DialogueLine> lines)
        {
            ExpressionsUsage.Clear();
            StringResult = string.Empty;

            var characterLines = lines.Where(l => l.Character == CharacterName);

            foreach (var line in characterLines)
            {
                ExpressionsUsage.TryAdd(line.Expression, 0);
                ExpressionsUsage[line.Expression] += 1;
            }

            FormatStringResult();
        }
    }
}
