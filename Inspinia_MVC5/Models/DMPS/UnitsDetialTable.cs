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
    
    public partial class UnitsDetialTable
    {
        public long UnitDetialID { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public Nullable<decimal> RoomArea { get; set; }
        public Nullable<int> BedRoomQty { get; set; }
        public Nullable<int> LivingRoomQty { get; set; }
        public Nullable<int> ToiletRoomQty { get; set; }
        public Nullable<int> KitchenRoomQty { get; set; }
        public string RoomAddress { get; set; }
        public string Building { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> UnitsID { get; set; }
    }
}