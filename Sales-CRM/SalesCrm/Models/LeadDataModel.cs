namespace SalesCrm.Models
{
    public class LeadDataModel
    {
        public int LeadID { get; set; }

        public string LeadName { get; set; }

        public string LeadMobile { get; set; }

        public string LeadEmail { get; set; }

        public string LeadData { get; set; }

        public int CompanyID { get; set; }

        public int CompanyUserID { get; set; }

        public int LeadActionID { get; set; }

        public int LeadMailActionID { get; set; }

        public int LeadStageID { get; set; }

        public int LeadSource { get; set; }

        public int LeadVerticle { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
