using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALClientPOCConfigurationForm : IDALClientPOCConfigurationForm
    {
        private string _connectionString;

        public DALClientPOCConfigurationForm()
        {
            _connectionString = CommonHelper.GetConnectionString();
        }

        public ClientPOCConfigurationModel GetClientPOCForm(RequestModel objModel)
        {
            ClientPOCConfigurationModel clientPOCForm = new ClientPOCConfigurationModel();

            try
            {
                using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString()))
                {
                    var data = con.Query<ClientPOCConfigurationModel>(
                        "sp_Get_Client_POC_Form",
                        new { CompanyID = objModel.CompanyID },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    if (data != null)
                    {
                        clientPOCForm = data;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return clientPOCForm;
        }
        public ResponseModel AddClientPOCForm(ClientPOCConfigurationModel objModel)
        {
            ResponseModel response = new ResponseModel();
            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_insert_client_poc_form", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@form_name", objModel.FormName);
                cmd.Parameters.AddWithValue("@form_json", objModel.FormJson);
                cmd.Parameters.AddWithValue("@company_id", objModel.CompanyID);
                cmd.Parameters.AddWithValue("@added_by", objModel.UserID);
                cmd.Parameters.AddWithValue("@last_updated_date", CommonHelper.GetDate);
                cmd.Parameters.AddWithValue("@form_id", SqlDbType.Int).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                var formID = Convert.ToInt32(cmd.Parameters["@form_id"].Value);

                response.Status = true;
                response.ID = formID;
                response.Message = CommonHelper.SaveMessage;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public ResponseModel UpdateClientPOCForm(ClientPOCConfigurationModel objModel)
        {
            ResponseModel response = new ResponseModel();
            SqlConnection con = new SqlConnection(_connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_update_client_poc_form", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@form_id", objModel.FormID);
                cmd.Parameters.AddWithValue("@form_name", objModel.FormName);
                cmd.Parameters.AddWithValue("@form_json", objModel.FormJson);
                cmd.Parameters.AddWithValue("@company_id", objModel.CompanyID);
                cmd.Parameters.AddWithValue("@last_updated_by", objModel.UserID);
                cmd.Parameters.AddWithValue("@last_updated_date", CommonHelper.GetDate);

                con.Open();
                cmd.ExecuteNonQuery();

                response.Status = true;
                response.ID = objModel.FormID;
                response.Message = CommonHelper.UpdateMessage;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                CommonHelper.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return response;
        }
    }
}