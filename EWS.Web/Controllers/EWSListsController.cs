using EWS.Business;
using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Business.Helpers;
using EWS.Model;
using EWS.Web.Helpers;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWS.Web.Controllers
{
    [AuthenticationAction]
    public class EWSListsController : BaseController
    {
        // GET: EWSLists
        public ActionResult Index()
        {
            ViewBag.Title = "Public Library";
            List<EWSList> lists = new List<EWSList>();

            IEWSList ilist = new BsEWSList();

            lists = ilist.GetAllList(GetCurrentUser().ID);

            lists = lists.Where(p => p.EWSReadPart.Count() == 0).ToList();
            ViewBag.UserID = GetCurrentUser().ID;

            //return View(lists.Where(p => p.EWSListWord.Count > 0).ToList());
            return View(lists);
        }
        public ActionResult NewList()
        {
            ViewBag.Title = "New List";
            return PartialView();
        }

        [HttpGet]
        public ActionResult ListDetail(Guid listID)
        {
            IEWSList ilist = new BsEWSList();

            EWSList list = ilist.GetList(listID, GetCurrentUser().ID);

            return View(list);
        }

        [HttpPost]
        public ActionResult Preview(Guid listID, string type)
        {
            IEWSList ilist = new BsEWSList();

            EWSList list = ilist.GetList(listID, GetCurrentUser().ID);
            ViewBag.ListID = listID;
            ViewBag.type = type;

            return PartialView("Onebyone", list);
        }

        [HttpGet]
        public ActionResult Preview(Guid listID)
        {
            IEWSList ilist = new BsEWSList();

            EWSList list = ilist.GetList(listID, GetCurrentUser().ID, true);
            ViewBag.ListID = listID;

            CurrentList = list;
            CurrentSequence = 0;
            return View("EWSListDetail", list);
        }
        
        [HttpPost]
        public ActionResult SetIKnow(Guid wordID)
        {
            IEWSList ilist = new BsEWSList();
            IWord iword = new BsWord();

            ilist.SetIKnowWord(wordID, GetCurrentUser().ID);

            EWSWord word = iword.GetWordById(wordID, GetCurrentUser().ID);

            if (word.Description == null)
            {
                Translator t = new Translator();
                string TranslateText = word.WordBody.Trim();
                string result = t.Translate(TranslateText, "English", "Turkish");

                word.Description = result;
                //word = iword.WordSave(word);
            }

            ViewBag.FromDetail = true;
            return PartialView("../EWSWords/Word", word);
        }

        [HttpPost]
        public ActionResult RemoveIKnow(Guid wordID)
        {
            IEWSList ilist = new BsEWSList();

            ilist.RemoveIKnow(wordID, GetCurrentUser().ID);

            IWord iword = new BsWord();
            EWSWord w = iword.GetWordById(wordID, GetCurrentUser().ID, false);

            ViewBag.FromDetail = true;
            return PartialView("../EWSWords/Word", w);
        }

        [HttpPost]
        public ActionResult SetIWillLearn(Guid wordID)
        {
            IEWSList ilist = new BsEWSList();
            IWord iword = new BsWord();

            ilist.SetILearn(wordID, GetCurrentUser().ID);

            EWSWord word = iword.GetWordById(wordID, GetCurrentUser().ID);

            if (word.Description == null)
            {
                Translator t = new Translator();
                string TranslateText = word.WordBody.Trim();
                string result = t.Translate(TranslateText, "English", "Turkish");

                word.Description = result;
                //word = iword.WordSave(word);
            }

            ViewBag.FromDetail = true;
            return PartialView("../EWSWords/Word", word);
        }

        [HttpPost]
        public ActionResult RemoveIWillLearn(Guid wordID)
        {
            IEWSList ilist = new BsEWSList();

            ilist.RemoveIWillLearn(wordID, GetCurrentUser().ID);

            IWord iword = new BsWord();
            EWSWord w = iword.GetWordById(wordID, GetCurrentUser().ID, false);

            ViewBag.FromDetail = true;
            return PartialView("../EWSWords/Word", w);
        }

        [HttpPost]
        public ActionResult GetMultiple(Guid listID, string type, string showType = "multiple")
        {
            IEWSList ilist = new BsEWSList();

            ViewBag.ListID = listID;
            ViewBag.showType = showType;
            ViewBag.type = type;
            EWSList list = ilist.GetList(listID, GetCurrentUser().ID);

            return PartialView("Multiple", list);
        }

        [HttpPost]
        public ActionResult GetMultipleList(Guid listID, string type)
        {
            IEWSList ilist = new BsEWSList();

            ViewBag.ListID = listID;
            ViewBag.type = type;
            EWSList list = ilist.GetList(listID, GetCurrentUser().ID);

            return PartialView(list);
        }

        [HttpGet]
        public ActionResult listIWillLearn()
        {
            IEWSList ilist = new BsEWSList();

            List<EWSLearnList> list = ilist.GetListIWillLearn(GetCurrentUser().ID);
            ViewBag.Title = "Öğrenmek İstediğim Listeler";
            return View(list);
        }

        public ActionResult MyLists(int userID = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOgrenilecekListeler(Guid ListUN)
        {
            IEWSList ilist = new BsEWSList();
            EWSLearnList list = new EWSLearnList()
            {
                ListUN = ListUN,
                UN = Guid.NewGuid(),
                UserID = GetCurrentUser().ID,
            };

            ilist.OgrenilecekListeEkle(list);
            return Script("Message('Liste Öğrenilecek Listelere Eklendi.','success')");
        }

        [HttpPost]
        public ActionResult CikarOgrenilecekListeler(Guid ListUN)
        {
            IEWSList ilist = new BsEWSList();
            EWSLearnList list = new EWSLearnList()
            {
                ListUN = ListUN,
                UN = Guid.NewGuid(),
                UserID = GetCurrentUser().ID,
            };

            ilist.OgrenilecekListeCikar(list);
            return Script("Message('Liste Öğrenilecek Listelerden Çıkarıldı.','success')");
        }

        [HttpPost]
        public ActionResult SaveNewList(string listName)
        {
            IEWSList ilist = new BsEWSList();
            EWSList ewsList = new EWSList()
            {
                Name = listName,
                UserID = GetCurrentUser().ID,
                UN = Guid.NewGuid(),
            };

            ewsList = ilist.SaveNewList(ewsList);
            return Content("");
        }

        [HttpPost]
        public ActionResult GetUserLists()
        {
            IEWSList ilist = new BsEWSList();

            List<EWSList> result = ilist.GetUserListByUserID(GetCurrentUser().ID);

            result = result.Where(p => p.EWSReadPart.Count() == 0).ToList();
            return PartialView("EWSListGrid", result);
        }

        [HttpGet]
        public ActionResult AddWordToList(Guid listID)
        {
            EWSList list = new EWSList();
            IEWSList ilist = new BsEWSList();
            list = ilist.GetList(listID, GetCurrentUser().ID);

            return View(list);
        }

        [HttpPost]
        public ActionResult DeleteList(Guid listID)
        {
            EWSList list = new EWSList();
            IEWSList ilist = new BsEWSList();
            ilist.DeleteList(listID);

            return Content("");
        }

        [HttpPost]
        public ActionResult ListedenCikar(Guid UN)
        {
            IEWSList ilist = new BsEWSList();

            ilist.ListedenCikar(UN);

            return Content("");
        }

        [HttpPost]
        public JsonResult GetListWordBySequenceNo(string leftright)
        {
            try
            {

                IWord iword = new BsWord();

                if (leftright == "")
                    CurrentSequence = 1;

                if (leftright == "left")
                    CurrentSequence--;
                else if (leftright == "right")
                    CurrentSequence++;

                if (CurrentSequence <= 0)
                {
                    CurrentSequence++; // eski değerine getir
                    return null;
                }

                if (CurrentSequence > CurrentList.EWSListWord.Count())
                {
                    CurrentSequence--; // eski değerine getir
                    return null;
                }

                EWSWord word = iword.GetWordById(((Guid)CurrentList.EWSListWord.Where(p => p.Number == CurrentSequence).FirstOrDefault().WordUN), GetCurrentUser().ID, false);

                if (word != null)
                {
                    if (word.Description == null)
                    {
                        Translator t = new Translator();
                        string TranslateText = word.WordBody.Trim();
                        string result = t.Translate(TranslateText, "English", "Turkish");

                        word.Description = result;
                        word = iword.WordSave(word);
                    }
                    word.CurrentSequence = CurrentSequence;
                }
                return Json(word, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SelectedList(Guid UN)
        {
            IEWSList ilist = new BsEWSList();

            CurrentList = ilist.GetList(UN, GetCurrentUser().ID, true);
            CurrentSequence = 0;

            return Content(CurrentList.EWSListWord.Count().ToString());
        }

        [HttpPost]
        public ActionResult PreviewMultiple(Guid UN)
        {
            IEWSList ilist = new BsEWSList();
            IWord iword = new BsWord();

            CurrentList = ilist.GetList(UN, GetCurrentUser().ID, true);

            foreach (var item in CurrentList.EWSListWord)
            {
                if (item.EWSWord.Description == null)
                {
                    EWSWord word = new EWSWord();
                    Translator t = new Translator();
                    string TranslateText = item.EWSWord.WordBody.Trim();
                    string result = t.Translate(TranslateText, "English", "Turkish");
                    word.UN = item.EWSWord.UN;
                    word.UserID = item.EWSWord.UserID;
                    word.Description = result;
                    word.WordBody = item.EWSWord.WordBody;

                    item.EWSWord.Description = result;
                    word = iword.WordSave(word);
                }
                item.EWSWord.CurrentSequence = CurrentSequence;
            }
            return PartialView(CurrentList);
        }

        [HttpPost]
        public ActionResult PreviewTable(Guid UN)
        {
            IEWSList ilist = new BsEWSList();

            CurrentList = ilist.GetList(UN, GetCurrentUser().ID, true);

            return PartialView(CurrentList);
        }

        [HttpPost]
        public ActionResult UploadListWithFile()
        {
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;

                string filename = Path.GetFileName(Request.Files[0].FileName);

                HttpPostedFileBase file = files[0];
                string fname;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                List<EWSWord> listOfWord = new List<EWSWord>();
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        ExcelWorksheet workSheet = currentSheet.First();

                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        string word;
                        string mean;
                        string sampleSentence;
                        string sampleSentenceMean;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            word = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                            mean = ((workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString());
                            sampleSentence = ((workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString());
                            sampleSentenceMean = ((workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString());

                            EWSWord w = new EWSWord();
                            w.WordBody = word.ToLowerInvariant();
                            w.Description = mean.ToLowerInvariant();
                            w.UserID = GetCurrentUser().ID;

                            if (sampleSentence != "")
                            {
                                EWSSampleSentence sentence = new EWSSampleSentence()
                                {
                                    UN = Guid.NewGuid(),
                                    UserID = GetCurrentUser().ID,
                                    Sentence = sampleSentence,
                                    SentenceMean = sampleSentenceMean,
                                };

                                w.EWSSampleSentence.Add(sentence);
                            }
                            listOfWord.Add(w);
                        } // for end
                    }//using end

                }//if end

                IWord iWord = new BsWord();

                iWord.SaveWordMultiple(listOfWord, fname,GetCurrentUser().ID);
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult GetSystemLists()
        {
            IEWSList ilist = new BsEWSList();

            List<EWSList> result = ilist.GetSystemList();

            result = result.Where(p => p.EWSReadPart.Count() == 0).ToList();

            return PartialView("EWSListGrid", result);
        }
    }
}