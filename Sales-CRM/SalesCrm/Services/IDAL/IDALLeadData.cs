using SalesCrm.Models;

public interface IDALLeadData
{
    List<LeadDataModel> GetLeadData(RequestModel request);

    LeadDataModel GetLeadDataById(int leadId);

    ResponseModel AddLeadData(LeadDataModel objModel);

    ResponseModel UpdateLeadData(LeadDataModel objModel);

    ResponseModel DeleteLeadData(int leadId, int userId);
}