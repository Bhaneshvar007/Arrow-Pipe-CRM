using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Intelligent_SKU_Match_utility.NLP
{
   
    public static class TextCleaner
    {
        public static string Clean(string text)
        {
            text = text.ToLower();

            text = Regex.Replace(text, "[^a-z0-9 ]", "");

            text = Regex.Replace(text, @"\s+", " ");

            return text.Trim();
        }
    }
}
