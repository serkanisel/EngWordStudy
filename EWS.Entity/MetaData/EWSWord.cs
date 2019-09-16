using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Model
{
    partial class EWSWord
    {
        public bool Known { get; set; }
        public bool WillLearn { get; set; }

        public int CurrentSequence { get; set; }
    }
}
