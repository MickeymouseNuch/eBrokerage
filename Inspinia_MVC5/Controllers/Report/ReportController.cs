using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using UVG_Main.Models.MainClass;
using System.Configuration;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers.Report
{
    public class ReportController : Controller
    {
        MASDBEntities MASDB = new MASDBEntities();
        //cLog cLog = new cLog();

        // GET: Report
        public ActionResult Index(string ReportCode = "")
        {
            try
            {
                //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
                var dataReport = MASDB.ReportTables.SingleOrDefault(s => s.IsDelete == false && s.ReportCode == ReportCode);
                ViewBag.dataReport = dataReport;
                var lstCri = MASDB.ReportCriteriaTables.Where(s => s.ReportCode == ReportCode && s.IsDelete == false).OrderBy(s => s.CriteriaRow).ToList();
                ViewBag.lstCri = lstCri;
                var maxRow = MASDB.ReportCriteriaTables.Where(s => s.ReportCode == ReportCode && s.IsDelete == false).Max(s => s.CriteriaRow).GetValueOrDefault();
                ViewBag.maxRow = maxRow;
            }
            catch (Exception ex)
            {
             //   cLog.StampErrorLogTrans(0, "Report", "Index", ex.ToString(), "");
            }
            return View();
        }

        public string getListCriteria(string ReportCode = "")
        {
            var lstCri = MASDB.ReportCriteriaTables.Where(s => s.ReportCode == ReportCode && s.IsDelete == false).OrderBy(s => s.CriteriaRow).ToList();
            return lstCri.ToObj2Json();
        }

        public string getData2DDL(string CriteriaQuery)
        {
            string result = string.Empty;
            var tempDDL = MASDB.Database.SqlQuery<cDDL>(CriteriaQuery).ToList();
            string aa = "0";
            tempDDL.Add(new cDDL { id = aa, text = "Please Select" });
            result = tempDDL.ToObj2Json();
            return result;
        }

        public ActionResult Preview(string ReportCode, List<string> objName, List<string> objValue)
        {
            DataTable dt = new DataTable();
            ReportTable rptT = MASDB.ReportTables.SingleOrDefault(s => s.ReportCode == ReportCode && s.IsDelete == false);
            dt = getData(rptT.ReportDBConnection, rptT.ReportProcedure, objName, objValue);
            return PartialView(dt);
        }

        public ActionResult ReportViewer(string ReportCode, string objName, string objValue)
        {
            //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");

            //ReportTable rptT = MASDB.ReportTables.SingleOrDefault(s => s.ReportCode == ReportCode && s.IsDelete == false);
            //DataTable dtData = getData(rptT.ReportDBConnection, rptT.ReportProcedure, objName.Split(',').ToList(), objValue.Split(',').ToList());
            //Session["dtData"] = dtData;
            //return PartialView();

            //DataSet ds = new DataSet();
            //DataTable dt1 = new DataTable();
            //DataTable dt2 = new DataTable();
            //dt1 = getData2("", "SP_GetDocDefectByDocNo", objValue);
            //dt2 = getData2("", "SP_GetItemDefect", objValue);
            //ds.Tables.Add(dt1);
            //ds.Tables.Add(dt2);
            //Session["dtData"] = ds;

            //Session["paramName"] = "@" + objName;
            //Session["paramValue"] = objValue;

            Session["paramName"] = objName;
            Session["paramValue"] = objValue;
            return PartialView();
        }

        public ActionResult GetReport(string ReportCode)
        {
            Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
            ReportTable rptT = MASDB.ReportTables.SingleOrDefault(s => s.ReportCode == ReportCode && s.IsDelete == false);
            string rptPath = Server.MapPath(rptT.ReportPath + rptT.ReportFileName);
            
            StiReport report = new StiReport();
            report.Dictionary.DataStore.Clear();
            report.Load(rptPath);
            report.Compile();

            //set Parameter to Stimusoft Report
            var _Paramater = Session["paramName"].ToString().Split('|');
            var _Value = Session["paramValue"].ToString().Split('|');
            for (int i = 0; i < _Paramater.Count(); i++)
            {
                report["@" + _Paramater[i].ToString()] = _Value[i].ToString();
            }
            report.Render();

            return StiMvcViewer.GetReportResult(report);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }

        public DataTable getData(string sqlConnection, string procName, List<string> objName, List<string> objValue)
        {
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings[sqlConnection];
            SqlConnection connection = new SqlConnection(Conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(procName, connection);
            command.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < objName.Count; i++)
            {
                command.Parameters.AddWithValue("@" + objName[i].ToString(), objValue[i].ToString());
            }
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            return dt;
        }

        public DataTable getData2(string sqlConnection, string procName, string objValue)
        {
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["HFDBSqlConnection"];
            SqlConnection connection = new SqlConnection(Conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(procName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@param_docno", objValue);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            return dt;
        }

        public void PrintToPrinter(string ReportCode, string objName, string objValue)
        {
            try
            {
                string printername = ConfigurationManager.AppSettings["PrinterName"].ToString();

                ReportTable rpt = MASDB.ReportTables.SingleOrDefault(s => s.ReportCode == ReportCode && s.IsDelete == false);
                string rptPath = Server.MapPath(rpt.ReportPath + rpt.ReportFileName);
                StiReport report = new StiReport();


                report.PrinterSettings.PrinterName = printername;
                report.Load(rptPath);
                report["@" + (string)objName] = objValue;

                report.Print(showPrintDialog: false);
            }
            catch (Exception ex)
            {
                var param_er = ex.Message.ToString();
            }
        }
    }
}