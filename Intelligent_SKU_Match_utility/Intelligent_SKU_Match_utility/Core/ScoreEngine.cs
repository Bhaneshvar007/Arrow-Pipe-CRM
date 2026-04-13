using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Core
{
    internal class ScoreEngine
    {
        public static double Calculate(double fullTextRank, int fuzzy, bool dimensionMatch)
        {
            //double normalizedRank = Math.Min((fullTextRank / 1000.0) * 100, 100);
            double score = 0;

            score += fullTextRank * 0.4;
            score += fuzzy * 0.4;

            if (dimensionMatch)
                score += 20;

            return score;
        }
    }
    
}
