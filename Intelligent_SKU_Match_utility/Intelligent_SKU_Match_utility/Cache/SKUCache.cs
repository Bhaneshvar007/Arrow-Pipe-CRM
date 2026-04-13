using Intelligent_SKU_Match_utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Cache
{
    public static class SKUCache
    {
        public static List<SKU> CachedSKUs = new List<SKU>();

        public static void Load(List<SKU> skus)
        {
            CachedSKUs = skus;
        }
    }
}
