using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CcesStatsCalculator.Stats
{
    class FavoriteWord : IStatCalculator
    {
        public string CharacterName { get; }

        public int MaxFavorites { get; }

        public string StringResult { get; private set; } = string.Empty;

        public Dictionary<string, int> WordsUsage { get; } = new Dictionary<string, int>();

        public FavoriteWord(string characterName, int maxFavorites)
        {
            CharacterName = characterName;
            MaxFavorites = maxFavorites;
        }

        void FormatStringResult()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--- Favorite word ({CharacterName})");

            var orderedWordsUsage = new SortedDictionary<string, int>(WordsUsage).OrderByDescending(x => x.Value);

            for (int i = 0; i < MaxFavorites && i < orderedWordsUsage.Count(); i++)
                sb.AppendLine($"{orderedWordsUsage.ElementAt(i).Key} - {orderedWordsUsage.ElementAt(i).Value}");

            StringResult = sb.ToString();
        }

        public void Calculate(IEnumerable<DialogueLine> lines)
        {
            WordsUsage.Clear();
            StringResult = string.Empty;

            var characterLines = lines.Where(l => l.Character == CharacterName);

            // This could use some... optimization
            // Todo: correctly handle "Foo..." and "...Foo"
            foreach (var l in characterLines)
                l.Line = l.Line.Replace("!", "")
                    .Replace("?", "")
                    .ToLowerInvariant()
                    .Trim();

            var sb = new StringBuilder();
            foreach (var l in characterLines)
                sb.Append(l.Line).Append(" ");
            var words = sb.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            // ...

            foreach (var word in words)
            {
                WordsUsage.TryAdd(word, 0);
                WordsUsage[word] += 1;
            }

            FormatStringResult();
        }
    }
}
