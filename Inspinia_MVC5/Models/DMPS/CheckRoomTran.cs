//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models.DMPS
{
    using System;
    using System.Collections.Generic;
    
    public partial class CheckRoomTran
    {
        public long CheckRoomTransID { get; set; }
        public Nullable<long> CheckRoomID { get; set; }
        public Nullable<long> CheckType { get; set; }
        public Nullable<long> ID { get; set; }
        public string DocsType { get; set; }
        public string CheckRoomQty { get; set; }
        public Nullable<bool> IsBroken { get; set; }
        public string BrokenRoomQty { get; set; }
        public Nullable<bool> IsEnd { get; set; }
        public Nullable<System.DateTime> IsEndDate { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}