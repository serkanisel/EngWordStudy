using EWS.Business.Contract;
using EWS.Model;
using EWS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business.BS
{
    public class BSSentence : ISentence
    {
        public void Dispose()
        {
            this.Dispose();
        }
        public EWSSampleSentence GetSentenceById(Guid Id)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence sentence = _repSentence.Get(p => p.UN == Id);

            return sentence;
        }

        public List<EWSSampleSentence> GetSentences()
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            List<EWSSampleSentence> sentences = _repSentence.GetList();

            return sentences;
        }

        public EWSSampleSentence SaveSentence(EWSSampleSentence _sentence)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence sentence = new EWSSampleSentence();
            sentence.UN = _sentence.UN;
            sentence.Sentence = _sentence.Sentence;
            sentence.SentenceMean = _sentence.SentenceMean;

            _repSentence.Add(sentence);

            return sentence;
        }

        public List<EWSSampleSentence> SearchSentence(string searchWord)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            List<EWSSampleSentence> listSentence = _repSentence.GetList(p => p.Sentence.Contains(searchWord) || p.SentenceMean.Contains(searchWord));

            return listSentence;
        }

        public void UpdateSentence(EWSSampleSentence _sentence)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence sentence = _repSentence.Get(p => p.UN == _sentence.UN);

            if (sentence != null)
            {
                sentence.Sentence = _sentence.Sentence;
                sentence.SentenceMean = _sentence.SentenceMean;
                sentence.WordUN = _sentence.WordUN;
            }

            _repSentence.Update(sentence);
        }

        public EWSSampleSentence GetSentenceByBody(string sentence)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence _sentence = _repSentence.Get(p => p.Sentence == sentence);

            return _sentence;
        }

        public void DeleteReletaionSentenceWord(EWSSampleSentence _sentence)
        {
            IRepositoryBase<EWSSampleSentence> _repSentence = new RepositoryBase<EWSSampleSentence>();

            EWSSampleSentence sentence = _repSentence.Get(p => p.UN == _sentence.UN);

            if (sentence != null)
            {
                sentence.WordUN = _sentence.WordUN;
            }

            _repSentence.Update(sentence);
        }

        public List<EWSSentenceGroup> EWSSentenceGroups(int UserId)
        {
            IRepositoryBase<EWSSentenceGroup> _resSentenceGroup = new RepositoryBase<EWSSentenceGroup>();

            List<EWSSentenceGroup> listSentenceGroup = _resSentenceGroup.GetList(p => p.UserID == UserId);

            return listSentenceGroup;
        }

        public List<EWSSampleSentence> EWSGroupSentences(Guid groupUN)
        {
            IRepositoryBase<EWSSentenceGroupTable> _resSentencesOfGroup = new RepositoryBase<EWSSentenceGroupTable>();

            IRepositoryBase<EWSSampleSentence> _resSentences = new RepositoryBase<EWSSampleSentence>();

            List<EWSSentenceGroupTable> listGroupTable;

            if (groupUN != Guid.Empty)
                listGroupTable = _resSentencesOfGroup.GetList(p => p.GroupUN == groupUN);
            else
                listGroupTable = _resSentencesOfGroup.GetList();

            List<EWSSampleSentence> returnList = new List<EWSSampleSentence>();

            foreach (var item in listGroupTable)
            {
                EWSSampleSentence sent = _resSentences.Get(p => p.UN == item.SentenceUN);

                returnList.Add(sent);
            }

            return returnList;
        }
    }
}
