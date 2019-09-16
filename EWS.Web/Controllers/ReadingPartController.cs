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
    [AuthenticationAction]
    public class ReadingPartController : BaseController
    {
        // GET: ReadingPart
        public ActionResult Index()
        {
            IEWSReadingPart iReadPart = new BSReadingPart();

            List<EWSReadPart> model = iReadPart.GetReadingPartsByUserID(GetCurrentUser().ID);
            
            return View(model);
        }

        public ActionResult SelectReadingPart(Guid UN)
        {
            IEWSReadingPart iReadPart = new BSReadingPart();

            EWSReadPart readPart= iReadPart.GetReadingPart(UN);

            if (readPart.ReadPartHtml == null)
                readPart.ReadPartHtml = readPart.ReadPart;

            return View("ReadingPart",readPart);
        }

        public ActionResult ReadingPartSentences(Guid listUN,string name)
        {
            IEWSReadingPart iReadPart = new BSReadingPart();

            List<EWSSampleSentence> sentences = iReadPart.GetReadingPartSentences(listUN);

            ViewBag.Title = "Okuma Parçası Cümleleri";
            ViewBag.ReadPartName = name;
            return View("ReadPartSentence", sentences);
        }
        

        [HttpPost]
        public JsonResult GetReadinPartByUNJson(Guid UN)
        {
            IEWSReadingPart iReadPart = new BSReadingPart();

            EWSReadPart readPart = iReadPart.GetReadingPart(UN,false);

            return Json(readPart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReadingPartList()
        {
            IEWSReadingPart iReadPart = new BSReadingPart();

            List<EWSReadPart> model = iReadPart.GetReadingPartsByUserID(GetCurrentUser().ID);

            return PartialView("ReadPartGrid", model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NewReadingPart(string name,string body,string html)
        {
            IEWSList iList = new BsEWSList();

            iList.SaveReadPart(name, GetCurrentUser().ID, body,html);

            return Script("Message('Okuma Parçası Kayıt Edildi.','success');");
        }

    }
}