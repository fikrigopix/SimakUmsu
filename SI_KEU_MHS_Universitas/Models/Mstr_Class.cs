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
    
    public partial class Mstr_Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mstr_Class()
        {
            this.Acd_Class_Lecturer = new HashSet<Acd_Class_Lecturer>();
            this.Acd_Course_Classical = new HashSet<Acd_Course_Classical>();
            this.Acd_Course_Sched = new HashSet<Acd_Course_Sched>();
            this.Acd_Exam_Sched = new HashSet<Acd_Exam_Sched>();
            this.Acd_Offered_Course = new HashSet<Acd_Offered_Course>();
            this.Acd_Sched_Real = new HashSet<Acd_Sched_Real>();
            this.Acd_Student = new HashSet<Acd_Student>();
            this.Acd_Student_Khs_Transfer = new HashSet<Acd_Student_Khs_Transfer>();
            this.Acd_Student_Krs = new HashSet<Acd_Student_Krs>();
            this.Emp_Teach_Kbk = new HashSet<Emp_Teach_Kbk>();
            this.Emp_Teach_Kbk_Count = new HashSet<Emp_Teach_Kbk_Count>();
        }
    
        public short Class_Id { get; set; }
        public string Class_Name { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Class_Lecturer> Acd_Class_Lecturer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Course_Classical> Acd_Course_Classical { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Course_Sched> Acd_Course_Sched { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Exam_Sched> Acd_Exam_Sched { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Offered_Course> Acd_Offered_Course { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Sched_Real> Acd_Sched_Real { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Student> Acd_Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Student_Khs_Transfer> Acd_Student_Khs_Transfer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Student_Krs> Acd_Student_Krs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp_Teach_Kbk> Emp_Teach_Kbk { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp_Teach_Kbk_Count> Emp_Teach_Kbk_Count { get; set; }
    }
}
