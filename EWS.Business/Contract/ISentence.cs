using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business.Contract
{
    public interface ISentence 
    {
        EWSSampleSentence GetSentenceById(Guid Id);
        List<EWSSampleSentence> GetSentences();

        EWSSampleSentence SaveSentence(EWSSampleSentence _sentence);

        void UpdateSentence(EWSSampleSentence sentence);

        void DeleteReletaionSentenceWord(EWSSampleSentence sentence);

        List<EWSSampleSentence> SearchSentence(string searchWord);

        EWSSampleSentence GetSentenceByBody(string sentence);

        List<EWSSentenceGroup> EWSSentenceGroups(int UserId);


        List<EWSSampleSentence> EWSGroupSentences(Guid groupUN);
    }
}
