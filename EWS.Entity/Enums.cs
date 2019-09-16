using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWS.Model
{
    public enum ListState
    {
        [Display(Name = "normal")]
        normal = 1,

        [Display(Name = "silindi")]
        silindi= 2,

    }

    public enum AddType
    {
        [Display(Name = "EWS Web Site")]
        web = 1,
        
        [Display(Name = "Chrome Extension")]
        extension = 2,

        [Display(Name = "Readin Part")]
        readPart = 2
    }
}
