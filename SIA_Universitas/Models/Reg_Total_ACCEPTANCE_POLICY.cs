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
    
    public partial class Reg_Total_ACCEPTANCE_POLICY
    {
        public short Faculty_Id { get; set; }
        public short Department_Id { get; set; }
        public Nullable<int> Total_Capacity { get; set; }
        public Nullable<int> Total_Cadangan { get; set; }
        public short Term_Year_Id { get; set; }
        public short Entry_Period_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public Nullable<short> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<short> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}
