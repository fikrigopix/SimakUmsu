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
    
    public partial class Mstr_Department_Event_Sched
    {
        public int Department_Event_Sched_Id { get; set; }
        public short Term_Year_Id { get; set; }
        public short Department_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public short Event_Category_Id { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Mstr_Class_Program Mstr_Class_Program { get; set; }
        public virtual Mstr_Department Mstr_Department { get; set; }
        public virtual Mstr_Event_Category Mstr_Event_Category { get; set; }
        public virtual Mstr_Term_Year Mstr_Term_Year { get; set; }
    }
}
