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
    
    public partial class Fnc_Profession_PERIODE_MASA_BAYAR
    {
        public short Department_Id { get; set; }
        public short PERIODE { get; set; }
        public short MASA_BAYAR_KE { get; set; }
        public System.DateTime Start_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Fnc_Profession_PERIODE Fnc_Profession_PERIODE { get; set; }
    }
}
