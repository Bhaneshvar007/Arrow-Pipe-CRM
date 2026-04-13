using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALClient
    {
        List<CustomerDataModel> GetClient(RequestModel requestModel);
        CustomerDataModel GetClientById(int customer_id);

        ResponseModel AddClient(CustomerDataModel objModel);

        ResponseModel UpdateClient(CustomerDataModel objModel);

        ResponseModel DeleteClient(int customer_id,int user_id);
        List<CustomerDataModel> GetClientSuggestion(int companyId, string cust_name);
    }
}
