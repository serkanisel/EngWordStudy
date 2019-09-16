using EWS.Business;
using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWS.Web.Helpers;
using System.Speech.Synthesis;
using EWS.Business.Helpers;

namespace EWS.Web.Controllers
{
    [AuthenticationAction]
    public class EWSWordsController : BaseController
    {
        // GET: EWSWords
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewWord()
        {
            ViewBag.Title = "New Word";


            return PartialView();
        }

        [HttpPost]
        public ActionResult NewWord(string wordbody, string description)
        {
            IWord iword = new BsWord();
            ViewBag.Title = "New Word";

            //kelime var mı kontrol et.varsa ekleme. 
            EWSWord word = iword.GetWordByBody(wordbody);

            if (word != null)
                return Script("Message('Bu Kelime Zaten Var.','info');");

            EWSWord wrd = new EWSWord()
            {
                UN = new Guid(),
                Description = description,
                WordBody = wordbody,
                UserID = GetCurrentUser().ID,
            };

            //EWSSampleSentence sentence = new EWSSampleSentence()
            //{
            //    Sentence = samplesentence,
            //    WordUN=wrd.UN,
            //    UserID=CurrentUser.ID,
            //    SentenceMean=samplesentenceMean,
            //};

            //wrd.EWSSampleSentence.Add(sentence);



            wrd = iword.WordSave(wrd);

            return Script("Message('Kelime Eklendi','success');");
        }

