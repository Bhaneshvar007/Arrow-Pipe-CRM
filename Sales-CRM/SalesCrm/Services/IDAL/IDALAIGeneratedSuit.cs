using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALAIGeneratedSuit
    {
        List<AIGeneratedSuitModel> GetAIGeneratedSuitEnquirys(AIGeneratedSuitRequest req);
    }
}
