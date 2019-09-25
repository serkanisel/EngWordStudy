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
    
    public partial class EWSList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EWSList()
        {
            this.EWSLearnList = new HashSet<EWSLearnList>();
            this.EWSListWord = new HashSet<EWSListWord>();
            this.EWSReadPart = new HashSet<EWSReadPart>();
            this.EWSSampleSentence = new HashSet<EWSSampleSentence>();
            this.EWSListWillLearn = new HashSet<EWSListWillLearn>();
        }
    
        public System.Guid UN { get; set; }
        public string Name { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<short> State { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EWSLearnList> EWSLearnList { get; set; }
        public virtual EWSUser EWSUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EWSListWord> EWSListWord { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EWSReadPart> EWSReadPart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EWSSampleSentence> EWSSampleSentence { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EWSListWillLearn> EWSListWillLearn { get; set; }
    }
}
