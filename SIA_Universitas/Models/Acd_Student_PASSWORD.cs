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
    
    public partial class Acd_Student_PASSWORD
    {
        public string Student_Password_Id { get; set; }
        public long Student_Id { get; set; }
        public string Password { get; set; }
        public string Password_2 { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<int> Created_Number { get; set; }
    
        public virtual Acd_Student Acd_Student { get; set; }
    }
}
