using System.Text.Json.Serialization;

namespace SalesCrm.Models
{
    public class AIGeneratedSuitModel
    {
        public int Id { get; set; }
        public string EnquiryID { get; set; }
        public string TopMatchesJson { get; set; }


        [JsonIgnore]
        public string AttachmentsJson { get; set; }   
        public List<AttachmentModel> Attachments { get; set; }  
    }

    public class AttachmentModel
    {
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentType { get; set; }
    }

    public class AIGeneratedSuitRequest
    {
        public string EnqId { get; set; }
        public int PageId { get; set; }
        public string PageRef { get; set; }
    }
}
