namespace SalesCrm.Models
{
    public class ResponseModel
    {

        public int ID { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }
    }
}
