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
    
    public partial class Reg_MAPEL_JNS_RAPORT
    {
        public short Raport_Type_Id { get; set; }
        public Nullable<short> Raport_Type_Code { get; set; }
        public int Mapel_Id { get; set; }
        public Nullable<short> Order_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Reg_MAPEL Reg_MAPEL { get; set; }
    }
}
