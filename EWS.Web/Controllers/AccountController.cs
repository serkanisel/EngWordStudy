using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Model;
using EWS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EWS.Web.Controllers
{
    [AuthenticationAction]
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                IUser iuser = new BsUser();
                password = Helper.SHA1HashStringForUTF8String(password);
                EWSUser ouser = iuser.Login(username, password);

                if (ouser != null)
                {
                    SetCurrentUser(ouser);
                    FormsAuthentication.SetAuthCookie(username, false);
                    return Content("OK");
                }
                else //login işlemi başarısız                
                    return Content("Kullanıcı adı ve/veya şifre hatalı");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string namesurname)
        {
            IUser iuser = new BsUser();
            password = Helper.SHA1HashStringForUTF8String(password);

            EWSUser ouser = iuser.Register(username, password, namesurname);

            return Content("");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CheckSession()
        {
            if (GetCurrentUser() != null)
                return Content("OK");
            else
                return Content("");
        }
    }
}