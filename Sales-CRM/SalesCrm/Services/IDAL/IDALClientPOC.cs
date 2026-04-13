using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALClientPOC
    {
        CustomerPOCDataModel GetClientPOCById(int client_poc_id);

        List<CustomerPOCDataModel> GetClientPOC(RequestModel objModel);

        ResponseModel AddClientPOC(CustomerPOCDataModel objModel);

        ResponseModel UpdateClientPOC(CustomerPOCDataModel objModel);

        ResponseModel DeleteClientPOC(int client_poc_id,int user_id);
    }
}
