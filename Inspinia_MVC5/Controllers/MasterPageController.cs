using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;
using Inspinia_MVC5.Models.MainClass;
using Inspinia_MVC5.API;
using System.Web.Configuration;
using System.Configuration;
using System.IO;

namespace Inspinia_MVC5.Controllers
{
    public class MasterPageController : Controller
    {
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();
        
        public ActionResult Index(string ApplicationCode)
        {
            var myDecrypt = ApplicationCode.ToDecrypt(true);

            //myDecrypt = "fyGjrS5TdLQLrqxMaHwXUw==".ToDecrypt(true);

            string EmployeeID = myDecrypt.Substring(0, myDecrypt.Length - 3);
            long ApplicationID = myDecrypt.Substring((myDecrypt.Length - 3), 3).ToInt32();


            //string EmployeeID = "10610538";
            //long ApplicationID = 28;

            cEmployeeDetail EmployeeDetail = cApi.apiGetEmployeeDetail(EmployeeID);
            ViewBag.ApplicationID = ApplicationID.ToString();

            cRoleAdminApp RoleAdmin = getRoleAdminApp(cApi.apiGetRoleAdminApp(EmployeeID, ApplicationID));
            ViewBag.RoleAdmin = RoleAdmin;

            //Footer display################################################################
            ViewBag.ServerName = ConfigurationManager.AppSettings["ServerName"];
            ViewBag.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];

            ViewBag.UserName = EmployeeDetail.UserLogon.ToString();
            ViewBag.DisplayName = EmployeeDetail.EmpID.ToString() + " : " + EmployeeDetail.DisplayName.ToString();
            //##############################################################################
            
            return View(EmployeeDetail);
        }

        public ActionResult PageInfo(long ApplicationID)
        {
            cApplication cApp = cApi.apiGetApplication(ApplicationID);
            return PartialView(cApp);
        }      

        public ActionResult LoadTopBar(string EmployeeID,long ApplicationID = 0)
        {
            var LstMainMenu = cApi.apiGetMainMenuList(EmployeeID, ApplicationID);
            var LstSubMenu = cApi.apiGetSubMenuList(EmployeeID, ApplicationID);         

            ViewBag.LstMainMenu = LstMainMenu;
            ViewBag.LstSubMenu = LstSubMenu;
            return PartialView();
        }

        public string getUrlAppportal(string EmployeeID,long ApplicationID)
        {
            string result = string.Empty;
            cApplication cApp = cApi.apiGetApplication(ApplicationID);
            result = cApp.ApplicationUrlPublic + "?MyEmpID=" + EmployeeID.ToEncrypt(true);
            return result;
        }

        private cRoleAdminApp getRoleAdminApp(cRoleAdminApp data)
        {
            if(data.RoleAdminID == 0)
            {
                data.RoleAdmin = 0;
                data.ViewOnly = false;
                data.IsModify = false;
            }
            else { data.RoleAdmin = 1; }
            return data;
        }
        public FileResult loadManual()
        {

            string pathSource = Server.MapPath("~/Report/Manual/Manual_EB.pdf");
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, "application/pdf");
        }
    }
}