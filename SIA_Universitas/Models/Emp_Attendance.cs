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
    
    public partial class Emp_Attendance
    {
        public int Attendance_Id { get; set; }
        public int Employee_Id { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> Check_In_Time { get; set; }
        public Nullable<System.DateTime> Check_Out_Time { get; set; }
        public string Ip_Enter { get; set; }
        public string Ip_Exit { get; set; }
        public string Ip_Forward_Enter { get; set; }
        public string Ip_Forward_Exit { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Emp_Employee Emp_Employee { get; set; }
    }
}
