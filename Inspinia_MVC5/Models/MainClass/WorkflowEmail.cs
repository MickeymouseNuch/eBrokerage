using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class WorkflowEmail
    {
        public long WorkflowEmailHeadID { get; set; }
        public Nullable<long> ApplicationId { get; set; }
        public string DocumentId { get; set; }
        public string WorkflowEmail1 { get; set; }
        public string WorkflowEmailSubject { get; set; }
        public string WorkflowEmailHeader { get; set; }
        public string WorkflowEmailFooter { get; set; }
        public string WorkflowEmailStattus { get; set; }
        public Nullable<long> TaskId { get; set; }
    }
}