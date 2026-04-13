using Dapper;
using Microsoft.Data.SqlClient;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALEmailConfigurations:IDALEmailConfigurations
    {



        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _connectionString;

        public DALEmailConfigurations(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = CommonHelper.GetConnectionString();
        }

        public List<EmailConfigurationsModel> GetEmailConfigurations(RequestModel model)
        {
            List<EmailConfigurationsModel> list = new List<EmailConfigurationsModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<EmailConfigurationsModel>(
                        "sp_get_email_configurations",
                        new
                        {
                            company_id = model.CompanyID,
                            company_user_id = model.CompanyUserID,
                            page_number = model.PageNumber,
                            page_size = model.PageSize
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
    }
}
