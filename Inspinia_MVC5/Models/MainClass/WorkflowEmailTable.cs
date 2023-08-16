using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class WorkflowEmailTable
    {
        public long EmailID { get; set; }
        public Nullable<long> WorkflowEmailHeadID { get; set; }
        public Nullable<long> NumberRecords { get; set; }
        public string EmailBodyLable { get; set; }
        public string EmailBodyValue { get; set; }
        public string EmailBodyEventually { get; set; }
        public Nullable<long> Line { get; set; }
    }
}