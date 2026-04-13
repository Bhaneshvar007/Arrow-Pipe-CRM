using System.Configuration;
using System.Data.SqlClient;

namespace Intelligent_SKU_Match_utility.Data
{
    internal class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.AppSettings["Constr"];
        }

        internal SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}