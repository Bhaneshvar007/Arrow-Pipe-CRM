namespace SalesCrm.Models
{
    public class MailWorkflowModel
    {

        public int WorkFlowId { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string WorkFlowName { get; set; }
        public int LeadStage { get; set; }
        public string? LeadStageName { get; set; }
        public int Vertical { get; set; }
        public string? VerticalName { get; set; }
        public int Template { get; set; }
        public string? TemplateName { get; set; }
        public int MailAction { get; set; }
        public string? MailActionName { get; set; }

        public string EmailSubject { get; set; }
        public bool CustomerReplyedMail { get; set; }
        public int NewMailStatusReplyed { get; set; }
        public string? NewReplyedMailStatusName { get; set; }

        public bool CustomerNotReplyedMail { get; set; }
        public int NewMailStatusNotReplyed { get; set; }
        public string? NewNotReplyedMailStatusName { get; set; }

        public int MailBox { get; set; }
        public string? MailBoxName { get; set; }
        public string MailBoxCC { get; set; }
        public string MailBoxBCC { get; set; }

        public int IntervalDays { get; set; }
        public TimeSpan MailSendTime { get; set; }

        public bool WorkFlowAction { get; set; }
        public bool TrackEmail { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public int TotalCount { get; set; }
    }



}
