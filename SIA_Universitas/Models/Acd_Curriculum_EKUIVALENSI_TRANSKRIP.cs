//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIA_Universitas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Acd_Curriculum_EKUIVALENSI_TRANSKRIP
    {
        public int Equivalency_Cur_Transcript_Id { get; set; }
        public string Equivalency_Cur_Code { get; set; }
        public long Student_Id { get; set; }
        public short Faculty_Id { get; set; }
        public short Department_Id { get; set; }
        public string Available_Edu_Id { get; set; }
        public short New_Course_Id { get; set; }
        public Nullable<short> Sks { get; set; }
        public string Grade_Letter { get; set; }
        public Nullable<decimal> Weight_Value { get; set; }
        public string Old_Course_Id_1 { get; set; }
        public Nullable<short> Old_Sks_1 { get; set; }
        public string Old_Grade_1 { get; set; }
        public string Old_Course_Id_2 { get; set; }
        public Nullable<short> Old_Sks_2 { get; set; }
        public string Old_Grade_2 { get; set; }
        public string Old_Course_Id_3 { get; set; }
        public Nullable<short> Old_Sks_3 { get; set; }
        public string Old_Grade_3 { get; set; }
        public Nullable<bool> Is_Edit { get; set; }
        public Nullable<short> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<short> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Acd_Student Acd_Student { get; set; }
        public virtual Acd_Curriculum_PERUBAHAN Acd_Curriculum_PERUBAHAN { get; set; }
    }
}
