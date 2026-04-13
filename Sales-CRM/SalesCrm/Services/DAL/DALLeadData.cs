using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALLeadData : IDALLeadData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALLeadData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<LeadDataModel> GetLeadData(RequestModel request)
        {
            List<LeadDataModel> data = new List<LeadDataModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadDataModel>(
                        "sp_get_lead_data",
                        new
                        {
                            company_id = request.CompanyID,
                            company_user_id = request.CompanyUserID,
                            page_number = request.PageNumber,
                            page_size = request.PageSize
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }

        public LeadDataModel GetLeadDataById(int leadId)
        {
            LeadDataModel data = new LeadDataModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadDataModel>(
                        "sp_get_lead_data_byid",
                        new { lead_id = leadId },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return data;
        }

        public ResponseModel AddLeadData(LeadDataModel objModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@lead_name", objModel.LeadName);
                    param.Add("@lead_mobile", objModel.LeadMobile);
                    param.Add("@lead_email", objModel.LeadEmail);
                    param.Add("@lead_data", objModel.LeadData);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@company_user_id", objModel.CompanyUserID);
                    param.Add("@lead_action_id", objModel.LeadActionID);
                    param.Add("@lead_mail_action_id", objModel.LeadMailActionID);
                    param.Add("@lead_stage_id", objModel.LeadStageID);
                    param.Add("@lead_source", objModel.LeadSource);
                    param.Add("@lead_verticle", objModel.LeadVerticle);
                    param.Add("@lead_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_lead_data", param, commandType: CommandType.StoredProcedure);

                    response.Status = true;
                    response.ID = param.Get<int>("@lead_id");
                    response.Message = CommonHelper.SaveMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }

        public ResponseModel UpdateLeadData(LeadDataModel objModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_lead_data",
                        new
                        {
                            lead_id = objModel.LeadID,
                            lead_name = objModel.LeadName,
                            lead_mobile = objModel.LeadMobile,
                            lead_email = objModel.LeadEmail,
                            lead_data = objModel.LeadData,
                            lead_action_id = objModel.LeadActionID,
                            lead_mail_action_id = objModel.LeadMailActionID,
                            lead_stage_id = objModel.LeadStageID,
                            lead_source = objModel.LeadSource,
                            lead_verticle = objModel.LeadVerticle,
                            last_updated_by = objModel.LastUpdatedBy
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = objModel.LeadID;
                    response.Message = CommonHelper.UpdateMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }

        public ResponseModel DeleteLeadData(int leadId, int userId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_delete_lead_data",
                        new
                        {
                            lead_id = leadId,
                            updated_by = userId
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = leadId;
                    response.Message = CommonHelper.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }

            return response;
        }
    }
}