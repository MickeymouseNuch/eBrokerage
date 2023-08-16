using Inspinia_MVC5;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models;
using Inspinia_MVC5.Models.DMPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_MasterDevelopperController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadDataListView(string _SearcheName, long _id)
       {
            var qdata = DMPS.DevelopmentTables.Where(s => s.DevelopmentID != 0);

            if (_id != 0) 
            { qdata = qdata.Where(s => s.DevelopmentID == _id); }

            if (_SearcheName != "")
            {
                _SearcheName = _SearcheName.ToUpper();
                qdata = qdata.Where(s => (s.DevelopmentName.ToUpper()).Contains(_SearcheName) || (s.DevelopmentCode.ToUpper()).Contains(_SearcheName));
            }

            ViewBag.lstData = qdata.OrderBy(s => s.IsDelete).ThenBy(s => s.DevelopmentCode).ThenBy(s => s.DevelopmentName).ToList();


            return PartialView();
        }

        public ActionResult LoadModal()
        {
            return PartialView();
        }

        public string LoadData2Modal(long id)
        {
            string result = string.Empty;
            List<Object> obj = new List<object>();
            //var Creater = cApi.apiGetEmployeeDetail("");
            //var Reviser = cApi.apiGetEmployeeDetail("");
            
            var Creater = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();
            var Reviser = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();

            var data = DMPS.DevelopmentTables.Where(s => s.DevelopmentID == id).SingleOrDefault();


            if (data == null)
            {
                data = new DevelopmentTable();
                data.DevelopmentID = 0;
                data.DevelopmentCode = "";
                data.DevelopmentName = "";
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

        public string CheckDuplicateByDevCode(string _DevCode)
        {

            if (_DevCode != "")
            {

                var chkDevCode = DMPS.DevelopmentTables.Where(s => s.DevelopmentCode.ToUpper() == _DevCode.ToUpper()).ToList();
                return chkDevCode.ToObj2Json();
            }

            return "false";
        }

        public long SaveDeveloper(DevelopmentTable _Data)
        {
            var Data = DMPS.DevelopmentTables.Where(s => s.DevelopmentID == _Data.DevelopmentID).SingleOrDefault();

            if (Data == null)
            {
                Data = new DevelopmentTable();
                Data.DevelopmentCode = _Data.DevelopmentCode;
                Data.DevelopmentName = _Data.DevelopmentName;
                Data.Creator = _Data.Creator;
                Data.CreateDateTime = DateTime.Now;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.DevelopmentTables.Add(Data);

            }
            else
            {
                Data.DevelopmentCode = _Data.DevelopmentCode;
                Data.DevelopmentName = _Data.DevelopmentName;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.Entry(Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();

            return Data.DevelopmentID;
        }
    }
}