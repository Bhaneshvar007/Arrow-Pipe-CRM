using Intelligent_SKU_Match_utility.Core;
using Intelligent_SKU_Match_utility.NLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Services
{
    
        public class InquiryService
        {
            MatchingEngine engine = new MatchingEngine();

            public void ProcessInquiry(string rawText)
            {
                string cleanText = TextCleaner.Clean(rawText);

                var result = engine.FindBestMatch(1, cleanText);

                Console.WriteLine("Best SKU : " + result.SKU_Id);
                Console.WriteLine("Score : " + result.Score);
                Console.WriteLine("Classification : " + result.Classification);
            }
        }
    }

