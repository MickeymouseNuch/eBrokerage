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
    
    public partial class CallVisitActivity
    {
        public int CallVisitActivityID { get; set; }
        public int CallVisitID { get; set; }
        public Nullable<int> DevelopmentID { get; set; }
        public int ProjectID { get; set; }
        public int ActivityID { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public Nullable<int> TopicID { get; set; }
        public string TopicOther { get; set; }
        public string ActivityDetails { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}