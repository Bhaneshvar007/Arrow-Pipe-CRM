namespace SalesCrm.Models
{
    public class AttachmentDetailsModel
    {
        public int AttachementID { get; set; }
        public int DocumentID { get; set; }
        public string? DocumnentType { get; set; }
        public string AttachmentType { get; set; }
        public int PageID { get; set; }
        public string? PageRef { get; set; }
        public int GoogleID { get; set; } = 0;
        public int RelevantID { get; set; }
        public int CompanyUserID { get; set; } = 0;
        public int CompanyID { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
    }
}
