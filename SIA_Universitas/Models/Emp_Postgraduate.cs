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
    
    public partial class Emp_Postgraduate
    {
        public int Postgraduate_Id { get; set; }
        public int Employee_Id { get; set; }
        public short Stage_Education_Id { get; set; }
        public string Institution_Id { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Emp_Employee Emp_Employee { get; set; }
        public virtual Mstr_Stage_Education Mstr_Stage_Education { get; set; }
    }
}
