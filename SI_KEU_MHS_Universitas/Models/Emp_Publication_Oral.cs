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
    
    public partial class Emp_Publication_Oral
    {
        public int Publication_Oral_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Title { get; set; }
        public string Conference_Forum { get; set; }
        public string Institution_Organizer { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public string Document_Name { get; set; }
        public string Document_Ext { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Emp_Employee Emp_Employee { get; set; }
    }
}
