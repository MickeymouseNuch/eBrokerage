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
    public class DMPS_MasterUnitTypeController : Controller
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
            var qdata = DMPS.DMPS_UnitTypeTable.Where(s => s.ProjectID != 0);

            //if (StartDate != null) { qdata = qdata.Where(s => s.CreateDateTime >= StartDate && s.CreateDateTime <= StartDate); }

            if (_id != 0) { qdata = qdata.Where(s => s.ID == _id); }

            if (_SearcheName != "")
            {
                _SearcheName = _SearcheName.ToUpper();
                qdata = qdata.Where(s => (s.UnitType.ToUpper()).Contains(_SearcheName));
            }

            ViewBag.lstData = qdata.OrderBy(s => s.IsDelete).ThenBy(s => s.UnitType).ToList();

            return PartialView();
        }

        public ActionResult LoadModal()
        {

            //var lstDeveloper = (from t1 in DMPS.DevelopmentTables.Where(s => s.IsDelete == false) select new { DeveloperID = t1.DevelopmentID, DisplayName = t1.DevelopmentName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDeveloper = new SelectList(lstDeveloper, "DeveloperID", "DisplayName");

            return PartialView();
        }

        public string LoadData2Modal(long id)
        {
            string result = string.Empty;
            List<Object> obj = new List<object>();
            var Creater = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();
            var Reviser = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();

            var data = DMPS.DMPS_UnitTypeTable.Where(s => s.ID == id).SingleOrDefault();


            if (data == null)
            {
                data = new DMPS_UnitTypeTable();
                data.ID = 0;
                data.UnitType = "";
                data.ProjectID = 0;
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

        public long SaveUnitType(DMPS_UnitTypeTable _Data)
        {
            var Data = DMPS.DMPS_UnitTypeTable.Where(s => s.ID == _Data.ID).SingleOrDefault();

            if (Data == null)
            {
                Data = new DMPS_UnitTypeTable();
                Data.ProjectID = _Data.ProjectID;
                Data.UnitType = _Data.UnitType;
                Data.Creator = _Data.Creator;
                Data.CreateDateTime = DateTime.Now;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.DMPS_UnitTypeTable.Add(Data);

            }
            else
            {
                Data.ProjectID = _Data.ProjectID;
                Data.UnitType = _Data.UnitType;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.Entry(Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();

            return Data.ID;
        }

        public string CheckDuplicate(string _ChkDup)
        {

            if (_ChkDup != "")
            {

                var ChkDup = DMPS.DMPS_UnitTypeTable.Where(s => s.UnitType.ToUpper() == _ChkDup.ToUpper()).ToList();
                return ChkDup.ToObj2Json();
            }

            return "false";
        }

    }
}