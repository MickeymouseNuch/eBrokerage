//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models.CashAdvance
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConfigCAD
    {
        public long ConfigID { get; set; }
        public string DeptID { get; set; }
        public string DeptCode { get; set; }
        public Nullable<bool> IsFN { get; set; }
        public Nullable<bool> IsGA { get; set; }
        public Nullable<decimal> AmountMax { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
    }
}
