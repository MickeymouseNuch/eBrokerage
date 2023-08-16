using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.CashAdvance
{
    public class cBudgetAccountTable
    {
        public long BudgetAccountID { get; set; }
        public string BudgetAccountCode { get; set; }
        public string BudgetAccountNameTH { get; set; }
        public string BudgetAccountNameEN { get; set; }
        public string BudgetAccountDescription { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<int> Reviser { get; set; }
        public Nullable<System.DateTime> ReviseDateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}