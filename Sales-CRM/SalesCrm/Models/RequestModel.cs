using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Models
{
    public class RequestModel
    {
        public int CompanyUserID { get; set; }
        public int CompanyID { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

}
