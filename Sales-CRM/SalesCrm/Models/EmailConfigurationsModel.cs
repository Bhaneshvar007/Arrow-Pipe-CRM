namespace SalesCrm.Models
{
    public class EmailConfigurationsModel
    {

        public int EmailConfId { get; set; }

        public string Name { get; set; }

        public int AccountTypeId { get; set; }      // 🔥 ID
        public string AccountTypeName { get; set; } // 🔥 Display

        public string Email { get; set; }
        public string EmailPassword { get; set; }

        public string Server { get; set; }
        public int Port { get; set; }

        public int CompanyID { get; set; }
        public int CompanyUserID { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
}
