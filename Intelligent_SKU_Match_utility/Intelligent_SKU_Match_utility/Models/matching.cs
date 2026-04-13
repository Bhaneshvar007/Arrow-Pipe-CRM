using FuzzySharp;
using Intelligent_SKU_Match_utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Model
{
    public class matching
    {
        public int CalculateFuzzyScore(string input, string skuDescription)
        {
            return Fuzz.TokenSortRatio(input, skuDescription);
        }

        public SKU GetBestMatch(string input, List<SKU> candidates)
        {
            return candidates
                .Select(s => new
                {
                    SKU = s,
                    Score = Fuzz.TokenSortRatio(input, s.Description)
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault()?.SKU;
        }
        public double CalculateFinalScore(
    double fullTextRank,
    int fuzzyScore,
    bool exactDimensionMatch)
        {
            double score = 0;

            score += (fullTextRank * 0.4);
            score += (fuzzyScore * 0.4);

            if (exactDimensionMatch)
                score += 20;

            return score;
        }

        public string Classify(double score)
        {
            if (score > 90) return "Auto Accept";
            if (score >= 70) return "Manual Review";
            return "Regret";
        }


    }
}
