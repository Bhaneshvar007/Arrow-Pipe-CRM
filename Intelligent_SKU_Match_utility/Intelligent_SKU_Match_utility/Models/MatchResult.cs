using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Models
{
   public class MatchResult
    {
        public int InquiryId { get; set; }
        public int SKU_Id { get; set; }
        public double Score { get; set; }
        public string Classification { get; set; }
        public int Item { get; set; }
    }


     
}
