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
    
    public partial class Mstr_Department_CLASS_
    {
        public short Department_Class_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public string Department_Name { get; set; }
        public string Department_Acronym { get; set; }
        public string SINGKATAN { get; set; }
        public string SINGKATAN_IJAZAH { get; set; }
        public string NAMA_PRODI_IJAZAH { get; set; }
        public string NAMA_PRODI_IJAZAH_ENG { get; set; }
        public string Title { get; set; }
        public string Title_ENG { get; set; }
        public string Title_Acronym { get; set; }
        public string Acreditation_Status { get; set; }
        public string Acreditation_Number { get; set; }
        public Nullable<System.DateTime> Acreditation_date { get; set; }
        public string IJIN_PREFIX { get; set; }
        public string IJIN_PREFIX_ENG { get; set; }
        public string IJIN_NOMOR { get; set; }
        public Nullable<System.DateTime> IJIN_TANGGAL { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}
