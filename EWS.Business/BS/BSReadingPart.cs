using EWS.Business.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWS.Model;
using EWS.Repository;
using System.Text.RegularExpressions;

namespace EWS.Business.BS
{
    public class BSReadingPart : IEWSReadingPart, IDisposable
    {
        public void Dispose()
        {
            this.Dispose();
        }

        public EWSReadPart GetReadingPart(Guid UN, bool getRelations = true)
        {
            IRepositoryBase<EWSReadPart> _rep = new RepositoryBase<EWSReadPart>(getRelations);
            IRepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>(true);

            EWSReadPart readingPart = _rep.Get(p => p.UN == UN);

            //string desen = @"([A-z])\w+";

            //var matches = Regex.Matches(readingPart.ReadPart, desen);
            //foreach (var item in matches)
            //{
            //    string w = item.ToString();
            //    // önce sorgula. varsa ekleme.yoksa ekle.
            //    EWSWord word = _repWord.Get(p => p.WordBody == w);

            //    if (word == null)
            //    {
            //        word = new EWSWord()
            //        {
            //            UN = Guid.NewGuid(),
            //            UserID = UserID,
            //            WordBody = item.ToString(),
            //        };

            //        Translator t = new Translator();
            //        string TranslateText = word.WordBody.Trim();
            //        string result = t.Translate(TranslateText, "English", "Turkish");

            //        word.Description = result;

            //        if (word.WordBody != word.Description)
            //            word = _repWord.Add(word);
            //    }

            //    //varsa ekleme 
            //    EWSListWord listWord = null;

            //    listWord = _repListWord.Get(p => p.ListUN == list.UN && p.WordUN == word.UN);
            //    // list word kaydet
            //    if (listWord == null)
            //    {
            //        listWord = new EWSListWord();
            //        listWord.ListUN = list.UN;
            //        listWord.UN = Guid.NewGuid();
            //        listWord.WordUN = word.UN;
            //        listWord.isPublic = false;

            //        _repListWord.Add(listWord);
            //    }
            //}

            //List<EWSListWord> listWords = _repListWord.GetList(p => p.ListUN == readingPart.ListUN);

            //foreach (var item in listWords)
            //{
            //    readingPart.ReadPart =readingPart.ReadPart.Replace(item.EWSWord.WordBody, "<a href='javascript:void(0)'>" + item.EWSWord.WordBody + "</a>");
            //}

            readingPart.ReadPart = readingPart.ReadPart.Replace("\n","<br \\>");
            return readingPart;
        }

        public List<EWSReadPart> GetReadingPartsByUserID(int userID, bool getRelations = true)
        {
            IRepositoryBase<EWSReadPart> _rep = new RepositoryBase<EWSReadPart>(false);
            IRepositoryBase<EWSKnownWords> _repKnownWord = new RepositoryBase<EWSKnownWords>(false);
            IRepositoryBase<EWSList> _repList = new RepositoryBase<EWSList>(false);

            List<EWSReadPart> result = _rep.GetList(p => p.UserID == userID);

            foreach (var item1 in result)
            {
                item1.EWSList = _repList.Get(p => p.UN == item1.ListUN);

                item1.EWSList.ListKnow = new List<Guid>();

                foreach (var item2 in item1.EWSList.EWSListWord)
                {
                    EWSKnownWords listWord = _repKnownWord.Get(p => p.WordUN == item2.WordUN);

                    if (listWord != null)
                        item1.EWSList.ListKnow.Add((Guid)listWord.WordUN);
                }
            }

            return result;
        }

        public List<EWSSampleSentence> GetReadingPartSentences(Guid listUN, bool getRelations = true)
        {
            RepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            List<EWSSampleSentence> result=_rep.GetList(p => p.ListUN==listUN);

            return result;
        }
    }
}
