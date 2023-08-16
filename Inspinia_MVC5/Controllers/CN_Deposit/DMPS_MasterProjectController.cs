using Inspinia_MVC5;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models;
using Inspinia_MVC5.Models.DMPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_MasterProjectController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();


        public ActionResult Index()
        {
            //ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.Where(s => s.IsDelete == false).OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");

            return View();
        }

      
        public ActionResult LoadDataListView(string _SearcheName, long _id)
        {
            var qdata = DMPS.vw_Project.Where(s => s.ProjectID != 0 /*&& s.IsDelete != true*/);

            if (_id != 0) { qdata = qdata.Where(s => s.ProjectID == _id); }

            if (_SearcheName != "")
            {
                _SearcheName = _SearcheName.ToUpper();
                qdata = qdata.Where(s => (s.ProjectName.ToUpper()).Contains(_SearcheName) || (s.ProjectCode.ToUpper()).Contains(_SearcheName) || (s.DevelopmentName.ToUpper()).Contains(_SearcheName));
            }

            ViewBag.lstData = qdata.OrderBy(s => s.IsDelete).ThenBy(s => s.DevelopmentName).ThenBy(s => s.ProjectCode).ToList();


            return PartialView();
        }
        public string getDeveloper(int ThisID)
        {

            string result = string.Empty;
            //var lstData = (from data in MASDB.AmphurTables where (data.AMPHUR_ID != 0) select new { id = data.AMPHUR_ID, text = data.AMPHUR_NAME }).ToList(); ;
            var lstData = (from data in DMPS.DevelopmentTables where (data.IsDelete == false) select new { id = data.DevelopmentID, text = data.DevelopmentName }).ToList(); ;
            if (ThisID != 0)
            {
                lstData = (from data in DMPS.DevelopmentTables select new { id = data.DevelopmentID, text = data.DevelopmentName }).OrderBy(data => data.text).ToList(); ;

            }

            long a = 0;
            //var p = new { id = 0, text = "Please Select" };
            lstData.Add(new { id = a, text = "Please Select" });
            result = lstData.ToObj2Json();
            return result;
        }

        public ActionResult LoadModal()
        {
            //ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.Where(s => s.IsDelete == false).OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            //ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");

            return PartialView();
        }

        public string LoadData2Modal(long id)
        {

            string result = string.Empty;
            List<Object> obj = new List<object>();
            var Creater = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();
            var Reviser = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();

            var data = DMPS.ProjectTables.Where(s => s.ProjectID == id).SingleOrDefault();


            if (data == null)
            {
                data = new ProjectTable();
                data.ProjectID = 0;
                data.DevelopmentID = 0;
                data.ProjectCode = "";
                data.ProjectName = "";
                data.ProjectNameEng = "";
                data.IsDelete = false;
            }
             else
            {
                if (data.Creator != null) { Creater = MASDB.STG_EMPLOYEEVw.Where(s => s.EM_CODE == data.Creator.ToString()).SingleOrDefault(); }
                if (data.Reviser != null) { Reviser = MASDB.STG_EMPLOYEEVw.Where(s => s.EM_CODE == data.Reviser.ToString()).SingleOrDefault(); }

            }

            //---------------------------------------------------------------------------------------
            obj.Add(data);
            obj.Add(Creater);
            obj.Add(Reviser);

            result = obj.ToObj2Json();

            return result;
        }

        public long SaveProjects(ProjectTable _Data)
        {
            var Data = DMPS.ProjectTables.Where(s => s.ProjectID == _Data.ProjectID).SingleOrDefault();
            try
            {
                if (Data == null)
                {
                    Data = new ProjectTable();
                    Data.ProjectCode = _Data.ProjectCode;
                    Data.ProjectName = _Data.ProjectName;
                    Data.ProjectNameEng = _Data.ProjectNameEng;
                    Data.DevelopmentID = _Data.DevelopmentID;
                    Data.Creator = _Data.Creator;
                    Data.CreateDateTime = DateTime.Now;
                    Data.Reviser = _Data.Reviser;
                    Data.ReviseDateTime = DateTime.Now;
                    Data.IsDelete = _Data.IsDelete;
                    DMPS.ProjectTables.Add(Data);

                }
                else
                {
                    Data.ProjectCode = _Data.ProjectCode;
                    Data.ProjectName = _Data.ProjectName;
                    Data.ProjectNameEng = _Data.ProjectNameEng;
                    Data.DevelopmentID = _Data.DevelopmentID;
                    Data.Reviser = _Data.Reviser;
                    Data.ReviseDateTime = DateTime.Now;
                    Data.IsDelete = _Data.IsDelete;
                    DMPS.Entry(Data).State = System.Data.Entity.EntityState.Modified;

                }

                DMPS.SaveChanges();
 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return Data.ProjectID;
        }

        public string CheckDuplicate(int _Dev, string _Code)
        {
            if (_Dev != 0)
            {
                if (_Code != "")
                {

                    var ChkDup = DMPS.ProjectTables.Where(s => s.ProjectCode.ToUpper() == _Code.ToUpper() && s.DevelopmentID == _Dev).ToList();
                    return ChkDup.ToObj2Json();
                }
            }
            


            return "false";
        }
    }
}