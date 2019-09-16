using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business
{
    public interface IWord
    {
        EWSWord GetWordById(Guid ID, int userID,bool ProxyCreationEnabled =true);

        List<EWSWord> GetAllWords();

        EWSWord WordSave(EWSWord word);
        EWSWord GetWordByEWSListWordID(Guid listID, int rownumber,string type,int userID);

        EWSSampleSentence SaveComment(EWSSampleSentence ewsSample);

        List<EWSSampleSentence> GetSampleSentences(Guid WordID,int userID);

        void DeleteComment(Guid ID);

        List<EWSWord> GetWords(string wordBody = "", string Description = "");

        EWSWord GetWordByBody(string wordBody);

        void AddWordToList(Guid ListUN, Guid wordUN);

        void SaveWordMultiple(List<EWSWord> listOfWord,string listName,int userID);

        List<EWSList> GetWordListByWordUN(Guid wordUN,int userID);

        void WordSaveAsListMember(EWSWord word, EWSListWord listWord);

        EWSSampleSentence SaveSampleSentence(EWSSampleSentence sentence);

        EWSSampleSentence GetSampleSentenceByBody(string body);

        List<EWSWord> GetWordsByUserID(int userID, int? addType = null);

        List<EWSKnownWords> GetKnownWords(int userID);
        List<EWSWillLearn> GetWillLearnWords(int userID);
    }
}
