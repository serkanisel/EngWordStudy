//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EWS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EWSListWord
    {
        public System.Guid UN { get; set; }
        public Nullable<System.Guid> ListUN { get; set; }
        public Nullable<System.Guid> WordUN { get; set; }
        public bool isPublic { get; set; }
    
        public virtual EWSList EWSList { get; set; }
        public virtual EWSWord EWSWord { get; set; }
    }
}
