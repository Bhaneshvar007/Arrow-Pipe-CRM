namespace SalesCrm.Models
{
    public class LeadModel
    {
        public int TotalRecords { get; set; }
        public int LeadRefId { get; set; }
        public int EnqRefId { get; set; }
        public string? LeadId { get; set; }
        public bool IsRepeatCustomer { get; set; }
        public string? LeadTag { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string CustomerInfoData { get; set; }
        // public int IndustryType { get; set; }
        public string? IndustryType { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContact { get; set; }

        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string? ProjectName { get; set; }
        public string? DeliveryLocation { get; set; }
        public string? Remark { get; set; }
        public string? LeadSource { get; set; }
        public string? LeadSourceDetail { get; set; }
        public string ReceivedOn { get; set; }
        public string LeadStatus { get; set; }
        public string? LeadType { get; set; }
        public string? LeadDescription { get; set; }

        public int AssignTo { get; set; }
        public int CompanyID { get; set; }
        public int CompanyUserID { get; set; }
        public DateTime? LeadOn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool is_active { get; set; }

        //[JsonIgnore]
        public string? Attachments { get; set; }
        //  [JsonIgnore] 
        public string? SKUItems { get; set; }


        public List<LeadDocumentsModel> DocumentList { get; set; }
        public List<LeadSkuModel>? SkuList { get; set; }
        public LeadAssignmentModel? Assignment { get; set; }

    }

    public class LeadDocumentsModel
    {
        public int AttachmentId { get; set; }
        public int EnquiryId { get; set; }
        public int PageId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime? AttachmentReceivedOn { get; set; }
        public bool IsActive { get; set; }

    }


    public class LeadSkuModel
    {
        public int EnqSkuId { get; set; }
        public int LeadID { get; set; }
        public string Description { get; set; }
        public string Material { get; set; } //not present
        public string SkuType { get; set; }
        public string Size { get; set; }
        public string Schedule { get; set; }
        //public string GradeSpec { get; set; }
        public string Grade { get; set; }
        public string Brand { get; set; }  //not present
        public string CountryOfOrigin { get; set; }  //not present
        public string Inventory { get; set; }   //not present
        public string Unit { get; set; }

        public string Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public int TotalCount { get; set; }
    }


    public class LeadAssignmentModel
    {
        public int AssId { get; set; }
        public int LeadId { get; set; }
        public int AssignTo { get; set; }
        public string Status { get; set; }
        public string InternalNotes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }

    }

    public class UpdateLeadItem
    {
        public string LeadId { get; set; }
        public int? AssignedTo { get; set; }
        public string LeadStatus { get; set; }
        public int UpdatedBy { get; set; }
        public int CompanyId { get; set; }
    }


}
