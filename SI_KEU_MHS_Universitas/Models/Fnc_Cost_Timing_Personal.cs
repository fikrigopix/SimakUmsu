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
    
    public partial class Fnc_Cost_Timing_Personal
    {
        public int Cost_Timing_Personal_Id { get; set; }
        public short Camaru_Id { get; set; }
        public short Cost_Item_Id { get; set; }
        public short Payment_Order { get; set; }
        public short Term_Year_Id { get; set; }
        public Nullable<System.DateTime> Due_Date { get; set; }
        public Nullable<int> Amount { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Fnc_Cost_Item Fnc_Cost_Item { get; set; }
        public virtual Mstr_Term_Year Mstr_Term_Year { get; set; }
    }
}
