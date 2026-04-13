namespace SalesCrm.Models
{
    public class ClientConfigurationModel
    {
        public int FormID { get; set; }
        public string FormName { get; set; }
        public string FormJson { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
         
    }
}
