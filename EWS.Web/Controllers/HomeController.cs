using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWS.Business;
using EWS.Model;
using EWS.Web.Helpers;


namespace EWS.Web.Controllers
{
    [AuthenticationAction]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Sections()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult SidebarStatus()
        {
            return Json(SideBarStatus,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SidebarStatusSet(int status)
        {
            SideBarStatus = status;
            return Json(SideBarStatus, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult SetShowMenu(bool menushow)
        {
            Session["MenuShow"]=menushow;

            return Json(menushow, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchGenel(string searchSentence)
        {
            IWord iword = new BsWord();

            List<EWSWord> words = iword.GetWords(searchSentence);
            return PartialView(words);
        }



    }
}