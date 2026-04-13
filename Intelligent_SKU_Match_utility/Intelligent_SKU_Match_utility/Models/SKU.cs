using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Models
{
    public class SKU
    {
        
            public int SKU_Id { get; set; }
        public string Description { get; set; }
        public string Dimension { get; set; }
        public int FullTextRank { get; set; }
        public int RankPercent { get; set; }
    

    }
}
