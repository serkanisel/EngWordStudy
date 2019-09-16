using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Model
{
    public partial class EWSList
    {
        public List<Guid> ListKnow { get; set; }
        //public List<EWSListWillLearn> ListWillLearn { get; set; }
        public bool isLearn { get; set; }
    }
}
