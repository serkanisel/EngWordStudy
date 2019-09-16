using EWS.Business;
using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Business.Helpers;
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

    public class EWSActionsController : BaseController
    {
        // GET: EWSActions
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TranslateTextJson(string TranslateText)
        {
            IWord iword = new BsWord();
            Translator t = new Translator();
            TranslateText = TranslateText.Trim();
            TranslateText = TranslateText.Replace("\n", "");
            TranslateText = TranslateText.Replace("\r", "");

            string result = t.Translate(TranslateText, "English", "Turkish");

            EWSWord w = new EWSWord();

            //varsa kelimeyi getir. Yoksa yeni yaratıp gönder. 
            EWSWord word = iword.GetWordByBody(TranslateText);
            if (word != null)
            {
                w.UN = word.UN;
                w.WordBody = word.WordBody;
                w.Description = result;
            }
            else
            {
                w.Description = result;
                w.WordBody = TranslateText;
            }

            return Json(w, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult WordSave(string wordbody, string description)
        {
            IWord iword = new BsWord();
            ViewBag.Title = "New Word";

            //kelime var mı kontrol et.varsa ekleme. 
            EWSWord wrd = iword.GetWordByBody(wordbody);

            if (wrd != null)
            {
                wrd.WordBody = wordbody;
                wrd.Description = description;

                ViewBag.Message = "Kelime zaten kayıtlıydı Güncellendi.";
                ViewBag.MessageType = "info";

                return PartialView();
            }

            if (wrd == null)
                wrd = new EWSWord()
                {
                    UN = new Guid(),
                    Description = description,
                    WordBody = wordbody,
                    UserID = GetCurrentUser().ID,
                    AddType = 2
                };

            iword.WordSave(wrd);
            ViewBag.Message = "Kelime Kayıt Edildi";
            ViewBag.MessageType = "success";

            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveSentence(string sentenceBody, string sentenceMean)
        {

            ISentence isentence = new BSSentence();
            ViewBag.Title = "New Sentence";

            EWSSampleSentence sentence = isentence.GetSentenceByBody(sentenceBody);

            if (sentence != null)
            {
                sentence.SentenceMean = sentenceBody;
                sentence.SentenceMean = sentenceMean;
                isentence.SaveSentence(sentence);

                ViewBag.Message = "Cümle zaten vardı güncellendi.";
                ViewBag.MessageType = "info";

                return PartialView("WordSave");
            }

            if (sentence == null)
                sentence = new EWSSampleSentence()
                {
                    UN = Guid.NewGuid(),
                    Sentence = sentenceBody,
                    SentenceMean = sentenceMean,
                    UserID = GetCurrentUser().ID,
                };

            isentence.SaveSentence(sentence);

            ViewBag.Message = "Cümle Kayıt Edildi";
            ViewBag.MessageType = "success";

            return PartialView("WordSave");
        }

        [HttpPost]
        public ActionResult ScriptReturn()
        {
            ViewBag.Message = "dana veli ne yaptın hacim";
            ViewBag.MessageType = "error";
            return PartialView("WordSave");
        }

        [HttpPost]
        public JsonResult CheckSession()
        {
            bool result = false;

            try
            {
                if (Session["CurrentUser"] != null)
                    result = true;

            }
            catch (Exception)
            {
                return Json(result);
            }

            return Json(result);
        }

        //[HttpPost]
        //public JsonResult Login(string loginName,string pass)
        //{
        //    try
        //    {

        //        IUser iuser = new BsUser();
        //        pass = Helper.SHA1HashStringForUTF8String(pass);
        //        EWSUser ouser = iuser.Login(loginName, pass);

        //        if (ouser != null)
        //        {
        //            SetCurrentUser(ouser);
        //            FormsAuthentication.SetAuthCookie(loginName, false);
        //            return Json(true);
        //        }
        //        else //login işlemi başarısız                
        //            return Json(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}