using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALClientPOCConfigurationForm
    {
        ClientPOCConfigurationModel GetClientPOCForm(RequestModel objModel);

        ResponseModel AddClientPOCForm(ClientPOCConfigurationModel objModel);

        ResponseModel UpdateClientPOCForm(ClientPOCConfigurationModel objModel);
    }
}
