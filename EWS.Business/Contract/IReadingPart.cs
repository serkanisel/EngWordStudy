using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business.Contract
{
    public interface IEWSReadingPart
    {
        List<EWSReadPart> GetReadingPartsByUserID(int userID,bool getRelations=true);

        EWSReadPart GetReadingPart(Guid UN,bool getRelations=true);

        List<EWSSampleSentence> GetReadingPartSentences(Guid listUN, bool getRelations = true);
    }
}
