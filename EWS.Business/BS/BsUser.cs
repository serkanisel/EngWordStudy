using EWS.Business.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWS.Model;
using EWS.Repository;

namespace EWS.Business.BS
{
    public class BsUser : IUser, IDisposable
    {
        public void Dispose()
        {
            this.Dispose();
        }

        public EWSUser Login(string loginname, string pass)
        {
            RepositoryBase<EWSUser> _rep = new RepositoryBase<EWSUser>();
            RepositoryBase<EWSKnownWords> _repKnown = new RepositoryBase<EWSKnownWords>();
            RepositoryBase<EWSWillLearn> _repWillLearn = new RepositoryBase<EWSWillLearn>();

            EWSUser user = _rep.Get(p => p.UserName == loginname && p.Password == pass);

            user.KnownWordsCount = _repKnown.GetList(p => p.UserID == user.ID).Count();
            user.WillLearnWordsCount= _repWillLearn.GetList(p => p.UserID == user.ID).Count();

            return user;
        }

        public EWSUser Register(string username, string pass, string namesurname)
        {
            RepositoryBase<EWSUser> _rep = new RepositoryBase<EWSUser>();

            EWSUser ouser = new EWSUser()
            {
                Name = namesurname,
                Password = pass,
                UserName = username,
            };

            return _rep.Add(ouser);
        }
    }
}
