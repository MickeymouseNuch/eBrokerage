using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class WorkflowTask
    {
        public long TaskId { get; set; }
        public Nullable<long> ApplicationId { get; set; }
        public string WorkflowTaskNo { get; set; }
        public long WorkflowId { get; set; }
        public long VersionNo { get; set; }
        public string DocumentId { get; set; }
        public Nullable<long> WorkflowStateID { get; set; }
        public long StateId { get; set; }
        public string EmpId { get; set; }
        public long DeptId { get; set; }
        public Nullable<long> UserId { get; set; }
        public string UserLogon { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> ActionDatetime { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string SubmitedBy { get; set; }
        public Nullable<bool> EmailSent { get; set; }
        public Nullable<System.DateTime> EmailDatetime { get; set; }
        public bool UnRead { get; set; }
        public string Comment { get; set; }
    }
}