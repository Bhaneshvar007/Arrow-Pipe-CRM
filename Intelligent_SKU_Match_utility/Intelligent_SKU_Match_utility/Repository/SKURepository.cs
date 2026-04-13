using Intelligent_SKU_Match_utility.Data;
using Intelligent_SKU_Match_utility.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.Repository
{
    public class SKURepository
    {
        DatabaseHelper db = new DatabaseHelper();

        public List<SKU> GetTop10SKU(string searchText)
        {
            List<SKU> list = new List<SKU>();

            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_GetTopRankedSKUS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@searchText", searchText);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new SKU
                    {
                        SKU_Id = Convert.ToInt32(dr["SKU_Id"]),
                        Description = dr["Description"].ToString(),
                        FullTextRank = (int)Convert.ToDouble(dr["FullTextRank"]),
                        RankPercent = (int)Convert.ToDouble(dr["FinalPercent"])
                    });
                }
            }

            return list;
        }



        public void InsertMatchResult(int inquiryId, int skuId, double score, string classification)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertMatchResult", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InquiryId", inquiryId);
                cmd.Parameters.AddWithValue("@SKU_Id", skuId);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.Parameters.AddWithValue("@Classification", classification);
                cmd.Parameters.AddWithValue("@Item", 1);

                cmd.ExecuteNonQuery();
            }
        }


    }
}
