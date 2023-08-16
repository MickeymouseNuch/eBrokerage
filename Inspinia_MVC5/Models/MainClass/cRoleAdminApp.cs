using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cRoleAdminApp
    {
        public int RoleAdminID { get; set; }
        public Nullable<long> ApplicationId { get; set; }
        public string EmpID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsModify { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDate { get; set; }
        public Nullable<bool> ViewOnly { get; set; }
        public Nullable<int> RoleAdmin { get; set; }
    }
}