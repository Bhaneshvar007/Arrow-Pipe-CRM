using SalesCrm.Models;

namespace SalesCrm.Services.IDAL
{
    public interface IDALMailWorkflow
    {
        List<MailWorkflowModel> GetWorkflow(RequestModel req);
        MailWorkflowModel GetById(int id);
        ResponseModel Add(MailWorkflowModel model);
        ResponseModel Update(MailWorkflowModel model);
        ResponseModel Delete(int id, int userId);
    }
}
