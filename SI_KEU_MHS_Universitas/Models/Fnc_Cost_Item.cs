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
    
    public partial class Fnc_Cost_Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fnc_Cost_Item()
        {
            this.Acd_Student_Krs = new HashSet<Acd_Student_Krs>();
            this.Fnc_Cost_Regular = new HashSet<Fnc_Cost_Regular>();
            this.Fnc_Cost_Regular_Personal = new HashSet<Fnc_Cost_Regular_Personal>();
            this.Fnc_Cost_Regular_Personal_Up = new HashSet<Fnc_Cost_Regular_Personal_Up>();
            this.Fnc_Cost_Regular_Up = new HashSet<Fnc_Cost_Regular_Up>();
            this.Fnc_Cost_Sched_Detail = new HashSet<Fnc_Cost_Sched_Detail>();
            this.Fnc_Cost_Timing_Personal = new HashSet<Fnc_Cost_Timing_Personal>();
            this.Fnc_Cost_Timing = new HashSet<Fnc_Cost_Timing>();
            this.Fnc_Returns_Setting = new HashSet<Fnc_Returns_Setting>();
            this.Fnc_Student_Payment = new HashSet<Fnc_Student_Payment>();
            this.Mstr_Course_Type_Fnc = new HashSet<Mstr_Course_Type_Fnc>();
        }
    
        public short Cost_Item_Id { get; set; }
        public string Cost_Item_Code { get; set; }
        public string Cost_Item_Name { get; set; }
        public string Acronym { get; set; }
        public Nullable<short> Order_Id { get; set; }
        public Nullable<bool> Is_Shodaqoh { get; set; }
        public Nullable<bool> Is_Krs { get; set; }
        public Nullable<bool> Is_Remediasi { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<bool> Is_Profession { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acd_Student_Krs> Acd_Student_Krs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Regular> Fnc_Cost_Regular { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Regular_Personal> Fnc_Cost_Regular_Personal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Regular_Personal_Up> Fnc_Cost_Regular_Personal_Up { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Regular_Up> Fnc_Cost_Regular_Up { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Sched_Detail> Fnc_Cost_Sched_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Timing_Personal> Fnc_Cost_Timing_Personal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Cost_Timing> Fnc_Cost_Timing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Returns_Setting> Fnc_Returns_Setting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fnc_Student_Payment> Fnc_Student_Payment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mstr_Course_Type_Fnc> Mstr_Course_Type_Fnc { get; set; }
    }
}
