using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALLeadConfiguration : IDALLeadConfiguration
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALLeadConfiguration(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<LeadConfigurationModel> GetLeadForm(RequestModel requestModel)
        {
            List<LeadConfigurationModel> data = new List<LeadConfigurationModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<LeadConfigurationModel>(
                        "sp_get_lead_form",
                        new { company_id = requestModel.CompanyID, company_user_id = requestModel.CompanyUserID },
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

        public ResponseModel AddLeadForm(LeadConfigurationModel objModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@form_name", objModel.FormName);
                    param.Add("@form_json", objModel.FormJson);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@added_by", objModel.UserID);
                    param.Add("@form_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_lead_form", param, commandType: CommandType.StoredProcedure);

                    response.Status = true;
                    response.ID = param.Get<int>("@form_id");
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

        public ResponseModel UpdateLeadForm(LeadConfigurationModel objModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_lead_form",
                        new
                        {
                            form_id = objModel.FormID,
                            form_name = objModel.FormName,
                            form_json = objModel.FormJson,
                            company_id = objModel.CompanyID,
                            last_updated_by = objModel.UserID
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = objModel.FormID;
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
    }
}