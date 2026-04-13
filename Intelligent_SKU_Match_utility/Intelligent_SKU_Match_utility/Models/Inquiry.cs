using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Models
{
    public class Inquiry
    {
        public int InquiryId { get; set; }
        public string RawText { get; set; }
       // public string ProcessedText { get; set; }
       public List<ProcessedItem> ProcessedText { get; set; }
       public string description { get; set; }
        public bool Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public class ProcessedItem
    {
        public string size { get; set; }
        public string lot { get; set; }
        public string description { get; set; }
    }




}
