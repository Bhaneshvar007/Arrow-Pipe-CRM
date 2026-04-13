using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALMailWorkflow : IDALMailWorkflow
    {
        private string _connectionString;

        public DALMailWorkflow()
        {
            _connectionString = CommonHelper.GetConnectionString();
        }
        public List<MailWorkflowModel> GetWorkflow(RequestModel req)
        {
            List<MailWorkflowModel> response = new List<MailWorkflowModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    response = con.Query<MailWorkflowModel>(
                        "sp_get_workflow",
                        new
                        {
                            company_id = req.CompanyID,
                            page_number = req.PageNumber,
                            page_size = req.PageSize
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }

        public MailWorkflowModel GetById(int id)
        {
            MailWorkflowModel response = new MailWorkflowModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    response = con.Query<MailWorkflowModel>(
                        "sp_get_workflow_byid",
                        new { workflow_id = id },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }

        public ResponseModel Add(MailWorkflowModel model)
        {
            ResponseModel res = new ResponseModel();

            if (string.IsNullOrWhiteSpace(model.WorkFlowName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "WorkFlowName is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(model.WorkFlowName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "WorkFlowName should contain only alphabets",
                    Type = "warning"
                };
            }


            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@company_id", model.CompanyId);
                    param.Add("@workflow_name", model.WorkFlowName);
                    param.Add("@lead_stage", model.LeadStage);
                    param.Add("@vertical", model.Vertical);
                    param.Add("@template", model.Template);
                    param.Add("@mail_action", model.MailAction);
                    param.Add("@customer_replied_mail", model.CustomerReplyedMail);
                    param.Add("@new_mail_status_replied", model.NewMailStatusReplyed);
                    param.Add("@customer_not_replied_mail", model.CustomerNotReplyedMail);
                    param.Add("@new_mail_status_not_replied", model.NewMailStatusNotReplyed);
                    param.Add("@mail_box", model.MailBox);
                    param.Add("@mail_box_cc", model.MailBoxCC);
                    param.Add("@mail_box_bcc", model.MailBoxBCC);
                    param.Add("@interval_days", model.IntervalDays);
                    param.Add("@mail_send_time", model.MailSendTime);
                    param.Add("@workflow_action", model.WorkFlowAction);
                    param.Add("@track_email", model.TrackEmail);
                    param.Add("@email_subject", model.EmailSubject);
                    param.Add("@created_by", model.CreatedBy);
                    param.Add("@workflow_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_workflow", param, commandType: CommandType.StoredProcedure);

                    res.ID = param.Get<int>("@workflow_id");
                    res.Status = true;
                    res.Message = CommonHelper.SaveMessage;
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return res;
        }

        public ResponseModel Update(MailWorkflowModel model)
        {
            ResponseModel res = new ResponseModel();

            if (string.IsNullOrWhiteSpace(model.WorkFlowName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "WorkFlowName is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(model.WorkFlowName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "WorkFlowName should contain only alphabets",
                    Type = "warning"
                };

            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute("sp_update_workflow", new
                    {
                        workflow_id = model.WorkFlowId,
                        workflow_name = model.WorkFlowName,
                        lead_stage = model.LeadStage,
                        vertical = model.Vertical,
                        template = model.Template,
                        mail_action = model.MailAction,
                        customer_replied_mail = model.CustomerReplyedMail,
                        new_mail_status_replied = model.NewMailStatusReplyed,
                        customer_not_replied_mail = model.CustomerNotReplyedMail,
                        new_mail_status_not_replied = model.NewMailStatusNotReplyed,
                        mail_box = model.MailBox,
                        mail_box_cc = model.MailBoxCC,
                        mail_box_bcc = model.MailBoxBCC,
                        interval_days = model.IntervalDays,
                        mail_send_time = model.MailSendTime,
                        workflow_action = model.WorkFlowAction,
                        track_email = model.TrackEmail,
                        email_subject = model.EmailSubject,
                        updated_by = model.UpdatedBy
                    }, commandType: CommandType.StoredProcedure);

                    res.ID = model.WorkFlowId;
                    res.Status = true;
                    res.Message = CommonHelper.UpdateMessage;
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.Type = "error";
                CommonHelper.WriteLog(ex.Message);
            }

            return res;
        }

        public ResponseModel Delete(int id, int userId)
        {
            ResponseModel res = new ResponseModel();

            if (id <= 0)
            {
                res.Status = false;
                res.Message = "Id must be greater than 0.";
                res.Type = "warning";
                return res;
            }   

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute("sp_delete_workflow",
                        new { workflow_id = id, user_id = userId },
                        commandType: CommandType.StoredProcedure);

                    res.ID = id;
                    res.Status = true;
                    res.Message = CommonHelper.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }

            return res;
        }

    }
}
