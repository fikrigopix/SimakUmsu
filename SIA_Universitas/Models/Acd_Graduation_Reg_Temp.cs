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
    
    public partial class Acd_Graduation_Reg_Temp
    {
        public long Graduation_Reg_Temp_Id { get; set; }
        public short Graduate_Periode_Id { get; set; }
        public long Student_Id { get; set; }
        public string Full_Name { get; set; }
        public string Birth_Place_Id { get; set; }
        public Nullable<System.DateTime> Birth_Date { get; set; }
        public Nullable<System.DateTime> Thesis_Exam_Date { get; set; }
        public Nullable<System.DateTime> Yudisium_Date { get; set; }
        public Nullable<decimal> Gpa { get; set; }
        public string Thesis_Title { get; set; }
        public string Thesis_Title_Eng { get; set; }
        public string Address_0 { get; set; }
        public string Address_1 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Parent_Name { get; set; }
        public string Parent_Phone { get; set; }
        public string Job_Type { get; set; }
        public string Job_Institution_Name { get; set; }
        public string Job_Address { get; set; }
        public string Competence_1 { get; set; }
        public string Competence_2 { get; set; }
        public string Competence_3 { get; set; }
        public string Achievement_Type { get; set; }
        public string Achievement_Champion { get; set; }
        public string Achievement_Award { get; set; }
        public string Achievement_Organizer { get; set; }
        public string Achievement_Level { get; set; }
        public string Achievement_Time { get; set; }
        public Nullable<System.DateTime> Register_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Acd_Graduation_Period Acd_Graduation_Period { get; set; }
        public virtual Acd_Student Acd_Student { get; set; }
    }
}
