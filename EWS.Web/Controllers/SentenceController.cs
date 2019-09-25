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
    public class SentenceController : BaseController
    {
        // GET: Sentence
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Random()
        {

            return PartialView("Random");
        }

        public ActionResult Liste()
        {
            ISentence iSentence = new BSSentence();

            List<EWSSampleSentence> sentence = iSentence.GetSentences();

            return PartialView(sentence);
        }

        [HttpPost]
        public ActionResult NewSentence(string sentenceBody, string sentenceMean)
        {
            ISentence isentence = new BSSentence();
            ViewBag.Title = "New Word";

            EWSSampleSentence sentence = new EWSSampleSentence()
            {
                UN = Guid.NewGuid(),
                Sentence= sentenceBody,
                SentenceMean = sentenceMean,
                UserID = GetCurrentUser().ID,
            };

            sentence = isentence.SaveSentence(sentence);

            return Script("Message('Cümle Kayıt Edildi.','success');");
        }

        [HttpPost]
        public ActionResult UpdateSentence(Guid UN,string sentenceBody, string sentenceMean)
        {
            ISentence isentence = new BSSentence();

            EWSSampleSentence sentence = new EWSSampleSentence()
            {
                UN = UN,
                Sentence = sentenceBody,
                SentenceMean = sentenceMean,
                UserID = GetCurrentUser().ID,
            };

            isentence.UpdateSentence(sentence);

            return Script("Message('Cümle Kayıt Edildi.','success');");
        }

        [HttpPost]
        public ActionResult SearchSentence(string searchSentence)
        {
            ISentence isentence = new BSSentence();

            List<EWSSampleSentence> model = isentence.SearchSentence(searchSentence);

            return PartialView("SampleList", model);
        }

        [HttpPost]
        public ActionResult DeleteRelationSentenceAndWord(Guid UN)
        {
            ISentence isentence = new BSSentence();

            EWSSampleSentence sentence = isentence.GetSentenceById(UN);

            sentence.WordUN = null;

            isentence.DeleteReletaionSentenceWord(sentence);

            return Script("Message('Kelime ile cümle ilişki kaldırıldı','success');");
        }
      
    }
}