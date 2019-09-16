using EWS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWS.Web
{
    public class BaseController:Controller
    {
        public EWSUser GetCurrentUser()
        {
            if (Session["CurrentUser"] != null)
                return (EWSUser)Session["CurrentUser"];

            return null;
        }

        public int SideBarStatus
        {
            get {
                if (Session["SideBarStatus"] == null)
                    Session["SideBarStatus"] = 0;

                return ((int)Session["SideBarStatus"]);
            }
            set {
                Session["SideBarStatus"] = value;
            }
        }

        public void SetCurrentUser(EWSUser user)
        {
            Session["CurrentUser"] = user;
        }

        public ActionResult Script(string script)
        {
            string returnScript = "<script language='javascript' type='text/javascript'>";
            returnScript += script;
            returnScript += "</script>";
            return Content(returnScript);
        }

        public EWSList CurrentList {
            get {
                return ((EWSList)Session["CurrentList"]);
            } set {
                Session["CurrentList"] = value;
            }  }

        public int CurrentSequence
        {
            get
            {
                return ((int)Session["CurrentSequence"]);
            }
            set
            {
                Session["CurrentSequence"] = value;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Session["Error"] = filterContext.Exception.StackTrace.ToString();

            WriteEventLog(filterContext);

            if (Request.IsAjaxRequest())
            {
                filterContext.Result = new PartialViewResult
                {
                    ViewName = "~/Views/Shared/PartialError.cshtml"
                };
                //throw new Exception("Hata oluştu");
            }
            else
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml"
                };
        }

        public void WriteEventLog(ExceptionContext ex)
        {
            if (!EventLog.SourceExists("EWS"))
            {
                EventLog.CreateEventSource("EWS", "EWS");
            }
            EventLog eventLog = new EventLog();

            eventLog.Source = "EWS";

            string hata = ex.Exception.Message;
            hata += ex.Exception.StackTrace.ToString();

            eventLog.WriteEntry(hata, EventLogEntryType.Error, 1000);
        }

    }
}