        [HttpPost]
        public JsonResult GetWordByListID(Guid listID, int rownumber, string type)
        {
            IWord iword = new BsWord();

            EWSWord w = iword.GetWordByEWSListWordID(listID, rownumber, type, GetCurrentUser().ID);

            return Json(w, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveComment(Guid wordID, string comment, string commentMean)
        {
            IWord iword = new BsWord();
            EWSSampleSentence ewsSamp = new EWSSampleSentence()
            {
                Sentence = comment,
                UN = Guid.NewGuid(),
                UserID = GetCurrentUser().ID,
                WordUN = wordID,
                SentenceMean = commentMean,
            };

            iword.SaveComment(ewsSamp);
            return GetWordSamples(wordID);
        }

        [HttpPost]
        public ActionResult GetWordSamples(Guid wordID)
        {
            IWord iword = new BsWord();
            List<EWSSampleSentence> samps = new List<EWSSampleSentence>();

            samps = iword.GetSampleSentences(wordID, GetCurrentUser().ID);

            return PartialView("WordSamples", samps);

        }

        [HttpPost]
        public ActionResult DeleteComment(Guid ID)
        {
            IWord iword = new BsWord();
            iword.DeleteComment(ID);

            return Content("");
        }

        [HttpPost]
        public ActionResult GetWordList(Guid ListID)
        {
            IEWSList ilist = new BsEWSList();
            EWSList list = ilist.GetList(ListID, GetCurrentUser().ID);

            return PartialView("WordGrid", list.EWSListWord.ToList());
        }

        [HttpPost]
        public ActionResult SearchWord(string wordBody)
        {
            IWord iword = new BsWord();

            List<EWSWord> words = iword.GetWords(wordBody);
            return PartialView(words);
        }

        [HttpPost]
        public ActionResult AddWordToList(Guid listUN, Guid wordUN)
        {
            IWord iword = new BsWord();

            iword.AddWordToList(listUN, wordUN);

            return Content("");
        }

        [HttpPost]
        public ActionResult GetWordDetailByUN(Guid UN)
        {
            IWord iword = new BsWord();

            EWSWord word = iword.GetWordById(UN, GetCurrentUser().ID);

            if (word.Description == null)
            {
                Translator t = new Translator();
                string TranslateText = word.WordBody.Trim();
                string result = t.Translate(TranslateText, "English", "Turkish");

                word.Description = result;
                //word = iword.WordSave(word);
            }
            return PartialView("WordDetail", word);
        }

        [HttpPost]
        public ActionResult TranslateText(string TranslateText)
        {
            IWord iword = new BsWord();
            Translator t = new Translator();
            TranslateText = TranslateText.Trim();
            TranslateText = TranslateText.Replace("\n", "");
            TranslateText = TranslateText.Replace("\r", "");

            string result = t.Translate(TranslateText, "English", "Turkish");


            //varsa kelimeyi getir. Yoksa yeni yaratıp gönder. 
            EWSWord word = iword.GetWordByBody(TranslateText);
            if (word != null)
            {
                //word.Description = result;
                return PartialView("WordDetail", word);
            }
            EWSWord w = new EWSWord()
            {
                Description = result,
                WordBody = TranslateText,
            };

            return PartialView("WordDetail", w);
        }

        [HttpPost]
        public ActionResult WordSound(string wordBody)
        {
            //    SpeechSynthesizer synth = new SpeechSynthesizer();

            //    // Configure the audio output. 
            //    synth.SetOutputToDefaultAudioDevice();

            //    // Speak a string.
            //    synth.Speak(wordBody);
            //    return Content("");

            // creating the object of SpeechSynthesizer class
            SpeechSynthesizer synth = new SpeechSynthesizer();

            synth.Volume = 100;
            //passing text box text to SpeakAsync method 
            synth.SpeakAsync(wordBody);
            return Content("");
        }

        public ActionResult GetWordByUN(Guid UN)
        {
            IWord iword = new BsWord();
            EWSWord w = iword.GetWordById(UN, GetCurrentUser().ID, false);


            return PartialView("Word", w);
        }

        [HttpPost]
        public ActionResult GlobalSearchWord(string wordBody)
        {
            IWord iword = new BsWord();

            List<EWSWord> words = iword.GetWords(wordBody);

            return PartialView(words);
        }

        [HttpPost]
        public ActionResult GetEWSList(Guid wordUN)
        {
            IWord iword = new BsWord();

            List<EWSList> result = iword.GetWordListByWordUN(wordUN, GetCurrentUser().ID);
            ViewBag.WordUN = wordUN;

            return PartialView("EWSList", result);
        }

        [HttpPost]
        public ActionResult GetEWSListGrid(Guid wordUN)
        {
            IWord iword = new BsWord();

            List<EWSList> result = iword.GetWordListByWordUN(wordUN, GetCurrentUser().ID);

            return PartialView("ListGrid", result);
        }

        [HttpPost]
        public ActionResult NewWordAsListMember(string wordbody, string description, Guid ListUN)
        {
            IWord iword = new BsWord();
            ViewBag.Title = "New Word";

            //kelime var mı kontrol et.varsa ekleme. 
            EWSWord word = iword.GetWordByBody(wordbody);

            if (word != null)
                return Script("Message('Bu Kelime Zaten Var.','info');");

            EWSWord wrd = new EWSWord()
            {
                UN = new Guid(),
                Description = description,
                WordBody = wordbody,
                UserID = GetCurrentUser().ID,
            };

            EWSListWord listWord = new EWSListWord()
            {
                UN = Guid.NewGuid(),
                isPublic = false,
                ListUN = ListUN,
                WordUN = wrd.UN
            };

            iword.WordSaveAsListMember(wrd, listWord);

            return Script("Message('Kelime Eklendi','success');");
        }

        [HttpPost]
        public ActionResult NewSentenceAsListMember(string body, string mean, Guid ListUN)
        {
            IWord iword = new BsWord();
            ViewBag.Title = "New Word";

            //kelime var mı kontrol et.varsa ekleme. 
            EWSSampleSentence sent = iword.GetSampleSentenceByBody(body);

            if (sent != null)
                return Script("Message('Bu cümle zaten ekli.','info');");

            EWSSampleSentence sentence = new EWSSampleSentence()
            {
                UN = Guid.NewGuid(),
                Sentence = body,
                SentenceMean = mean,
                UserID = GetCurrentUser().ID,
                ListUN = ListUN,
            };

            iword.SaveSampleSentence(sentence);

            return Script("Message('Cümle Eklendi','success');");
        }

        [HttpPost]
        public ActionResult NewSentence(string body, string mean, Guid wordUN)
        {
            IWord iword = new BsWord();

            //kelime var mı kontrol et.varsa ekleme. 
            EWSSampleSentence sent = iword.GetSampleSentenceByBody(body);

            if (sent != null)
                return Script("Message('Bu cümle zaten ekli.','info');");

            EWSSampleSentence sentence = new EWSSampleSentence()
            {
                UN = Guid.NewGuid(),
                Sentence = body,
                SentenceMean = mean,
                UserID = GetCurrentUser().ID,
                WordUN = wordUN,
            };

            iword.SaveSampleSentence(sentence);

            return Script("Message('Cümle Eklendi','success');");
        }

        [HttpPost]
        public ActionResult GetSentencesByWordUN(Guid wordUN)
        {
            IWord iword = new BsWord();
            List<EWSSampleSentence> samps = new List<EWSSampleSentence>();

            samps = iword.GetSampleSentences(wordUN, GetCurrentUser().ID);

            return PartialView("SentenceGrid", samps);

        }

        [HttpPost]
        public ActionResult GetEkledigimKelimeler()
        {
            IWord iword = new BsWord();

            List<EWSWord> words = iword.GetWordsByUserID(GetCurrentUser().ID, (int)AddType.extension.GetHashCode());
            return PartialView("MyAddedWords", words);
        }

        public ActionResult GetWillLearnWords(bool? partial = false)
        {
            IWord iword = new BsWord();

            List<EWSWillLearn> model = iword.GetWillLearnWords(GetCurrentUser().ID);
            if (partial == false)
                return View("WillLearnWords", model);
            else
                return PartialView("WillLearnWords", model);
        }

        public ActionResult GetIKnownWords(bool? partial = false)
        {
            IWord iword = new BsWord();

            List<EWSKnownWords> model = iword.GetKnownWords(GetCurrentUser().ID);
            if (partial == false)
                return View("KnownWords", model);
            else
                return PartialView("KnownWords", model);
        }

        [HttpPost]
        public ActionResult UpdateWord(Guid UN,string wordbody, string description)
        {
            IWord iword = new BsWord();

            EWSWord wrd = new EWSWord()
            {
                UN = UN,
                Description = description,
                //WordBody = wordbody,
            };

            wrd = iword.WordSave(wrd);

            return Script("Message('Kelime Güncellendi','success');");
        }
    }
}