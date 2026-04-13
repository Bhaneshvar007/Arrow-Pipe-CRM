namespace SalesCrm.Models
{

    public class DropDownCategoryModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int CompanyID { get; set; }

        public int CompanyUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int LastUpdatedBy { get; set; }
        public int TotalCount { get; set; }

    }
}
