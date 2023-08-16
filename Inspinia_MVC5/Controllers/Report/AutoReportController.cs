using Inspinia_MVC5.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace UVG_Main.Controllers.Report
{
    public class AutoReportController : Controller
    {
        // GET: AutoReport
        MASDBEntities MASDB = new MASDBEntities();

        public class Email
        {
            public long EmailTransID { get; set; }
            public string EmailAddress { get; set; }
            public string MailType { get; set; }
        }

        public class Subject
        {
            public string SubjectText { get; set; }
        }

        public ActionResult Index(string ReportCode, string objName = "", string objValue = "")
        {
            //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
            ////ดึงข้อมูลรายงานจาก Database [WH-MAS].ReportForAutoMailTable จาก ReportCode
            //ReportForAutoMailTable rp = MASDB.ReportForAutoMailTables.SingleOrDefault(s => s.ReportCode == ReportCode);
            //if (rp == null) { return View(); }//กรณีไม่มีข้อมูลให้เด้งออกไปเลย

            ////สร้างรายงาน
            //string rptPath = Server.MapPath(rp.ReportPath + rp.ReportFileName);
            //StiReport report = new StiReport();
            //report.Dictionary.DataStore.Clear();
            //report.Load(rptPath);
            //report.Compile();

            ////นำพารามิเตอร์เข้ารีพอร์ท
            //if (objName != "")
            //{
            //    var _Paramater = objName.Split('|');
            //    var _Value = objValue.Split('|');
            //    for (int i = 0; i < _Paramater.Count(); i++)
            //    {
            //        report["@" + _Paramater[i].ToString()] = _Value[i].ToString();
            //    }
            //}
            //report.Render();

            ////Save Html ไปที่ Text File ก่อนทำการแปลงเป็น Inline Html เพื่อนำส่งเมล์
            //string file = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["pathTempFileReportAutoMail"].ToString());
            //file += ReportCode + "_" + DateTime.Now.ToString("yyyyMMdd") + ".html";
            //report.ExportDocument(StiExportFormat.Html, file);
            //System.Diagnostics.Process.Start(file);
            ////อ่านข้อมูลจาก Text File ที่ Gen ได้ แปลงเป็น Inline Html เพื่อนำส่งเมล์
            //string html = System.IO.File.ReadAllText(file);
            //var htmlInline = PreMailer.Net.PreMailer.MoveCssInline(html).Html;

            ////กำหนด Subject Mail 
            //string SubjectMail = string.Empty;
            //string procedureName = string.Format("exec sp_getSubjectAutoReport '{0}'", rp.ReportCode);
            //Subject sj = MASDB.Database.SqlQuery<Subject>(procedureName).SingleOrDefault();
            //if (sj != null) { SubjectMail = sj.SubjectText; }

            ////เรียก Method ส่งเมล์
            //cMail _cMail = new cMail();
            //_cMail.SendAutoMail(rp.ReportID, SubjectMail, htmlInline);

            //return View();

            try
            {
                //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
                ////ดึงข้อมูลรายงานจาก Database [WH-MAS].ReportForAutoMailTable จาก ReportCode
                ReportForAutoMailTable rp = MASDB.ReportForAutoMailTables.SingleOrDefault(s => s.ReportCode == ReportCode);
                if (rp == null) { return View(); }//กรณีไม่มีข้อมูลให้เด้งออกไปเลย

                //สร้างรายงาน
                string rptPath = Server.MapPath(rp.ReportPath + rp.ReportFileName);
                StiReport report = new StiReport();
                report.Dictionary.DataStore.Clear();
                report.Load(rptPath);
                report.Compile();

                //นำพารามิเตอร์เข้ารีพอร์ท
                if (objName != "")
                {
                    var _Paramater = objName.Split('|');
                    var _Value = objValue.Split('|');
                    for (int i = 0; i < _Paramater.Count(); i++)
                    {
                        report["@" + _Paramater[i].ToString()] = _Value[i].ToString();
                    }
                }
                report.Render();

                //Save Html ไปที่ Text File ก่อนทำการแปลงเป็น Inline Html เพื่อนำส่งเมล์
                string file = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["pathTempFileReportAutoMail"].ToString());
                file += ReportCode + "_" + DateTime.Now.ToString("yyyyMMdd") + ".html";
                report.ExportDocument(StiExportFormat.Html, file);
                System.Diagnostics.Process.Start(file);
                //อ่านข้อมูลจาก Text File ที่ Gen ได้ แปลงเป็น Inline Html เพื่อนำส่งเมล์
                string html = System.IO.File.ReadAllText(file);
                var htmlInline = PreMailer.Net.PreMailer.MoveCssInline(html).Html;

                ////กำหนด Subject Mail 
                string SubjectMail = string.Empty;
                string procedureName = string.Format("exec sp_getSubjectAutoReport '{0}'", rp.ReportCode);
                Subject sj = MASDB.Database.SqlQuery<Subject>(procedureName).SingleOrDefault();
                if (sj != null) { SubjectMail = sj.SubjectText; }

                ////เรียก Method ส่งเมล์
                cMail _cMail = new cMail();
                _cMail.SendAutoMail(rp.ReportID, SubjectMail, htmlInline);

            }
            catch (Exception ex)
            {
                cLog clog = new cLog();
                clog.StampErrorLogTrans(0, "AutoReport", "TestAutoReport", ex.ToString(), "pisarn.s");
            }

            return View();
        }






















        public ActionResult getHTML2String(string ReportCode, string objName, string objValue)
        {            
            return View();
        }
        
        public ActionResult getReport(string ReportCode, string objName, string objValue)
        {
            //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
            string rptPath = Server.MapPath("~/Report/AutoMailReport/" + ReportCode + ".mrt");

            StiReport report = new StiReport();
            report.Dictionary.DataStore.Clear();
            report.Load(rptPath);
            report.Compile();

            ////set Parameter to Stimusoft Report
            //var _Paramater = objName.Split('|');
            //var _Value = objValue.Split('|');
            //for (int i = 0; i < _Paramater.Count(); i++)
            //{
            //    report["@" + _Paramater[i].ToString()] = _Value[i].ToString();
            //}
            report.Render();

            StiReportResponse.ResponseAsHtml(report);

            string file = Server.MapPath("~/AttachFile/AutoMailStimulsoft/Test");
            file += ".html";
            report.ExportDocument(StiExportFormat.Html, file);
            System.Diagnostics.Process.Start(file);
            
            string html = System.IO.File.ReadAllText(file);
            var htmlInline = PreMailer.Net.PreMailer.MoveCssInline(html).Html;

            cMail _cMail = new cMail();
            //_cMail.SendAutoMail("Test AutoMail", htmlInline);
            return PartialView();
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string RenderViewToString<T>(string viewPath, T model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(ControllerContext, viewPath);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(ControllerContext, view, vdd,
                                            new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

        public void SendMail(string HtmlString)
        {
            string a = HtmlString;
        }

        private void SendMail()
        {

        }
    }
}