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
    
    public partial class Emp_Organization_Membership
    {
        public int Organization_Membership_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Organization_Name { get; set; }
        public string Position { get; set; }
        public string Document_Name { get; set; }
        public string Document_Ext { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Emp_Employee Emp_Employee { get; set; }
    }
}
