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
    
    public partial class Fnc_Course_Cost_Package
    {
        public int Course_Cost_Package_Id { get; set; }
        public int Course_Cost_Type_Id { get; set; }
        public short Entry_Year_Id { get; set; }
        public int Amount_Per_Mk { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Fnc_Course_Cost_Type Fnc_Course_Cost_Type { get; set; }
        public virtual Mstr_Entry_Year Mstr_Entry_Year { get; set; }
    }
}
