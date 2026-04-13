using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SalesCrm.CommonHelpers;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.Data;

namespace SalesCrm.Services.DAL
{
    public class DALAIGeneratedSuit:IDALAIGeneratedSuit
    {
        private readonly string _connectionString;

        public DALAIGeneratedSuit(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<AIGeneratedSuitModel> GetAIGeneratedSuitEnquirys(AIGeneratedSuitRequest req)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var data = con.Query<AIGeneratedSuitModel>(
                        "sp_Get_MatchedEnquiry",
                        new
                        {
                            enq_id = req.EnqId,
                            page_id = req.PageId,
                            page_ref = req.PageRef
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    foreach (var item in data)
                    {
                        try
                        {
                            // debug log
                            CommonHelper.WriteLog("JSON: " + item.AttachmentsJson);

                            if (!string.IsNullOrWhiteSpace(item.AttachmentsJson)
                                && item.AttachmentsJson.Trim().StartsWith("["))
                            {
                                item.Attachments = JsonConvert.DeserializeObject<List<AttachmentModel>>(item.AttachmentsJson);
                            }
                             
                        }
                        catch (Exception ex)
                        {
                            item.Attachments = new List<AttachmentModel>();

                            //  FULL error log
                            CommonHelper.WriteLog("JSON ERROR: " + ex.ToString());
                        }
                    }

                    return data;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog(ex.Message);
                throw new Exception("Error while fetching matched enquiry data.", ex);
            }
        }
   
    
    }
}
