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
    
    public partial class WorkTran
    {
        public long WorkTransID { get; set; }
        public string WorkNO { get; set; }
        public Nullable<long> WorkID { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerRoad { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerPhone { get; set; }
        public Nullable<long> WorkTypeID { get; set; }
        public string WorkTypeText { get; set; }
        public string WorkDetail { get; set; }
        public Nullable<long> StartTime { get; set; }
        public Nullable<long> EndTime { get; set; }
        public Nullable<long> TimeTypeID { get; set; }
        public string SpecialOrder { get; set; }
        public Nullable<long> OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerMobile { get; set; }
        public Nullable<long> MessengerID { get; set; }
        public Nullable<int> DocumentStatusID { get; set; }
        public string Remark { get; set; }
        public Nullable<long> SendeMailOpen { get; set; }
        public Nullable<long> SendeMailCancel { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> DeleteBy { get; set; }
    }
}
