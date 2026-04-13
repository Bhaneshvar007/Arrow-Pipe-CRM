namespace SalesCrm.Models
{
    public class SKUMasterModel
    {

        public int Id { get; set; }
        public string Skuno { get; set; }

        public string Description { get; set; }
        public string Material { get; set; }
        public string Type { get; set; }

        public string Size { get; set; }
        public string Sch { get; set; }

        public string Wt { get; set; }
        public string WtIn { get; set; }

        public string Size2 { get; set; }
        public string Sch2 { get; set; }

        public string Wt2 { get; set; }
        public string Wt2In { get; set; }

        public string Grade { get; set; }
        public string Brand { get; set; }
        public string Coo { get; set; }
        public string Psl { get; set; }

        public string PressureRating { get; set; }
        public string BaseUnitOfMeasure { get; set; }

        public string Inventory { get; set; }
        public string InventoryFt { get; set; }
        public string PipesInventoryNos { get; set; }

        public string AvailableInventoryMeters { get; set; }
        public string AvailableInventoryFt { get; set; }
        public string AvailableInventoryNos { get; set; }

        public string UnitPriceAed { get; set; }
        public string CostUsdPerTon { get; set; }
        public string PriceUsdPerMeter { get; set; }
        public string PriceAedPerMeter { get; set; }
        public string PriceUsdPerFt { get; set; }
        public string PriceUsdPerTon { get; set; }

        public string QtyOnPurchaseOrder { get; set; }
        public string QuantitySold { get; set; }
        public string QuantityReserved { get; set; }
        public string QtyOnSalesOrder { get; set; }

        public string QuantityOnMr { get; set; }
        public string QuantityOnSalesQuote { get; set; }
        public string QuantityOnBlanketOrder { get; set; }
        public string QtyOnPurchaseOrderShipped { get; set; }

        public int TotalCount { get; set; }

    }
    public class SkuSuggestionFilterModel
    {
        public string? Description { get; set; }
        public string? Material { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
        public string? Sch { get; set; }
        public string? Grade { get; set; }
        public string? Brand { get; set; }
    }

    public class SkuFilterModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string material { get; set; }
        public string grade { get; set; }
        public string brand { get; set; }
        public int TotalCount { get; set; }
    }



    public class SKUDropDownModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

}
