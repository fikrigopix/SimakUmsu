//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SI_KEU_MHS_Universitas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mstr_Exam_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mstr_Exam_Type()
        {
            this.Acd_Exam_Sched = new HashSet<Acd_Exam_Sched>();
        }
    
        public short Exam_Type_Id { get; set; }
        public string Exam_Type_Code { get; set; }
        public string Exam_Type_Name { get; set; }
        public string Acronym { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Exam_Sched> Acd_Exam_Sched { get; set; }
    }
}
