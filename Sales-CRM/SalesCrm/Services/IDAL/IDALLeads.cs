using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALLeads
    {
        //List<LeadsEnqModel> GetAllLeadEnquiry(RequestModel request);
        List<LeadsEnqModel> GetAllLeadEnquiry(int companyId, int userId, int pageNumber, int pageSize, string source = null, string enquiryType = null,
            string assignedTo = null, string status = null, DateTime? fromDate = null, DateTime? toDate = null);

        LeadsEnqModel GetLeadEnquirybyEnqId(string enqId);
        ResponseModel ConvertEnqToLeadbyEnqIds(ConvertEnquiryRequest request);
        ResponseModel DeleteEnqbyEnqIds(List<string> enquiryIds);
        ResponseModel UpdateEnquiryList(List<UpdateEnquiryItem> request);
        ResponseModel SaveNewEnquiry(LeadsEnqModel model);
        ResponseModel UpdateEnquiry(LeadsEnqModel model);
        //List<LeadModel> GetAllLeadList(RequestModel request);
        List<LeadModel> GetAllLeadList(int companyId, int userId, int pageNumber, int pageSize, string source = null, string leadType = null,
            string assignedTo = null, DateTime? fromDate = null, DateTime? toDate = null);
        LeadModel GetLeadbyLeadId(string leadId);
        ResponseModel DeleteLeadbyLeadIds(List<string> LeadIds);
        ResponseModel UpdateLeadList(List<UpdateLeadItem> request);


    }
}
