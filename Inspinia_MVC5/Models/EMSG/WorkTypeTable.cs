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
    
    public partial class WorkTypeTable
    {
        public long WorkTypeID { get; set; }
        public string WorkTypeNameTH { get; set; }
        public string WorkTypeNameEN { get; set; }
        public Nullable<bool> IsText { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<bool> IsTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> DeleteBy { get; set; }
    }
}
