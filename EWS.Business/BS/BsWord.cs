using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWS.Model;
using EWS.Repository;
using EWS.Business.Helpers;
using EWS.Business.Contract;
using EWS.Business.BS;

namespace EWS.Business
{
    public class BsWord : IWord, IDisposable
    {
        public void Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Gets all words in DB
        /// </summary>
        /// <returns></returns>
        public List<EWSWord> GetAllWords()
        {
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();

            return _rep.GetList();
        }

        /// <summary>
        /// Gets word by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public EWSWord GetWordById(Guid ID, int userID, bool ProxyCreationEnabled = true)
        {
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>(ProxyCreationEnabled);
            RepositoryBase<EWSSampleSentence> _repSen = new RepositoryBase<EWSSampleSentence>(ProxyCreationEnabled);
            RepositoryBase<EWSKnownWords> _repKnown = new RepositoryBase<EWSKnownWords>(false);
            RepositoryBase<EWSWillLearn> _repWillLearn = new RepositoryBase<EWSWillLearn>(false);

            EWSWord wrd = _rep.Get(p => p.UN == ID);

            //bilinenler listesinde var mı kontrol et. 
            EWSKnownWords known = _repKnown.Get(p => p.WordUN == wrd.UN && p.UserID == userID);

            if (known != null)
            {
                wrd.Known = true;
                wrd.WillLearn = false;
            }
            else
                wrd.Known = false;

            if (wrd.Known == false)
            {
                //bilinenler listesinde var mı kontrol et. 
                EWSWillLearn willLearn = _repWillLearn.Get(p => p.WordUN == wrd.UN && p.UserID == userID);

                if (willLearn != null)
                    wrd.WillLearn = true;
                else
                    wrd.WillLearn = false;

            }
            //EWSSampleSentence sample = wrd.EWSSampleSentence.FirstOrDefault();
            //sample.WordUN = wrd.UN;

            //_repSen.Add(sample);

            return _rep.Get(p => p.UN == ID);
        }

        public EWSWord WordSave(EWSWord word)
        {
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();

            if (word.UN == new Guid())
            {
                word.UN = Guid.NewGuid();
                word.CreatedDate = DateTime.Now;
                return _rep.Add(word);
            }
            else
            {
                EWSWord w = _rep.Get(p => p.UN==word.UN);

                w.Description = word.Description;

                return _rep.Update(w);
            }
                
        }

        public void WordSaveAsListMember(EWSWord word, EWSListWord listWord)
        {
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();
            RepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>();


            if (word.UN == new Guid())
            {
                word.UN = Guid.NewGuid();
                word = _rep.Add(word);
            }
            else
                word = _rep.Update(word);


            listWord.WordUN = word.UN;

            _repListWord.Add(listWord);
        }

        public EWSWord GetWordByEWSListWordID(Guid listID, int rownumber, string type, int userID)
        {
            rownumber--;
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>(false);

            RepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>(false);
            RepositoryBase<EWSKnownWords> _repListKnow = new RepositoryBase<EWSKnownWords>(false);
            RepositoryBase<EWSWillLearn> _repListLearn = new RepositoryBase<EWSWillLearn>(false);
            RepositoryBase<EWSSampleSentence> _repSampleSentence = new RepositoryBase<EWSSampleSentence>(false);

            List<EWSListWord> lists = null;
            List<EWSKnownWords> listKnown = null;
            List<EWSWillLearn> listLearn = null;

            Guid wID = new Guid();
            if (type == "all")
            {
                lists = _repListWord.GetList(p => p.ListUN == listID);
                wID = (Guid)lists[rownumber].WordUN;
            }
            if (type == "known")
            {
                listKnown = _repListKnow.GetList(p => p.UserID == userID);
                wID = (Guid)listKnown[rownumber].WordUN;
            }
            if (type == "willlearn")
            {
                listLearn = _repListLearn.GetList(p => p.UserID == userID);
                wID = (Guid)listLearn[rownumber].WordUN;
            }


            EWSWord word = _rep.Get(p => p.UN == wID);

            //kelimenin kullanıcı tarafından bilinip bilinmediğini kontrol et.
            EWSKnownWords lstKnown = _repListKnow.Get(p => p.WordUN == word.UN && p.UserID == userID);
            if (lstKnown != null)
                word.Known = true;
            else
                word.Known = false;

            EWSWillLearn lstLearn = _repListLearn.Get(p => p.WordUN == word.UN && p.UserID == userID);
            if (lstLearn != null)
                word.WillLearn = true;
            else
                word.WillLearn = false;

            //word.EWSSampleSentence = _repSampleSentence.GetList(p => p.WordUN == word.UN && p.UserID == userID);
            return word;

        }

