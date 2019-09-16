using EWS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Business.Contract
{
    public interface IUser
    {
        EWSUser Login(string loginname, string pass);

        EWSUser Register(string usernname,string pass,string namesurname);
    }
}
