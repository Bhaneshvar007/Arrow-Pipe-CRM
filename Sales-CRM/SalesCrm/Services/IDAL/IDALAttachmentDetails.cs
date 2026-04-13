using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALAttachmentDetails
    {
        List<AttachmentDetailsModel> GetAttachmentDetails(int PageID, int RelevantID, string PageRef, int CompanyID);

        AttachmentDetailsModel GetAttachmentDetailsById(int attachement_id);
        List<AttachmentDetailsModel> GetAttachmentList(RequestModel requestModel);

        ResponseModel AddAttachmentDetails(AttachmentDetailsModel objModel);

        ResponseModel UpdateAttachmentDetails(AttachmentDetailsModel objModel);

        ResponseModel DeleteAttachmentDetails(int attachement_id);
        ResponseModel DeleteAttachments(int pageId, string pageRef, int relevantId, int companyId);
    }
}
