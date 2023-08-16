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
    public class DMPS_MasterFacilityController : Controller
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
            var qdata = DMPS.FacilityMasterTables.Where(s => s.FacilityID != 0);

            //if (StartDate != null) { qdata = qdata.Where(s => s.CreateDateTime >= StartDate && s.CreateDateTime <= StartDate); }

            if (_id != 0) { qdata = qdata.Where(s => s.FacilityID == _id); }

            if (_SearcheName != "")
            {
                _SearcheName = _SearcheName.ToUpper();
                qdata = qdata.Where(s => (s.FacilityDesc.ToUpper()).Contains(_SearcheName));
            }


            ViewBag.lstData = qdata.OrderBy(s => s.IsDelete).ThenBy(s => s.FacilityDesc).ToList();
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
            var Creater = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();
            var Reviser = new Inspinia_MVC5.Models.STG_EMPLOYEEVw();

            var data = DMPS.FacilityMasterTables.Where(s => s.FacilityID == id).SingleOrDefault();


            if (data == null)
            {
                data = new FacilityMasterTable();
                data.FacilityID = 0;
                data.FacilityDesc = "";
                data.FacilityUnit = "";
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

        public long SaveFacility(FacilityMasterTable _Data)
        {
            var Data = DMPS.FacilityMasterTables.Where(s => s.FacilityID == _Data.FacilityID).SingleOrDefault();

            if (Data == null)
            {
                Data = new FacilityMasterTable();
                Data.FacilityDesc = _Data.FacilityDesc;
                Data.FacilityUnit = _Data.FacilityUnit;
                Data.Creator = _Data.Creator;
                Data.CreateDateTime = DateTime.Now;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.FacilityMasterTables.Add(Data);

            }
            else
            {
                Data.FacilityDesc = _Data.FacilityDesc;
                Data.FacilityUnit = _Data.FacilityUnit;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.Entry(Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();

            return Data.FacilityID;
        }
        public string CheckDuplicate(string _ChkDup)
        {

            if (_ChkDup != "")
            {

                var ChkDup = DMPS.FacilityMasterTables.Where(s => s.FacilityDesc.ToUpper() == _ChkDup.ToUpper()).ToList();
                return ChkDup.ToObj2Json();
            }

            return "false";
        }
    }
}