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
    
    public partial class Acd_Class_Lecturer
    {
        public int Class_Lecturer_Id { get; set; }
        public int Course_Id { get; set; }
        public short Term_Year_Id { get; set; }
        public short Class_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public Nullable<decimal> Sks_Weight { get; set; }
        public Nullable<short> Order_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Acd_Course Acd_Course { get; set; }
        public virtual Emp_Employee Emp_Employee { get; set; }
        public virtual Mstr_Class Mstr_Class { get; set; }
        public virtual Mstr_Term_Year Mstr_Term_Year { get; set; }
    }
}
