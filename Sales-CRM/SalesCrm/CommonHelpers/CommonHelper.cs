namespace SalesCrm.CommonHelpers
{
    public class CommonHelper
    {
        private static IConfiguration _config;
        private static string _connectionString;
        private static string Path => _config?["ErrorSettings:ErrorPath"];

        public static void Init(IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new Exception("Connection string not initialized. Call Helper.Init() first.");

            return _connectionString;
        }
 
        public static void WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

                string fileName = "Error_Log_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                string fullPath = System.IO.Path.Combine(Path, fileName);

                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine($"{DateTime.Now:dd-MMM-yyyy HH:mm:ss}\t{message}");
                }
            }
            catch
            {
                throw;
            }
        }


        public static DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static string SaveMessage
        {
            get
            {
                return "Data Saved Successfully.";
            }
        }
        public static string UpdateMessage
        {
            get
            {
                return "Data Updated Successfully.";
            }
        }
    
        public static string DeleteMessage
        {
            get
            {
                return "Data Deleted Successfully.";
            }
        }
    
    
    }
}
