using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UVG_Main.Controllers.Report
{
    public class ReportViewerController : Controller
    {
        // GET: ReportViewer
        public ActionResult Index(string src)
        {
            ViewBag.srcReport = src;
            return View();
        }
    }
}