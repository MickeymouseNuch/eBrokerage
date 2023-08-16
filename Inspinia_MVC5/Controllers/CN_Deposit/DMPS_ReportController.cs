using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using Inspinia_MVC5.Models.MainClass;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models.DMPS;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_ReportController : Controller
    {
        #region -------------Data Connection----------------

        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();
        //MASEntities MASDB = new MASEntities();
        //cLog cLog = new cLog();
        #endregion

        #region -------------Index-------------
        public ActionResult Index()
        {
            var lstProject = (from t1 in DMPS.ProjectTables.Where(s => s.IsDelete != true) select new { ID = t1.ProjectID, DisplayName = t1.ProjectCode + " : " + t1.ProjectName }).OrderBy(s => s.ID).ToList();
            var lstDeveloper = (from t1 in DMPS.DevelopmentTables.Where(s => s.IsDelete != true ) select new { ID = t1.DevelopmentID, DisplayName =  t1.DevelopmentName }).OrderBy(s => s.DisplayName).ToList();

            var lstDocType = (from t1 in DMPS.UnitStatusMasterTables.Where(s => s.StatusID == 99999) select new { ID = t1.DescriptionTH, DisplayName =  t1.DescriptionENG }).OrderBy(s => s.ID).ToList();
            lstDocType.Add(new { ID = "DS", DisplayName = "ใบฝากทรัพย์" });
            lstDocType.Add(new { ID = "RV", DisplayName = "ใบจอง" });
            lstDocType.Add(new { ID = "RN", DisplayName = "สัญญาเช่า" });
            lstDocType.Add(new { ID = "RS", DisplayName = "สัญญาซื้อ" });

            var lstStatus = (from t1 in DMPS.UnitStatusMasterTables.Where(s => s.StatusID != 99999) select new { ID = t1.StatusID.ToString(), DisplayName = t1.DescriptionTH }).OrderBy(s => s.ID).ToList();
            lstStatus.Add(new { ID = "88", DisplayName = "ยกเลิกการจอง" });
            //var lstInCharge = (from t1 in DMPS.UnitStatusMasterTables.Where(s => s.StatusID != 99999) select new { ID = t1.DescriptionTH, DisplayName = t1.DescriptionENG }).OrderBy(s => s.ID).ToList();

            ViewBag.dll_Project = new SelectList(lstProject, "ID", "DisplayName");
            ViewBag.dll_Developer = new SelectList(lstDeveloper, "ID", "DisplayName");
            ViewBag.dll_DocType = new SelectList(lstDocType, "ID", "DisplayName");
            ViewBag.dll_Status = new SelectList(lstStatus, "ID", "DisplayName");
            //ViewBag.dll_InCharge = new SelectList(lstInCharge, "ID", "DisplayName");

            return View();
        }

        public ActionResult LoadDataListView(int Project = 0,int Developer = 0,string DocType = "", int Status = 99 ,DateTime? StartDate = null , DateTime? EndDate = null,string KeyIn = "")
        {
            ////ข้อมูลที่ต้องการค้นหา-------------------------------------------------------------------------Change
            //var q = MASDB.Projects.Where(s => s.ProjectID != "0");

            //if (ProjectID != "0")
            //{ q = q.Where(s => s.ProjectID == ProjectID); }


            //ViewBag.DataListView = q.OrderBy(s => s.ProjectID).ToList();
            //DataTable dt = new DataTable();
            //DataTable dtReturn = new DataTable();

            //var connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["PMDBSqlConnection"]);
            string SQL = "";

            if (DocType == "DS")
            {
                SQL = GetDataDeposit(Project, Developer, Status, StartDate, EndDate, KeyIn);
                var Data = DMPS.vw_rpt_Deposit.SqlQuery(SQL);
                ViewBag.DataListView = Data.OrderBy(s => s.DepositNo).ToList();
            }
            else if(DocType == "RV")
            {
                SQL = GetDataReservations(Project, Developer, Status, StartDate, EndDate, KeyIn);
                var Data = DMPS.vw_rpt_Reservations.SqlQuery(SQL);
                ViewBag.DataListView = Data.OrderBy(s => s.ReservationsNo).ToList();
            }
            else if (DocType == "RN")
            {
                SQL = GetDataRent(Project, Developer, Status, StartDate, EndDate, KeyIn);
                var Data = DMPS.vw_rpt_Rent.SqlQuery(SQL);
                ViewBag.DataListView = Data.OrderBy(s => s.RentNo).ToList();
            }
            else if (DocType == "RS")
            {
                SQL = GetDataResale(Project, Developer, Status, StartDate, EndDate, KeyIn);
                var Data = DMPS.vw_rpt_Resale.SqlQuery(SQL);
                if (DMPS.vw_rpt_Resale.SqlQuery(SQL) != null)
                {
                    return PartialView();

                }
                else
                {
                    ViewBag.DataListView = Data.OrderBy(s => s.ResaleNo).ToList();
                }

            }


            ViewBag.IsshowTab = DocType;

            return PartialView();
        }
        #endregion

        #region -------------Modal Popup------------
        public ActionResult LoadModal()
        {
            return PartialView();
        }


        private string GetDataDeposit(int Project, int Developer, int Status, DateTime? StartDate, DateTime? EndDate, string KeyIn = "")
        {
            string sql = "";

            sql = " SELECT * FROM  vw_rpt_Deposit where DepositNo <> '' ";

            if (Project != 0) { sql = sql + " AND ProjectID = " + Project + " ";  }

            if (Developer != 0) { sql = sql + " AND DeveloperID = " + Developer + " "; }

            if (Status != 99) { sql = sql + " AND DepositStatus = " + Status + " "; }

            if (StartDate != null && EndDate != null) { sql = sql + " and DepositDate between '" + StartDate + "' and '" + EndDate + "' "; }

            if (KeyIn.Trim() != "") { sql = sql + " and FirstName like '%" + KeyIn + "%' or  InChargeByName like '%" + KeyIn + "%' or DepositNo like '%" + KeyIn + "%'  "; }

            return sql;
        }

        private string GetDataReservations(int Project, int Developer, int Status, DateTime? StartDate, DateTime? EndDate, string KeyIn = "")
        {
            string sql = "";

            sql = " SELECT * FROM  vw_rpt_Reservations where ReservationsNo <> '' ";

            if (Project != 0) { sql = sql + " AND ProjectID = " + Project + " "; }

            if (Developer != 0) { sql = sql + " AND DevelopmentID = " + Developer + " "; }

            if (Status != 99 && Status!= 88 && Status != 5) { sql = sql + " AND DepositStatus = " + Status + " "; }

            if (Status == 88) { sql = sql + " AND IsCancel = " + 1 + " "; }

            if (Status == 5) { sql = sql + " AND IsDelete = " + 1 + " "; }

            if (StartDate != null && EndDate != null) { sql = sql + " and ReservationsDate between '" + StartDate + "' and '" + EndDate + "' "; }

            if (KeyIn.Trim() != "") { sql = sql + " and RV_FirstName like '%" + KeyIn + "%' or  InChargeByName like '%" + KeyIn + "%' or ReservationsNo like '%" + KeyIn + "%'  "; }

            return sql;
        }

        private string GetDataRent(int Project, int Developer, int Status, DateTime? StartDate, DateTime? EndDate, string KeyIn = "")
        {
            string sql = "";

            sql = " SELECT * FROM  vw_rpt_Rent where RentNo <> '' ";

            if (Project != 0) { sql = sql + " AND ProjectID = " + Project + " "; }

            if (Developer != 0) { sql = sql + " AND DevelopmentID = " + Developer + " "; }

            if ( Status != 99 && Status != 88 && Status != 5 && Status != 3) { sql = sql + " AND DepositStatus = " + Status + " "; }

            if ( Status == 5 &&  Status != 3) { sql = sql + " AND IsDelete = " + 1 + " "; }

            if ( Status == 3) { sql = sql + " AND IsEnd = 1 "; }

            if (StartDate != null && EndDate != null) { sql = sql + " and RentDate between '" + StartDate + "' and '" + EndDate + "' "; }

            if (KeyIn.Trim() != "") { sql = sql + " and RN_FirstName like '%" + KeyIn + "%' or  InChargeByName like '%" + KeyIn + "%' or RentNo like '%" + KeyIn + "%'  "; }

            return sql;
        }

        private string GetDataResale(int Project, int Developer, int Status, DateTime? StartDate, DateTime? EndDate, string KeyIn = "")
        {
            string sql = "";

            sql = " SELECT * FROM  vw_rpt_Rent where ResaleNo <> '' ";

            if (Project != 0) { sql = sql + " AND ProjectID = " + Project + " "; }

            if (Developer != 0) { sql = sql + " AND DevelopmentID = " + Developer + " "; }

            if (Status != 99 && Status != 88 && Status != 5) { sql = sql + " AND DepositStatus = " + Status + " "; }

            if (Status == 5) { sql = sql + " AND IsDelete = " + 1 + " "; }

            if (StartDate != null && EndDate != null) { sql = sql + " and ResaleDate between '" + StartDate + "' and '" + EndDate + "' "; }

            if (KeyIn.Trim() != "") { sql = sql + " and RS_FirstName like '%" + KeyIn + "%' or  InChargeByName like '%" + KeyIn + "%' or ResaleNo like '%" + KeyIn + "%'  "; }

            return sql;
        }

        #endregion


    }
}