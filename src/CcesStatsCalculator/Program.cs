using CcesStatsCalculator.Stats;
using System;
using System.Collections.Generic;
using System.IO;

namespace CcesStatsCalculator
{
    class Program
    {
        static void CheckArguments(string[] args)
        {
            if (args.Length <= 0)
                throw new ArgumentException(".cces folder path not specified");

            if (args.Length > 1)
                throw new ArgumentException($"To many arguments ({args.Length}); Expected 1 (.cces folder path)");
        }

        static void CalculateStats(string path, IEnumerable<IStatCalculator> stats)
        {
            var fileParser = new DialogueFileParser();

            var lines = new List<DialogueLine>();
            foreach (var file in Directory.GetFiles(path))
                lines.AddRange(fileParser.ParseDialogueFile(file));

            foreach (var stat in stats)
            {
                stat.Calculate(lines);
                Console.WriteLine(stat.StringResult);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                CheckArguments(args);

                var path = args[0];
                // Todo: specify stats through arguments instead of hard-coding
                var stats = new List<IStatCalculator>
                {
                    new WordsPerFile(),
                    new MessagesPerFile(),
                    new FavoriteExpression("Lea", 5),
                    new FavoriteWord("Lea", 5),
                    new FavoriteExpression("Shizuka", 5),
                    new FavoriteWord("Shizuka", 5),
                };
                CalculateStats(path, stats);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
