using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class WorkflowEmailLog
    {
        public long WorkflowEmailLog_ID { get; set; }
        public Nullable<long> WorkflowEmailHeadID { get; set; }
        public string WorkflowEmailLog_Userlogon { get; set; }
        public string WorkflowEmailLog_RecipientEmail { get; set; }
        public string WorkflowEmailLog_Message { get; set; }
        public string WorkflowEmailLog_Status { get; set; }
        public Nullable<System.DateTime> WorkflowEmailLog_Date { get; set; }
        public Nullable<long> EMailTypeID { get; set; }
    }
}