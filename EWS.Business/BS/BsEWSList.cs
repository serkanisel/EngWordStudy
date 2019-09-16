using EWS.Business.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWS.Model;
using EWS.Repository;
using System.IO;
using System.Text.RegularExpressions;
using EWS.Business.Helpers;

namespace EWS.Business.BS
{
    public class BsEWSList : IEWSList, IDisposable
    {
        public void Dispose()
        {
            this.Dispose();
        }
        public List<EWSList> GetAllList(int? userID)
        {
            IRepositoryBase<EWSList> _rep = new RepositoryBase<EWSList>();
            IRepositoryBase<EWSWillLearn> _repListWillLearn = new RepositoryBase<EWSWillLearn>();
            IRepositoryBase<EWSReadPart> _repReadinPart = new RepositoryBase<EWSReadPart>();

            IRepositoryBase<EWSKnownWords> _repKnownWord = new RepositoryBase<EWSKnownWords>();

            //IRepositoryBase<EWSLearnList> _repLearnList = new RepositoryBase<EWSLearnList>();
            short listState = (short)ListState.silindi.GetHashCode();
            List<EWSList> lists = _rep.GetList(p => (p.UserID == null || p.UserID == userID) && p.State != listState);


            List<EWSList> returnList = new List<EWSList>();
            foreach (var item in lists)
            {
                if (item.EWSReadPart != null && item.EWSReadPart.Count == 0)
                    returnList.Add(item);
            }

            foreach (var item1 in returnList)
            {
                item1.ListKnow = new List<Guid>();

                foreach (var item2 in item1.EWSListWord)
                {
                    EWSKnownWords listWord = _repKnownWord.Get(p => p.WordUN == item2.WordUN);

                    if (listWord != null)
                        item1.ListKnow.Add((Guid)listWord.WordUN);
                }
            }
            return returnList;
        }
        public EWSList GetList(Guid listID, int userID, bool numbers = false)
        {
            RepositoryBase<EWSList> _rep = new RepositoryBase<EWSList>();
            RepositoryBase<EWSWillLearn> _repListWillLearn = new RepositoryBase<EWSWillLearn>();

            EWSList list = new EWSList();
            list = _rep.Get(p => p.UN == listID);

            if (numbers)
            {
                int i = 1;
                foreach (var item in list.EWSListWord.OrderBy(p => p.EWSWord.WordBody))
                {
                    item.Number = i;
                    i++;

                    if (item.EWSWord.EWSKnownWords != null && item.EWSWord.EWSKnownWords.Count > 0)
                    {
                        if (item.EWSWord.EWSKnownWords.ToList()[0].UserID == userID)
                            item.EWSWord.Known = true;
                    }

                }
            }

            return list;
        }

        public void SetIKnowWord(Guid wordID, int userID)
        {
            RepositoryBase<EWSKnownWords> _rep = new RepositoryBase<EWSKnownWords>();
            RepositoryBase<EWSWillLearn> _repLearnList = new RepositoryBase<EWSWillLearn>();


            //sorgula yoksa ekle. 
            EWSKnownWords knownWord = _rep.Get(p => p.WordUN == wordID && p.UserID == userID);

            //varsa öğreneceklerim listesinden çıkar
            EWSWillLearn learnList = _repLearnList.Get(p => p.WordUN == wordID && p.UserID == userID);

            if (learnList != null)
            {
                _repLearnList.Remove(learnList);
            }

            if (knownWord != null)
                return;

            EWSKnownWords listKnow = new EWSKnownWords()
            {
                UN = Guid.NewGuid(),
                UserID = userID,
                WordUN = wordID
            };
            _rep.Add(listKnow);
        }

        public void RemoveIKnow(Guid wordID, int userID)
        {
            RepositoryBase<EWSKnownWords> _rep = new RepositoryBase<EWSKnownWords>();

            EWSKnownWords listKnow = _rep.Get(p => p.WordUN == wordID && p.UserID == userID);

            _rep.Remove(listKnow);
        }

        public void SetILearn(Guid wordID, int userID)
        {
            RepositoryBase<EWSWillLearn> _rep = new RepositoryBase<EWSWillLearn>();
            RepositoryBase<EWSKnownWords> _repKnown = new RepositoryBase<EWSKnownWords>();

            //varsa bildiklerim listesinden çıkar
            EWSKnownWords learnList = _repKnown.Get(p => p.WordUN == wordID && p.UserID == userID);

            if (learnList != null)
            {
                _repKnown.Remove(learnList);
            }

            //sorgula yoksa ekle. 
            EWSWillLearn willLearnWord = _rep.Get(p => p.WordUN == wordID && p.UserID == userID);

            if (willLearnWord != null)
                return;

            EWSWillLearn listLearn = new EWSWillLearn()
            {
                UN = Guid.NewGuid(),
                UserID = userID,
                WordUN = wordID
            };
            _rep.Add(listLearn);
        }

        public void RemoveIWillLearn(Guid wordID, int userID)
        {
            RepositoryBase<EWSWillLearn> _rep = new RepositoryBase<EWSWillLearn>();

            EWSWillLearn listILearn = _rep.Get(p => p.WordUN == wordID && p.UserID == userID);

            _rep.Remove(listILearn);
        }

