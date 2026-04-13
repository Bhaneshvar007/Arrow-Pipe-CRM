using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALClientConfigurationForm
    {
        ClientConfigurationModel GetClientForm(RequestModel objModel);

        ResponseModel AddClientForm(ClientConfigurationModel objModel);

        ResponseModel UpdateClientForm(ClientConfigurationModel objModel);
    }
}
