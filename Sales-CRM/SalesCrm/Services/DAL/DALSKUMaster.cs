using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Collections.Generic;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALSKUMaster : IDAlSKUMaster
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALSKUMaster(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }


        public List<SKUMasterModel> GetSkuMaster(SkuFilterModel req)
        {
            List<SKUMasterModel> list = new List<SKUMasterModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<SKUMasterModel>(
                        "sp_get_sku_master",
                         new
                         {
                             page_number = req.PageNumber,
                             page_size = req.PageSize,
                             material = req.material,
                             grade = req.grade,
                             brand = req.brand
                         },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        list = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return list;
        }

        public List<SKUMasterModel> GetSkuSuggestion(SkuSuggestionFilterModel req)
        {
            List<SKUMasterModel> list = new List<SKUMasterModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<SKUMasterModel>(
                        "sp_get_sku_master_bysuggation",
                         new
                         {
                             description = req.Description,
                             material = req.Material,
                             type = req.Type,
                             size = req.Size,
                             sch = req.Sch,
                             grade = req.Grade,
                             brand = req.Brand
                         },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    if (data != null)
                        list = data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
            }

            return list;
        }


        public List<SKUDropDownModel> GetSkuDropDownMaster(string type)
        {
            List<SKUDropDownModel> list = new List<SKUDropDownModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSKUDropdownData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", type);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SKUDropDownModel
                                {
                                    Text = reader["Text"]?.ToString(),
                                    Value = reader["Value"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
                throw;
            }

            return list;
        }
    }
}
