using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALClient : IDALClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<CustomerDataModel> GetClient(RequestModel objModel)
        {
            List<CustomerDataModel> customers = new List<CustomerDataModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<CustomerDataModel>(
                        "sp_get_client_data",
                         new
                         {
                             company_id = objModel.CompanyID,
                             user_id = objModel.CompanyUserID,
                             page_number = objModel.PageNumber,
                             page_size = objModel.PageSize
                         },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        customers = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return customers;
        }

        public CustomerDataModel GetClientById(int client_id)
        {
            CustomerDataModel customer = new CustomerDataModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<CustomerDataModel>(
                        "sp_get_client_data_byid",
                        new { client_id = client_id },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    if (data != null)
                        customer = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return customer;
        }
        public ResponseModel AddClient(CustomerDataModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (objModel.CustomerEmail.IsNullOrEmpty() || objModel.CustomerName.IsNullOrEmpty())
            {
                response.Status = false;
                response.Message = "Customer Name and Email could not be null !!";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidName(objModel.CustomerName))
            {
                response.Status = false;
                response.Message = "CustomerName should contain only alphabets";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidEmail(objModel.CustomerEmail))
            {
                response.Status = false;
                response.Message = "Invalid customer Email format";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidPhone(objModel.CustomerContactNumber))
            {
                response.Status = false;
                response.Message = "Invalid contact number format";
                response.Type = "warning";
                return response;

            }




            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();


                    var existingId = con.QueryFirstOrDefault<int?>(
                                "SELECT TOP 1 client_id FROM tbl_Client_Data WHERE LOWER(client_email) = LOWER(@Email) AND company_id = @CompanyId",
                                new { Email = objModel.CustomerEmail, CompanyId = objModel.CompanyID });

                    if (existingId != null)
                    {
                        response.Status = true;
                        response.ID = existingId.Value;
                        response.Message = "Customer Email already exists";
                        response.Type = "warning";
                        return response;
                    }



                    param.Add("@client_data", objModel.CustomerData);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@company_user_id", objModel.CompanyUserID);
                    param.Add("@client_name", objModel.CustomerName);
                    param.Add("@company_name", objModel.CompanyName);
                    param.Add("@client_email", objModel.CustomerEmail);
                    param.Add("@client_contact_number", objModel.CustomerContactNumber);
                    param.Add("@created_date", CommonHelper.GetDate);
                    param.Add("@client_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_client_data", param, commandType: CommandType.StoredProcedure);

                    response.Status = true;
                    response.ID = param.Get<int>("@client_id");
                    response.Message = CommonHelper.SaveMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Type = "error";
                CommonHelper.WriteLog(ex.Message);
                throw;
            }

            return response;
        }

        public ResponseModel UpdateClient(CustomerDataModel objModel)
        {
            ResponseModel response = new ResponseModel();


            if (objModel.CustomerEmail.IsNullOrEmpty() || objModel.CustomerName.IsNullOrEmpty())
            {
                response.Status = false;
                response.Message = "Customer Name and Email could not be null !!";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidName(objModel.CustomerName))
            {
                response.Status = false;
                response.Message = "CustomerName should contain only alphabets";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidEmail(objModel.CustomerEmail))
            {
                response.Status = false;
                response.Message = "Invalid customer Email format";
                response.Type = "warning";
                return response;
            }

            if (!CustomeValidatore.IsValidPhone(objModel.CustomerContactNumber))
            {
                response.Status = false;
                response.Message = "Invalid contact number format";
                response.Type = "warning";
                return response;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_client_data",
                        new
                        {
                            client_id = objModel.CustomerID,
                            client_data = objModel.CustomerData,
                            company_id = objModel.CompanyID,
                            client_name = objModel.CustomerName,
                            company_name = objModel.CompanyName,
                            client_email = objModel.CustomerEmail,
                            client_contact_number = objModel.CustomerContactNumber,
                            last_updated_by = objModel.LastUpdatedBy,
                            last_updated_date = CommonHelper.GetDate
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = objModel.CustomerID;
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

        public ResponseModel DeleteClient(int client_id, int user_id)
        {
            ResponseModel response = new ResponseModel();

            if (client_id <= 0)
            {
                response.Status = false;
                response.Message = "Client Id Could't be null or 0!";
                response.Type = "warning";
                return response;

            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_delete_client_data",
                        new { client_id = client_id, user_id = user_id },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = client_id;
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


        public List<CustomerDataModel> GetClientSuggestion(int companyId, string cust_name)
        {
            List<CustomerDataModel> customers = new List<CustomerDataModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<CustomerDataModel>(
                        "sp_get_client_data_by_suggestion",
                         new
                         {
                             company_id = companyId,
                             name = cust_name
                         },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        customers = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return customers;
        }


    }
}