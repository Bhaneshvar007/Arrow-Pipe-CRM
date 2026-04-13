using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Models
{
    public class ClientDataModel
    {
        //public int ClientID { get; set; }
      
        //public string ClientData { get; set; }
    
        //public string ClientName { get; set; }
    
        //public string ClientEmail { get; set; }
        
        //public string ClientContactNumber { get; set; }
        
        //public int CompanyID { get; set; }
        //public int CompanyUserID { get; set; }
        //public DateTime AddedDate { get; set; }
        //public DateTime lastUpdatedDate { get; set; }
        //public int lastUpdateBy { get; set; }
   
    }

    public class CustomerDataModel
    {
        public int CustomerID { get; set; }

        public string CustomerData { get; set; }

        public string CustomerName { get; set; }
        public string CompanyName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerContactNumber { get; set; }

        public int CompanyID { get; set; }
        public string? CreatedBy { get; set; }

        public int CompanyUserID { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int LastUpdatedBy { get; set; }
        public int TotalCount { get; set; }
    }
}
