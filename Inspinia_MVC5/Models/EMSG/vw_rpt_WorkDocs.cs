//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models.EMSG
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_rpt_WorkDocs
    {
        public long WorkID { get; set; }
        public string WorkNO { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public Nullable<long> TimeTypeID { get; set; }
        public string TimeTypeNameTH { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public Nullable<long> WorkTypeID { get; set; }
        public string WorkTypeNameTH { get; set; }
        public string OwnerName { get; set; }
        public string DeptDescription { get; set; }
        public string CompanyName { get; set; }
        public string CP_ADDR { get; set; }
        public string CP_PHONE { get; set; }
        public string CP_FAX { get; set; }
        public string ImageURL { get; set; }
        public string WorkDetail { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerMobile { get; set; }
        public long WorkTransID { get; set; }
    }
}