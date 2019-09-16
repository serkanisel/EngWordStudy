using EWS.Business;
using EWS.Business.BS;
using EWS.Business.Contract;
using EWS.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EWS.Web.Controllers
{
    [AuthenticationAction]
    public class EWSFileController : BaseController
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadSozluk()
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

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            word = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                            mean = ((workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString()).ToUpper();

                            EWSWord w = new EWSWord();
                            w.WordBody = word.ToLowerInvariant();
                            w.Description = mean.ToLowerInvariant();

                            listOfWord.Add(w);
                        } // for end
                    }//using end

                }//if end

                IWord iWord = new BsWord();

                iWord.SaveWordMultiple(listOfWord,"",GetCurrentUser().ID);
            }

            return Script("Message('Sözlük Kayıt Edildi','success');");
        }

        public ActionResult AnalyzeFiles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnalyzeFile()
        {
            IEWSList iList = new BsEWSList();

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
                    StreamReader reader = new StreamReader(file.InputStream);

                    string m = reader.ReadToEnd();
                    
                    iList.SaveReadPart(filename, GetCurrentUser().ID, m,null);

                }//if end
            }

            return Script("Message('Dosya Analizi Tamamlandı','success');");
        }
    }
}