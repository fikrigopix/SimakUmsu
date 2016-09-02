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
    
    public partial class Mstr_Curriculum_Applied
    {
        public int Curiculum_Applied_Id { get; set; }
        public short Department_Id { get; set; }
        public Nullable<short> Curriculum_Id { get; set; }
        public Nullable<short> Class_Prog_Id { get; set; }
        public Nullable<short> Term_Start_Id { get; set; }
        public Nullable<decimal> Total_Sks_Core { get; set; }
        public Nullable<decimal> Total_Sks_Elective { get; set; }
        public Nullable<decimal> Min_Cum_Gpa { get; set; }
        public Nullable<int> Sks_Completion { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Mstr_Class_Program Mstr_Class_Program { get; set; }
        public virtual Mstr_Curriculum Mstr_Curriculum { get; set; }
        public virtual Mstr_Department Mstr_Department { get; set; }
    }
}
