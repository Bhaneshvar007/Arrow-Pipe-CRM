namespace SalesCrm.Models
{
    public class ClientPOCDataModel
    {
        //public int ClientPOCID { get; set; }
        //public string ClientPOCName { get; set; }
        //public string ClientPOCEmail { get; set; }
        //public string ClientPOCContactNumber { get; set; }
        //public string ClientPOCData { get; set; }
        //public int ClientID { get; set; }
        //public string? ClientName { get; set; }
        //public string? ClientEmail { get; set; }
        //public int CompanyID { get; set; }
        //public string? CreatedBy { get; set; }
        //public int CompanyUserID { get; set; }
        //public DateTime ClientBirthday { get; set; }
        //public DateTime AddedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }
        //public int LastUpdatedBy { get; set; }



    }

    public class CustomerPOCDataModel
    {
        public int CustomerPOCID { get; set; }
        public string CustomerPOCName { get; set; }
        public string CustomerPOCEmail { get; set; }
        public string CustomerPOCContactNumber { get; set; }
        public string CustomerPOCData { get; set; }
        public int CustomerID { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }

        public int CompanyID { get; set; }
        public int CompanyUserID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ClientBirthday { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public int TotalCount { get; set; }
    }

}