        public EWSSampleSentence SaveComment(EWSSampleSentence ewsSample)
        {
            RepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            return _rep.Add(ewsSample);
        }

        public List<EWSSampleSentence> GetSampleSentences(Guid WordID, int userID)
        {
            RepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            return _rep.GetList(p => p.WordUN == WordID && p.UserID == userID);
        }

        public void DeleteComment(Guid ID)
        {
            RepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence samp = _rep.Get(p => p.UN == ID);

            _rep.Remove(samp);
        }

        public List<EWSWord> GetWords(string wordBody = "", string description = "")
        {
            IRepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();
            IRepositoryBase<EWSKnownWords> _repKnowWord = new RepositoryBase<EWSKnownWords>();

            List<EWSWord> result = null;

            if (!string.IsNullOrEmpty(wordBody) && !string.IsNullOrEmpty(description))
                result = _rep.GetList(p => p.WordBody.StartsWith(wordBody) || p.Description.StartsWith(description));
            else if (!string.IsNullOrEmpty(wordBody))
                result = _rep.GetList(p => p.WordBody.StartsWith(wordBody));
            else if (!string.IsNullOrEmpty(description))
                result = _rep.GetList(p => p.WordBody.StartsWith(description));

            if (result != null)
            {
                foreach (var item in result)
                {
                    EWSKnownWords known = _repKnowWord.Get(p => p.WordUN == item.UN);

                    if (known != null)
                        item.Known = true;
                }
            }
            return result;

        }

        public EWSWord GetWordByBody(string wordBody)
        {
            IRepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();
            IRepositoryBase<EWSKnownWords> _repKnown = new RepositoryBase<EWSKnownWords>();
            EWSWord word = _rep.Get(p => p.WordBody == wordBody);
            if (word != null)
            {
                EWSKnownWords known = _repKnown.Get(p => p.WordUN == word.UN);
                if (known != null)
                    word.Known = true;
            }
            return word;
        }

        public void AddWordToList(Guid ListUN, Guid wordUN)
        {
            IRepositoryBase<EWSListWord> _rep = new RepositoryBase<EWSListWord>();

            //get , add if doesn't exists 
            EWSListWord w = _rep.Get(p => p.ListUN == ListUN && p.WordUN == wordUN);

            if (w == null)
            {
                EWSListWord listWord = new EWSListWord()
                {
                    UN = Guid.NewGuid(),
                    isPublic = false,
                    ListUN = ListUN,
                    WordUN = wordUN,
                };

                _rep.Add(listWord);
            }
        }

