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
    
    public partial class DocumentStatusTable
    {
        public int DocumentStatusID { get; set; }
        public string DocumentStatusName { get; set; }
        public string DocumentStatusNameEN { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public string Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDateTime { get; set; }
        public string Action { get; set; }
        public string LableStyle { get; set; }
        public string FontColor { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}