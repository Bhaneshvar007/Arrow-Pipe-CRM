using SalesCrm.Models;

namespace SalesCrm.CommonHelpers
{
    public class DalServiceHelper
    {

        public static CustomerDataModel CustomerinformationModelBinding(LeadsEnqModel model)
        {
            CustomerDataModel customerDataModel = new CustomerDataModel();

            customerDataModel.CustomerName = model.CustomerName;
            customerDataModel.CustomerEmail = model.CustomerEmail;
            customerDataModel.CustomerContactNumber = model.CustomerContact;
            customerDataModel.CompanyName = model.CompanyName;
            customerDataModel.CustomerData = model.CustomerInfoData;
            customerDataModel.CompanyUserID = model.CompanyUserID;
            customerDataModel.CompanyID = model.CompanyID;
            customerDataModel.CustomerID = model.CustomerId;

            return customerDataModel;
        }



        public static List<AttachmentDetailsModel> AttachmentModelBinding(LeadsEnqModel model)
        {
            return model.DocumentList?.Select(doc => new AttachmentDetailsModel
            {
                AttachmentName = doc.AttachmentName,
                AttachmentPath = doc.AttachmentPath,
                AttachmentType = doc.AttachmentType,
                CreatedDate = doc.AttachmentReceivedOn ?? DateTime.Now,
                DocumentID = 0,
                CompanyID = model.CompanyID,
                PageID = doc.PageId,
                CompanyUserID = model.CompanyUserID

            }).ToList();
        }

    }
}
