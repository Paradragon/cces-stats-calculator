using System.Collections.Generic;

namespace CcesStatsCalculator.Stats
{
    interface IStatCalculator
    {
        string StringResult { get; }
        
        void Calculate(IEnumerable<DialogueLine> lines);
    }
}
