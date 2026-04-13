using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALDropDownCategory : IDALDropDownCategory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALDropDownCategory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<DropDownCategoryModel> GetCategories(RequestModel request)
        {
            List<DropDownCategoryModel> data = new List<DropDownCategoryModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<DropDownCategoryModel>(
                        "sp_get_dropdown_category",
                        new
                        {
                            company_id = request.CompanyID,
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

        public DropDownCategoryModel GetCategoryById(int id)
        {
            DropDownCategoryModel data = new DropDownCategoryModel();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    data = con.Query<DropDownCategoryModel>(
                        "sp_get_dropdown_category_byid",
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

        public ResponseModel AddCategory(DropDownCategoryModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (string.IsNullOrWhiteSpace(objModel.CategoryName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Category name is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(objModel.CategoryName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Category name should contain only alphabets",
                    Type = "warning"
                };
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@category_name", objModel.CategoryName);
                    param.Add("@company_id", objModel.CompanyID);
                    param.Add("@added_by", objModel.CompanyUserID);
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("sp_insert_dropdown_category", param, commandType: CommandType.StoredProcedure);

                    int result = param.Get<int>("@id");

                    if (result == -1)
                    {
                        response.Status = false;
                        response.Message = "Category already exists!";
                        response.Type = "warning";
                    }
                    else
                    {
                        response.Status = true;
                        response.ID = result;
                        response.Message = CommonHelper.SaveMessage;
                    }
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

        public ResponseModel UpdateCategory(DropDownCategoryModel objModel)
        {
            ResponseModel response = new ResponseModel();

            if (string.IsNullOrWhiteSpace(objModel.CategoryName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Category name is required",
                    Type = "warning"
                };
            }

            if (!CustomeValidatore.IsValidName(objModel.CategoryName))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Category name should contain only alphabets",
                      Type = "warning"
                };
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_update_dropdown_category",
                        new
                        {
                            id = objModel.Id,
                            category_name = objModel.CategoryName,
                            company_id = objModel.CompanyID,
                            updated_by = objModel.LastUpdatedBy
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

        public ResponseModel DeleteCategory(int id, int userId)
        {
            ResponseModel response = new ResponseModel();

            if (id <= 0)
            {

                response.Status = false;
                response.Message = "CategoryId must be greater than 0";
                response.Type = "warning";
                return response;
            }



            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Execute(
                        "sp_delete_dropdown_category",
                        new { id, updated_by = userId },
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