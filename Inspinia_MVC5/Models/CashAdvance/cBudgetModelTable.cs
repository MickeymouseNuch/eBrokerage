using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.CashAdvance
{
    public class cBudgetModelTable
    {
        public long BudgetModelID { get; set; }
        public string BudgetModelCode { get; set; }
        public string BudgetModelNameTH { get; set; }
        public string BudgetModelNameEN { get; set; }
        public string BudgetModelDescription { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<int> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}