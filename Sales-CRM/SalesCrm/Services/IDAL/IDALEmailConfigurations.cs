using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALEmailConfigurations
    {
        List<EmailConfigurationsModel> GetEmailConfigurations(RequestModel model);
    }
}
