using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWS.Web.Controllers
{
    public class LookupController : Controller
    {
        // GET: Lookup
        public JsonResult GetEWSLists(int userID)
        {
            IEWSList iList = new BsEWSList();

            List<EWSList> result = iList.GetUserListByUserID(userID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEWSReadingPart(int userID)
        {
            IEWSReadingPart iReadinPart = new BSReadingPart();

            List<EWSReadPart> result = iReadinPart.GetReadingPartsByUserID(userID,false);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEWSSampleSenteceGroup(int userID)
        {
            ISentence iList = new BSSentence();

            List<EWSSentenceGroup> result = iList.EWSSentenceGroups(userID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}