        public void SaveWordMultiple(List<EWSWord> listOfWord, string listName, int userID)
        {
            IRepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();
            IRepositoryBase<EWSSampleSentence> _repSamp = new RepositoryBase<EWSSampleSentence>();
            IRepositoryBase<EWSList> _repList = new RepositoryBase<EWSList>();
            IRepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>();

            string lName = "";
            if (listName.Contains("."))
            {
                string[] preFileName = listName.Split('.');

                if (preFileName != null && preFileName.Count() > 0)
                    lName = preFileName[0];
            }
            else
                lName = listName;


            if (listOfWord == null || listOfWord.Count == 0)
                return;

            List<EWSWord> wordList = new List<EWSWord>();
            EWSList list = new EWSList()
            {
                UN = Guid.NewGuid(),
                Name = lName,
                UserID = userID,
            };

            if (userID == 1)
                list.UserID = null;

            list = _repList.Add(list);
            Translator t = new Translator();
            foreach (var item in listOfWord)
            {
                EWSWord w = _rep.Get(p => p.WordBody == item.WordBody);
                if (w == null)
                {
                    if (item.WordBody != null || item.Description == "")
                    {
                        string TranslateText = item.WordBody.Trim();
                        string result = t.Translate(TranslateText, "English", "Turkish");
                        item.Description = result;
                    }
                    w = new EWSWord()
                    {
                        UN = Guid.NewGuid(),
                        WordBody = item.WordBody,
                        UserID = userID,
                        Description = item.Description
                    };
                    w = _rep.Add(w);

                    wordList.Add(w);

                    foreach (var itemSamp in item.EWSSampleSentence)
                    {
                        if (itemSamp.Sentence != "" && itemSamp.SentenceMean == "")
                        {
                            itemSamp.Sentence = itemSamp.Sentence.Replace("\\n", "");
                            itemSamp.Sentence = itemSamp.Sentence.Replace("\\r", "");
                            string TranslateText = itemSamp.Sentence.Trim();
                            string result = t.Translate(TranslateText, "English", "Turkish");
                            itemSamp.SentenceMean = result;
                        }

                        EWSSampleSentence samp = new EWSSampleSentence()
                        {
                            Sentence = itemSamp.Sentence,
                            UserID = itemSamp.UserID,
                            SentenceMean = itemSamp.SentenceMean,
                            UN = Guid.NewGuid(),
                            WordUN = w.UN,
                        };

                        _repSamp.Add(samp);
                    }
                }
                else if (item.EWSSampleSentence != null)
                {
                    wordList.Add(w);
                    foreach (var itemSamp in item.EWSSampleSentence)
                    {
                        EWSSampleSentence samp = new EWSSampleSentence()
                        {
                            Sentence = itemSamp.Sentence,
                            UserID = itemSamp.UserID,
                            SentenceMean = itemSamp.SentenceMean,
                            UN = Guid.NewGuid(),
                            WordUN = w.UN,
                        };

                        _repSamp.Add(samp);
                    }
                }
            }

            foreach (var item in wordList)
            {
                EWSListWord listWrd = _repListWord.Get(p => p.WordUN == item.UN && p.ListUN == list.UN);

                if (listWrd != null)
                    continue;

                listWrd = new EWSListWord()
                {
                    UN = Guid.NewGuid(),
                    WordUN = item.UN,
                    ListUN = list.UN,
                    isPublic = false,
                };

                _repListWord.Add(listWrd);
            }
        }

        public List<EWSList> GetWordListByWordUN(Guid wordUN, int userID)
        {
            IRepositoryBase<EWSListWord> _repListWord = new RepositoryBase<EWSListWord>();

            List<EWSList> result = new List<EWSList>();

            List<EWSListWord> listKnown = _repListWord.GetList(p => p.WordUN == wordUN);

            IEWSList ilist = new BsEWSList();
            foreach (var item in listKnown)
            {
                EWSList list = ilist.GetList((Guid)item.ListUN, userID, true);
                result.Add(list);
            }
            return result;
        }

        public EWSSampleSentence SaveSampleSentence(EWSSampleSentence sentence)
        {
            IRepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            return _rep.Add(sentence);
        }

        public EWSSampleSentence GetSampleSentenceByBody(string body)
        {
            IRepositoryBase<EWSSampleSentence> _rep = new RepositoryBase<EWSSampleSentence>();

            return _rep.Get(p => p.Sentence == body);
        }

        public List<EWSWord> GetWordsByUserID(int userID, int? addType = null)
        {
            RepositoryBase<EWSWord> _rep = new RepositoryBase<EWSWord>();

            List<EWSWord> words = new List<EWSWord>();

            if (addType == null)
                words = _rep.GetList(p => p.UserID == userID && p.CreatedDate != null);
            else
                words = _rep.GetList(p => p.UserID == userID && p.CreatedDate != null && p.AddType==addType);

            return words;
        }

        public List<EWSKnownWords> GetKnownWords(int userID)
        {
            IRepositoryBase<EWSKnownWords> _repKnown = new RepositoryBase<EWSKnownWords>();

            List<EWSKnownWords> result = _repKnown.GetList(p => p.UserID == userID);

            foreach (var item in result)
            {
                item.EWSWord.Known = true;
                item.EWSWord.WillLearn = false;
            }
            return result;
        }

        public List<EWSWillLearn> GetWillLearnWords(int userID)
        {
            IRepositoryBase<EWSWillLearn> _repKnown = new RepositoryBase<EWSWillLearn>();

            List<EWSWillLearn> result = _repKnown.GetList(p => p.UserID == userID);

            foreach (var item in result)
            {
                item.EWSWord.WillLearn = true;
                item.EWSWord.Known = false;
            }
            return result;
        }
    }
}
