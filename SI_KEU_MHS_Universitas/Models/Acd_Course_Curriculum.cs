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
    
    public partial class Acd_Course_Curriculum
    {
        public int Course_Cur_Id { get; set; }
        public short Department_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public short Curriculum_Id { get; set; }
        public int Course_Id { get; set; }
        public short Course_Group_Id { get; set; }
        public Nullable<short> Study_Level_Id { get; set; }
        public short Study_Level_Sub { get; set; }
        public Nullable<decimal> Applied_Sks { get; set; }
        public Nullable<decimal> Transcript_Sks { get; set; }
        public Nullable<bool> Is_For_Transcript { get; set; }
        public Nullable<bool> Is_Required { get; set; }
        public Nullable<bool> Is_For_Concentration { get; set; }
        public Nullable<short> Curriculum_Type_Id { get; set; }
        public Nullable<short> Order_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<bool> Is_Valid { get; set; }
    
        public virtual Acd_Course Acd_Course { get; set; }
        public virtual Mstr_Curriculum_Type Mstr_Curriculum_Type { get; set; }
        public virtual Mstr_Department Mstr_Department { get; set; }
        public virtual Mstr_Study_Level Mstr_Study_Level { get; set; }
        public virtual Acd_Course_Group Acd_Course_Group { get; set; }
        public virtual Mstr_Class_Program Mstr_Class_Program { get; set; }
        public virtual Mstr_Curriculum Mstr_Curriculum { get; set; }
    }
}
