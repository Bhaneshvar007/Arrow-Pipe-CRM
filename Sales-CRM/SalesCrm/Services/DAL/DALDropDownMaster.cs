using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALDropDownMaster : IDALDropDownMaster
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALDropDownMaster(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<DropDownMasterModel> GetDropDownValues(DropDownFilterModel requestModel)
        {
            List<DropDownMasterModel> data = new List<DropDownMasterModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<DropDownMasterModel>(
                        "sp_get_dropdown_values",
                        new
                        {
                            company_user_id = requestModel.CompanyUserID,
                            company_id = requestModel.CompanyID,
                            page_number = requestModel.PageNumber,
                            category = requestModel.Category,
                            page_size = requestModel.PageSize
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


        public List<DropDownMasterModel> GetDropDownValuesByCetagory(int categoryId, int companyId)
        {
            List<DropDownMasterModel> data = new List<DropDownMasterModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<DropDownMasterModel>(
                        "sp_get_dropdown_values_by_Cetagory",
                        new
                        {
                            category_id = categoryId,
                            company_id = companyId
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



        public DropDownMasterModel GetDropDownValueById(int id)
        {
            DropDownMasterModel data = new DropDownMasterModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<DropDownMasterModel>(
                        "sp_get_dropdown_value_byid",
                        new { id },
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


        public ResponseModel AddDropDownValue(DropDownMasterModel objModel)
        {
            ResponseModel response = new ResponseModel();
            if (string.IsNullOrWhiteSpace(objModel.Name))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Name is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(objModel.Name))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Name should contain only alphabets",
                    Type = "warning"
                };
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@category_id", objModel.CategoryID);
                    param.Add("@name", objModel.Name);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@added_by", objModel.AddedBy);
                    param.Add("@is_default", objModel.IsDefault);
                    param.Add("@created_date", CommonHelper.GetDate);
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_dropdown_value", param, commandType: CommandType.StoredProcedure);

                    int resultId = param.Get<int>("@id");

                    if (resultId == -1)
                    {
                        return new ResponseModel
                        {
                            Status = false,
                            Message = "This name already exists in the selected category",
                            Type = "warning"
                        };

                    }

                    response.Status = true;
                    response.ID = resultId;
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



        //public ResponseModel AddDropDownValue(DropDownMasterModel objModel)
        //{
        //    ResponseModel response = new ResponseModel();

        //    if (string.IsNullOrWhiteSpace(objModel.Name))
        //    {
        //        return new ResponseModel
        //        {
        //            Status = false,
        //            Message = "Name is required",
        //            Type = "warning"
        //        };
        //    }

        //    if (!CustomeValidatore.IsValidName(objModel.Name))
        //    {
        //        return new ResponseModel
        //        {
        //            Status = false,
        //            Message = "Name should contain only alphabets",
        //            Type = "warning"
        //        };
        //    }

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            DynamicParameters param = new DynamicParameters();

        //            param.Add("@category_id", objModel.CategoryID);
        //            param.Add("@name", objModel.Name);
        //            param.Add("@company_id", objModel.CompanyID);
        //            param.Add("@added_by", objModel.AddedBy);
        //            param.Add("@is_default", objModel.IsDefault);
        //            param.Add("@created_date", CommonHelper.GetDate);
        //            param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //            con.Execute("sp_insert_dropdown_value", param, commandType: CommandType.StoredProcedure);

        //            response.Status = true;
        //            response.ID = param.Get<int>("@id");
        //            response.Message = CommonHelper.SaveMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = false;
        //        response.Message = ex.Message;
        //        response.Type = "error";
        //        CommonHelper.WriteLog(ex.Message);
        //    }

        //    return response;
        //}



        public ResponseModel UpdateDropDownValue(DropDownMasterModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (string.IsNullOrWhiteSpace(objModel.Name))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Name is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(objModel.Name))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Name should contain only alphabets",
                    Type = "warning"
                };

            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_dropdown_value",
                        new
                        {
                            id = objModel.Id,
                            category_id = objModel.CategoryID,
                            name = objModel.Name,
                            company_id = objModel.CompanyID,
                            updated_by = objModel.UpdatedBy,
                            is_default = objModel.IsDefault,
                            updated_date = CommonHelper.GetDate
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = objModel.Id;
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

        public ResponseModel DeleteDropDownValue(int id, int userId)
        {
            ResponseModel response = new ResponseModel();

            if (id <= 0)
            {

                response.Status = false;
                response.Message = "Id must be greater than 0.";
                response.Type = "warning";
                return response;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_delete_dropdown_value",
                        new
                        {
                            id = id,
                            updated_by = userId
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    response.Status = true;
                    response.ID = id;
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
    }
}