using Intelligent_SKU_Match_utility.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Intelligent_SKU_Match_utility.Utils
{
    public static class JsonReader
    {
        public static List<Inquiry> Load(string path)
        {
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<Inquiry>>(json);
        }
    }
}