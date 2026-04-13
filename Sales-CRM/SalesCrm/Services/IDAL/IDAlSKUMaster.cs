using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDAlSKUMaster
    {
        List<SKUMasterModel> GetSkuMaster(SkuFilterModel skuFilterModel);
        List<SKUMasterModel> GetSkuSuggestion(SkuSuggestionFilterModel req);
        List<SKUDropDownModel> GetSkuDropDownMaster(string type);
    }
}