        public List<EWSLearnList> GetListIWillLearn(int userID)
        {
            RepositoryBase<EWSLearnList> _rep = new RepositoryBase<EWSLearnList>();

            List<EWSLearnList> result = _rep.GetList(p => p.UserID == userID);
            return result;
        }

        public void OgrenilecekListeEkle(EWSLearnList ewsLearnList)
        {
            IRepositoryBase<EWSLearnList> _rep = new RepositoryBase<EWSLearnList>();

            _rep.Add(ewsLearnList);

        }

        public void OgrenilecekListeCikar(EWSLearnList ewsLearnList)
        {
            IRepositoryBase<EWSLearnList> _rep = new RepositoryBase<EWSLearnList>();

            ewsLearnList = _rep.Get(p => p.ListUN == ewsLearnList.ListUN && p.UserID == ewsLearnList.UserID);
            _rep.Add(ewsLearnList);

        }

        public List<EWSList> GetUserListByUserID(int userID, bool getBagliNesneler = true)
        {
            IRepositoryBase<EWSList> _rep = new RepositoryBase<EWSList>(getBagliNesneler);
            short listState = (short)ListState.silindi.GetHashCode();
            List<EWSList> lists = _rep.GetList(p => p.UserID == userID && p.State != listState);

            List<EWSList> returnList = new List<EWSList>();
            foreach (var item in lists)
            {
                if (item.EWSReadPart != null && item.EWSReadPart.Count == 0)
                {
                    EWSList ew = new EWSList()
                    {
                        Name = item.Name,
                        State = item.State,
                        UN = item.UN,
                        UserID = item.UserID,
                        isLearn = item.isLearn,
                        ListKnow = item.ListKnow,
                    };

                    returnList.Add(ew);
                }
            }
            return returnList;
        }

        public List<EWSList> GetSystemList(bool getBagliNesneler = true)
        {
            return GetAllList(null);
        }

        public EWSList SaveNewList(EWSList ewsList)
        {
            IRepositoryBase<EWSList> _rep = new RepositoryBase<EWSList>();

            return _rep.Add(ewsList);
        }

        public void DeleteList(Guid listID)
        {
            IRepositoryBase<EWSList> _rep = new RepositoryBase<EWSList>();

            EWSList list = _rep.Get(p => p.UN == listID);
            list.State = (short)ListState.silindi.GetHashCode();

            _rep.Update(list);
        }

        public void ListedenCikar(Guid UN)
        {
            IRepositoryBase<EWSListWord> _rep = new RepositoryBase<EWSListWord>();

            EWSListWord w = _rep.Get(p => p.UN == UN);

            _rep.Remove(w);
        }

        public void SaveReadPart(string fileName, int UserID, string fullText,string html)
        {
            IRepositoryBase<EWSReadPart> _repReadPart = new RepositoryBase<EWSReadPart>();
            IRepositoryBase<EWSList> _repList = new RepositoryBase<EWSList>();
            IRepositoryBase<EWSWord> _repWord = new RepositoryBase<EWSWord>();
            IRepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>();

            string textLine = string.Empty;
            string FullBody = string.Empty;
            string preFixFile = string.Empty;

            if (fileName.Contains("."))
            {
                string[] preFileName = fileName.Split('.');

                if (preFileName != null && preFileName.Count() > 0)
                    preFixFile = preFileName[0];
            }
            else
                preFixFile = fileName;

            //önce listeyi kaydet.
            EWSList list = new EWSList()
            {
                Name = preFixFile,
                UN = Guid.NewGuid(),
                UserID = UserID,
            };

            list = _repList.Add(list);

            //read part kaydet
            EWSReadPart readPart = new EWSReadPart()
            {
                UN = Guid.NewGuid(),
                ListUN = list.UN,
                UserID = UserID,
                ReadPart = fullText,
                Name = preFixFile,
                ReadPartHtml=html
            };

            readPart = _repReadPart.Add(readPart);

            string desen = @"([A-z])\w+";

            var matches = Regex.Matches(fullText, desen);
            foreach (var item in matches)
            {
                string w = item.ToString();
                // önce sorgula. varsa ekleme.yoksa ekle.
                EWSWord word = _repWord.Get(p => p.WordBody == w);

                if (word == null)
                {
                    word = new EWSWord()
                    {
                        UN = Guid.NewGuid(),
                        UserID = UserID,
                        WordBody = item.ToString(),
                    };

                    Translator t = new Translator();
                    string TranslateText = word.WordBody.Trim();
                    string result = t.Translate(TranslateText, "English", "Turkish");

                    word.Description = result;

                    //if (word.WordBody != word.Description)
                    word = _repWord.Add(word);
                }

                //varsa ekleme 
                EWSListWord listWord = _repListWord.Get(p => p.ListUN == list.UN && p.WordUN == word.UN);

                // list word kaydet
                if (listWord == null)
                {
                    EWSListWord lWord = new EWSListWord()
                    {
                        UN = Guid.NewGuid(),
                        isPublic = false,
                        ListUN = list.UN,
                        WordUN = word.UN,
                    };
                    _repListWord.Add(lWord);
                }
            }
        }
    }
}
