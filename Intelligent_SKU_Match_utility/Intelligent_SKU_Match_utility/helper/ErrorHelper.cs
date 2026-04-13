using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_SKU_Match_utility.helper
{
    public class ErrorHelper
    {
        public static string strErrorLogs = ConfigurationManager.AppSettings["ErrorLogs"];
        public static void WriteError(string errorMessage, string strSource)
        {
            try
            {
                string path = strErrorLogs + "\\BSE_NSE_LOGS_" + DateTime.Today.ToString("ddMMyyyy") + ".txt";

                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine("\r\nLog Entry : " + strSource + " | " + DateTime.Now);
                    //w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "\t" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message.ToString(), "MethodLWriteError");
            }

        }
    }
}
