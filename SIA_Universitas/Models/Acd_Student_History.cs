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
    
    public partial class Acd_Student_History
    {
        public long Student_History_Id { get; set; }
        public short Term_Year_Id { get; set; }
        public long Student_Id { get; set; }
        public Nullable<byte> Status_Id { get; set; }
        public Nullable<decimal> Sks_Smt { get; set; }
        public Nullable<decimal> Weight_Sks_Smt { get; set; }
        public Nullable<decimal> Gpa_Smt { get; set; }
        public Nullable<decimal> Sks_Cum { get; set; }
        public Nullable<decimal> Weight_Sks_Cum { get; set; }
        public Nullable<decimal> Gpa_Cum { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
    
        public virtual Acd_Student Acd_Student { get; set; }
        public virtual Mstr_Status Mstr_Status { get; set; }
        public virtual Mstr_Term_Year Mstr_Term_Year { get; set; }
    }
}
