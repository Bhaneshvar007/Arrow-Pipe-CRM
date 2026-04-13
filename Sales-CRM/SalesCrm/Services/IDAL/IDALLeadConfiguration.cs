using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALLeadConfiguration
    {
        List<LeadConfigurationModel> GetLeadForm(RequestModel requestModel);

        ResponseModel AddLeadForm(LeadConfigurationModel objModel);

        ResponseModel UpdateLeadForm(LeadConfigurationModel objModel);
    }
}
