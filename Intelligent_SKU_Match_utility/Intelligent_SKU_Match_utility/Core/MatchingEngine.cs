using FuzzySharp;
using Intelligent_SKU_Match_utility.Cache;
using Intelligent_SKU_Match_utility.Data;
using Intelligent_SKU_Match_utility.helper;
using Intelligent_SKU_Match_utility.Models;
using Intelligent_SKU_Match_utility.NLP;
using Intelligent_SKU_Match_utility.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Intelligent_SKU_Match_utility.Core
{



    public class MatchingEngine
    {
        DatabaseHelper db = new DatabaseHelper();
        SKURepository repo = new SKURepository();
        public MatchResult FindBestMatch(int inquiryId, string input)
        {

            var top10 = repo.GetTop10SKU(input);

            double bestScore = 0;
            SKU bestSku = null;

            foreach (var sku in top10)
            {
                int fuzzyScore = Fuzz.TokenSortRatio(input, sku.Description);

                bool dimensionMatch = false;

                dimensionMatch =
                       DimensionExtractor.IsDimensionMatch(input, sku.Description);



                double finalScore =
                    ScoreEngine.Calculate(
                        sku.RankPercent,
                        fuzzyScore,
                        dimensionMatch);

                if (finalScore > bestScore)
                {
                    bestScore = finalScore;
                    bestSku = sku;
                }
            }

            string classification = ClassificationHelper.Classify(bestScore);

            repo.InsertMatchResult(inquiryId, bestSku?.SKU_Id ?? 0, bestScore, classification);

            return new MatchResult
            {
                InquiryId = inquiryId,
                SKU_Id = bestSku?.SKU_Id ?? 0,
                Score = bestScore,
                Classification = classification
            };
        }

 
    }
}
