using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.NLP
{
    public class DimensionExtractor
    {
        public static string Extract(string text)
        {
            var match = Regex.Match(text, @"\d+(\.\d+)?\s?(mm|cm|inch|in)");

            if (match.Success)
                return match.Value;

            return "";
        }


        public static string ExtractDimension(string text)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                text,
                @"\b\d+(\.\d+)?\s?(mm|cm|inch|in)\b",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (match.Success)
                return match.Value.ToLower();

            return "";
        }

        public static bool IsDimensionMatch(string inquiry, string skuDesc)
        {
            string inquiryDim = ExtractDimension(inquiry);
            string skuDim = ExtractDimension(skuDesc);

            if (string.IsNullOrEmpty(inquiryDim) ||
                string.IsNullOrEmpty(skuDim))
                return false;

            return inquiryDim == skuDim;
        }
    }
}

