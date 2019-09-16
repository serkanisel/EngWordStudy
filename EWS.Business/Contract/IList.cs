using EWS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business.Contract
{
    public interface IEWSList
    {
        List<EWSList> GetAllList(int? userID);
        EWSList GetList(Guid listID, int userID, bool numbers = false);

        void SetIKnowWord(Guid wordID,int userID);
        void RemoveIKnow(Guid wordID, int userID);

        void SetILearn(Guid wordID, int userID);
        void RemoveIWillLearn(Guid wordID, int userID);

        List<EWSLearnList> GetListIWillLearn(int userID);

        void OgrenilecekListeEkle(EWSLearnList ewsLearnList);

        void OgrenilecekListeCikar(EWSLearnList ewsLearnList);

        List<EWSList> GetUserListByUserID(int userID, bool getBagliNesneler = true);
        List<EWSList> GetSystemList(bool getBagliNesneler = true);

        EWSList SaveNewList(EWSList ewsList);

        void DeleteList(Guid listID);

        void ListedenCikar(Guid UN);

        void SaveReadPart(string fileName, int UserID,string fullText,string html);
    }
}
