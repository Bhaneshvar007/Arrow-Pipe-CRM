namespace SalesCrm.Models
{
    public class DropDownMasterModel
    {
        public int Id { get; set; }

        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }

        public string Name { get; set; }

        public int CompanyID { get; set; }

        public int AddedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }

    public class DropDownFilterModel
    {
        public int CompanyUserID { get; set; }
        public int CompanyID { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Category { get; set; }

    }

}
