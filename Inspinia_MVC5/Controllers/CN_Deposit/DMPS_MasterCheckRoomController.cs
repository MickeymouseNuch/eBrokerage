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
    public class DMPS_MasterCheckRoomController : Controller
    {
        // GET: DMPS_MasterCheckRoom
        PMdbEntities1 DMPS = new PMdbEntities1();
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadDataListView(string _SearcheName, long _id)
        {
            var qdata = DMPS.CheckRoomMasterTables.Where(s => s.CheckRoomID != 0);

            //if (StartDate != null) { qdata = qdata.Where(s => s.CreateDateTime >= StartDate && s.CreateDateTime <= StartDate); }

            if (_id != 0) { qdata = qdata.Where(s => s.CheckRoomID == _id);}

            if (_SearcheName != "")
            {
                _SearcheName = _SearcheName.ToUpper();
                qdata = qdata.Where(s => (s.CheckRoomDesc.ToUpper()).Contains(_SearcheName));
            }

            ViewBag.lstData = qdata.OrderBy(s => s.IsDelete).ThenBy(s => s.CheckRoomDesc).ToList();


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

            var data = DMPS.CheckRoomMasterTables.Where(s => s.CheckRoomID == id).SingleOrDefault();

           
            if (data == null)
            {
                data = new CheckRoomMasterTable();
                data.CheckRoomID = 0;
                data.CheckRoomDesc = "";
                data.CheckRoomUnit = "";
                data.IsQty = false;
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

        public long SaveCheckRoomMaster(CheckRoomMasterTable _Data)
        {
            var Data = DMPS.CheckRoomMasterTables.Where(s => s.CheckRoomID == _Data.CheckRoomID).SingleOrDefault();

            if (Data == null)
            {
                Data = new CheckRoomMasterTable();
                Data.CheckRoomDesc = _Data.CheckRoomDesc;
                Data.IsQty = _Data.IsQty;
                Data.CheckRoomUnit = _Data.CheckRoomUnit;
                Data.Creator = _Data.Creator;
                Data.CreateDateTime = DateTime.Now;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.CheckRoomMasterTables.Add(Data);

            }
            else
            {
                Data.CheckRoomDesc = _Data.CheckRoomDesc;
                Data.IsQty = _Data.IsQty;
                Data.CheckRoomUnit = _Data.CheckRoomUnit;
                Data.Reviser = _Data.Reviser;
                Data.ReviseDateTime = DateTime.Now;
                Data.IsDelete = _Data.IsDelete;
                DMPS.Entry(Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();

            return Data.CheckRoomID;
        }

        public string CheckDuplicate(string _ChkDup)
        {

            if (_ChkDup != "")
            {

                var ChkDup = DMPS.CheckRoomMasterTables.Where(s => s.CheckRoomDesc.ToUpper() == _ChkDup.ToUpper()).ToList();
                return ChkDup.ToObj2Json();
            }

            return "false";
        }

    }
}