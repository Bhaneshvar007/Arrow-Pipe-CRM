using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

public class DALClientPOC : IDALClientPOC
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _connectionString;

    public DALClientPOC(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _connectionString = CommonHelper.GetConnectionString();
    }

    public List<CustomerPOCDataModel> GetClientPOC(RequestModel objModel)
    {
        List<CustomerPOCDataModel> POCList = new List<CustomerPOCDataModel>();

        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var data = con.Query<CustomerPOCDataModel>(
                    "sp_get_client_poc_data",
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
                    POCList = data;
            }
        }
        catch (Exception ex)
        {
            CommonHelper.WriteLog(ex.ToString());
        }

        return POCList;
    }

    public CustomerPOCDataModel GetClientPOCById(int client_poc_id)
    {
        CustomerPOCDataModel POCList = new CustomerPOCDataModel();

        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var data = con.Query<CustomerPOCDataModel>(
                    "sp_get_client_poc_data_byid",
                    new { client_poc_id = client_poc_id },
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();

                if (data != null)
                    POCList = data;
            }
        }
        catch (Exception ex)
        {
            CommonHelper.WriteLog(ex.ToString());
        }

        return POCList;
    }

    public ResponseModel AddClientPOC(CustomerPOCDataModel objModel)
    {
        ResponseModel response = new ResponseModel();

        if (objModel.CustomerPOCName.IsNullOrEmpty() || objModel.CustomerPOCEmail.IsNullOrEmpty())
        {
            response.Status = false;
            response.Message = "Name and Email could not be NullOrEmpty !!";
            response.Type = "warning";
            return response;
        }


        if (!CustomeValidatore.IsValidName(objModel.CustomerPOCName))
        {
            response.Status = false;
            response.Message = "Customer POC Name should contain only alphabets";
            response.Type = "warning";
            return response;
        }

        if (!CustomeValidatore.IsValidEmail(objModel.CustomerPOCEmail))
        {
            response.Status = false;
            response.Message = "Invalid customer poc Email format";
            response.Type = "warning";
            return response;
        }

        if (!CustomeValidatore.IsValidPhone(objModel.CustomerPOCContactNumber))
        {
            response.Status = false;
            response.Message = "Invalid contact number";
            response.Type = "warning";
            return response;
        }




        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                DynamicParameters param = new DynamicParameters();

                param.Add("@client_poc_name", objModel.CustomerPOCName);
                param.Add("@client_poc_email", objModel.CustomerPOCEmail);
                param.Add("@client_poc_contact_number", objModel.CustomerPOCContactNumber);
                param.Add("@client_poc_data", objModel.CustomerPOCData);
                param.Add("@client_id", objModel.CustomerID);
                param.Add("@company_id", objModel.CompanyID);
                param.Add("@client_birthday", objModel.ClientBirthday);
                param.Add("@company_user_id", objModel.CompanyUserID);
                param.Add("@created_date", CommonHelper.GetDate);
                param.Add("@client_poc_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute("sp_insert_client_poc_data", param, commandType: CommandType.StoredProcedure);

                response.Status = true;
                response.ID = param.Get<int>("@client_poc_id");
                response.Message = CommonHelper.SaveMessage;
            }
        }
        catch (Exception ex)
        {
            CommonHelper.WriteLog(ex.ToString());
            response.Status = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public ResponseModel UpdateClientPOC(CustomerPOCDataModel objModel)
    {
        ResponseModel response = new ResponseModel();

        if (objModel.CustomerPOCEmail.IsNullOrEmpty() || objModel.CustomerPOCEmail.IsNullOrEmpty())
        {
            response.Status = false;
            response.Message = "Name and Email could not be NullOrEmpty !!";
            response.Type = "warning";
            return response;
        }


        if (!CustomeValidatore.IsValidName(objModel.CustomerPOCName))
        {
            response.Status = false;
            response.Message = "Customer POC Name should contain only alphabets";
            response.Type = "warning";
            return response;
        }

        if (!CustomeValidatore.IsValidEmail(objModel.CustomerPOCEmail))
        {
            response.Status = false;
            response.Message = "Invalid customer poc Email format";
            response.Type = "warning";
            return response;
        }

        if (!CustomeValidatore.IsValidPhone(objModel.CustomerPOCContactNumber))
        {
            response.Status = false;
            response.Message = "Invalid contact number";
            response.Type = "warning";
            return response;
        }

        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Execute("sp_update_client_poc_data",
                    new
                    {
                        client_poc_id = objModel.CustomerPOCID,
                        client_poc_data = objModel.CustomerPOCData,
                        company_id = objModel.CompanyID,
                        client_id = objModel.CustomerID,
                        client_poc_name = objModel.CustomerPOCName,
                        client_poc_email = objModel.CustomerPOCEmail,
                        client_birthday = objModel.ClientBirthday,
                        client_poc_contact_number = objModel.CustomerPOCContactNumber,
                        last_updated_by = objModel.LastUpdatedBy,
                        last_updated_date = CommonHelper.GetDate,
                    },
                    commandType: CommandType.StoredProcedure);

                response.Status = true;
                response.ID = objModel.CustomerPOCID;
                response.Message = CommonHelper.UpdateMessage;
            }
        }
        catch (Exception ex)
        {
            CommonHelper.WriteLog(ex.ToString());
            response.Status = false;
            response.Type = "error";
            response.Message = ex.Message;
        }

        return response;
    }

    public ResponseModel DeleteClientPOC(int client_poc_id, int user_id)
    {
        ResponseModel response = new ResponseModel();


        if (client_poc_id <= 0)
        {
            response.Status = false;
            response.Message = "Id Could't be null or 0!";
            response.Type = "warning";
            return response;

        }


        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Execute("sp_delete_client_poc_data",
                    new { client_poc_id = client_poc_id, user_id = user_id },
                    commandType: CommandType.StoredProcedure);

                response.Status = true;
                response.ID = client_poc_id;
                response.Message = CommonHelper.DeleteMessage;
            }
        }
        catch (Exception ex)
        {
            CommonHelper.WriteLog(ex.ToString());
            response.Status = false;
            response.Type = "error";
            response.Message = ex.Message;
        }

        return response;
    }
}