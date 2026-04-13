using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALAttachmentDetails : IDALAttachmentDetails
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALAttachmentDetails(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }


        public List<AttachmentDetailsModel> GetAttachmentDetails(int PageID, int RelevantID, string PageRef, int CompanyID)
        {
            List<AttachmentDetailsModel> attachments = new List<AttachmentDetailsModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<AttachmentDetailsModel>(
                        "sp_get_attachment_details",
                        new
                        {
                            page_id = PageID,
                            page_ref = PageRef,
                            relevant_id = RelevantID,
                            company_id = CompanyID
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        attachments = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return attachments;
        }

        public AttachmentDetailsModel GetAttachmentDetailsById(int attachement_id)
        {
            AttachmentDetailsModel attachment = new AttachmentDetailsModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<AttachmentDetailsModel>(
                        "sp_get_attachment_details_byid",
                        new { attachement_id },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    if (data != null)
                        attachment = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return attachment;
        }


        public List<AttachmentDetailsModel> GetAttachmentList(RequestModel requestModel)
        {
            List<AttachmentDetailsModel> attachments = new List<AttachmentDetailsModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<AttachmentDetailsModel>(
                        "sp_get_attachment_list",
                        new
                        {
                            company_id = requestModel.CompanyID,
                            company_user_id = requestModel.CompanyUserID,
                            page_number = requestModel.PageNumber,
                            page_size = requestModel.PageSize
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        attachments = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return attachments;
        }

        public ResponseModel AddAttachmentDetails(AttachmentDetailsModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (objModel.AttachmentName.IsNullOrEmpty() || objModel.AttachmentPath.IsNullOrEmpty())
            {
                response.Status = false;
                response.Message = "AttachmentName and AttachmentPath can't not be null";
                response.Type = "warning";
                return response;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@google_id", objModel.GoogleID);
                    param.Add("@attachment_name", objModel.AttachmentName);
                    param.Add("@attachment_type", objModel.AttachmentType);
                    param.Add("@attachment_path", objModel.AttachmentPath);
                    param.Add("@page_id", objModel.PageID);
                    param.Add("@page_ref", objModel.PageRef);
                    param.Add("@relevant_id", objModel.RelevantID);
                    param.Add("@document_id", objModel.DocumentID);
                    param.Add("@company_user_id", objModel.CompanyUserID);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@created_date", CommonHelper.GetDate);
                    param.Add("@attachement_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_attachment_details", param, commandType: CommandType.StoredProcedure);

                    response.Status = true;
                    response.ID = param.Get<int>("@attachement_id");
                    response.Message = CommonHelper.SaveMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }


        public ResponseModel UpdateAttachmentDetails(AttachmentDetailsModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (objModel.AttachmentName.IsNullOrEmpty() || objModel.AttachmentPath.IsNullOrEmpty())
            {
                response.Status = false;
                response.Message = "AttachmentName and AttachmentPath can't not be null";
                response.Type = "warning";
                return response;

            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_attachment_details",
                        new
                        {
                            attachement_id = objModel.AttachementID,
                            google_id = objModel.GoogleID,
                            attachment_name = objModel.AttachmentName,
                            attachment_type = objModel.AttachmentType,
                            attachment_path = objModel.AttachmentPath,
                            page_id = objModel.PageID,
                            relevant_id = objModel.RelevantID,
                            document_id = objModel.DocumentID,
                            company_user_id = objModel.CompanyUserID,
                            company_id = objModel.CompanyID
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = objModel.AttachementID;
                    response.Message = CommonHelper.UpdateMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }


        public ResponseModel DeleteAttachmentDetails(int attachement_id)
        {
            ResponseModel response = new ResponseModel();

            if (attachement_id <= 0)
            {

                response.Status = false;
                response.Message = "AttachmentId must be greater than 0.";
                response.Type = "warning";
                return response;

            }


            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_delete_attachment_details",
                        new { attachement_id = attachement_id },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = attachement_id;
                    response.Message = CommonHelper.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }


        public ResponseModel DeleteAttachments(int pageId, string pageRef, int relevantId, int companyId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    con.Execute(
                        @"DELETE FROM tbl_attachment_details 
            WHERE page_id = @page_id 
            AND PageRef = @page_ref 
            AND relevant_id = @relevant_id 
            AND company_id = @company_id",
                        new
                        {
                            page_id = pageId,
                            page_ref = pageRef,
                            relevant_id = relevantId,
                            company_id = companyId
                        });

                    response.Status = true;
                    response.Message = "Attachments deleted successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";

                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }



    }
}