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
    
    public partial class ProjectTable
    {
        public long ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public Nullable<long> DevelopmentID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNameEng { get; set; }
        public Nullable<bool> ProjectStatus { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<int> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
