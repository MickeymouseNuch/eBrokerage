//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BankTypeTable
    {
        public long BankTypeID { get; set; }
        public string BankTypeCode { get; set; }
        public string BankTypeNameTH { get; set; }
        public string BankTypeNameEN { get; set; }
        public string BankTypeDescription { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<int> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
