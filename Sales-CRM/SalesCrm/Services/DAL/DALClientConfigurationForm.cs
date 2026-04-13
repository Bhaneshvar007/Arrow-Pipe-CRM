using Azure;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALClientConfigurationForm : IDALClientConfigurationForm
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;


        public DALClientConfigurationForm(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }


        public ClientConfigurationModel GetClientForm(RequestModel objModel)
        {
            ClientConfigurationModel clientForm = new ClientConfigurationModel();

            try
            {
                using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString()))
                {
                    var data = con.Query<ClientConfigurationModel>(
                        "sp_Get_Client_Form",
                        new { CompanyID = objModel.CompanyID },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    if (data != null)
                    {
                        clientForm = data;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return clientForm;
        }


        public ResponseModel AddClientForm(ClientConfigurationModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString());
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                SqlCommand cmd = new SqlCommand("sp_Insert_Client_Form", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@form_name", objModel.FormName);
                cmd.Parameters.AddWithValue("@form_json", objModel.FormJson);
                cmd.Parameters.AddWithValue("@user_id", objModel.UserID);
                cmd.Parameters.AddWithValue("@company_id", objModel.CompanyID);
                cmd.Parameters.AddWithValue("@created_date", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@form_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();

                var FormID = Convert.ToInt32(cmd.Parameters["@form_id"].Value.ToString());

                Response.Status = true;
                Response.ID = FormID;
                Response.Message = CommonHelper.SaveMessage;

            }
            catch (Exception ex)
            {
                Response.Status = false;
                Response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }


        public ResponseModel UpdateClientForm(ClientConfigurationModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString());

            try
            {
                SqlCommand cmd = new SqlCommand("sp_Update_Client_Form", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@form_id", objModel.FormID);
                cmd.Parameters.AddWithValue("@form_name", objModel.FormName);
                cmd.Parameters.AddWithValue("@form_json", objModel.FormJson);
                cmd.Parameters.AddWithValue("@company_id", objModel.CompanyID);
                cmd.Parameters.AddWithValue("@user_id", objModel.UserID);
                cmd.Parameters.AddWithValue("@updated_date", CommonHelper.GetDate);

                con.Open();
                cmd.ExecuteNonQuery();

                Response.Status = true;
                Response.ID = objModel.FormID;
                Response.Message = CommonHelper.UpdateMessage;
            }
            catch (Exception ex)
            {
                Response.Status = false;
                Response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return Response;
        }

    }
}
