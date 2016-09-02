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
    
    public partial class Emp_Teach_Kbk_Count
    {
        public int Teach_Kbk_Count_Id { get; set; }
        public short Term_Year_Id { get; set; }
        public Nullable<int> Course_Id { get; set; }
        public short Class_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public int Employee_Id { get; set; }
        public byte Month_Id { get; set; }
        public int Sks { get; set; }
        public Nullable<double> Index_Lecture { get; set; }
        public Nullable<int> Total_Lecture { get; set; }
        public Nullable<decimal> Adjustment_Spp { get; set; }
        public Nullable<int> Maximum_Meeting { get; set; }
        public Nullable<int> Total_Student_Class { get; set; }
        public Nullable<int> Minimum_Student_Class { get; set; }
        public Nullable<double> Percentage_Teach_Fee { get; set; }
        public Nullable<double> Percentage_Evaluation_Fee { get; set; }
        public Nullable<double> Percentage_Lecture_Evaluation { get; set; }
        public Nullable<double> Percentage_Pph { get; set; }
        public Nullable<double> Percentage_Infaq { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Attendance { get; set; }
        public Nullable<decimal> Bruto_Teach { get; set; }
        public Nullable<decimal> Pph_Teach { get; set; }
        public Nullable<decimal> Infaq_Teach { get; set; }
        public Nullable<decimal> Netto_Teach { get; set; }
        public Nullable<decimal> Bruto_Evaluation { get; set; }
        public Nullable<decimal> Pph_Evaluation { get; set; }
        public Nullable<decimal> Infaq_Evaluation { get; set; }
        public Nullable<decimal> Netto_Evaluation { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Acd_Course Acd_Course { get; set; }
        public virtual Emp_Employee Emp_Employee { get; set; }
        public virtual Mstr_Class Mstr_Class { get; set; }
        public virtual Mstr_Class_Program Mstr_Class_Program { get; set; }
        public virtual Mstr_Month Mstr_Month { get; set; }
        public virtual Mstr_Term_Year Mstr_Term_Year { get; set; }
    }
}
