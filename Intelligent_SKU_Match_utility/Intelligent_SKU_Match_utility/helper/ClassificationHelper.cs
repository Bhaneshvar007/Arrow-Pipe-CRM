using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.helper
{
    public static class ClassificationHelper
    {
        public static string Classify(double score)
        {
            if (score > 90)
                return "Auto Accept";

            if (score >= 70)
                return "Manual Review";

            return "Regret";
        }
    }
}
