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
    
    public partial class Acd_Student_Address
    {
        public long Student_Address_Id { get; set; }
        public Nullable<byte> Address_Category_Id { get; set; }
        public long Student_Id { get; set; }
        public Nullable<short> City_Id { get; set; }
        public Nullable<short> Country_Id { get; set; }
        public string Address { get; set; }
        public string Address_Medan { get; set; }
        public string Kecamatan { get; set; }
        public string Dusun { get; set; }
        public string Desa_Kelurahan { get; set; }
        public string Postal_Code { get; set; }
        public string Rw { get; set; }
        public string Rt { get; set; }
        public string Phone_Home { get; set; }
        public string Description { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<short> Order_Id { get; set; }
    
        public virtual Acd_Student Acd_Student { get; set; }
        public virtual Mstr_Address_Category Mstr_Address_Category { get; set; }
        public virtual Mstr_City Mstr_City { get; set; }
        public virtual Mstr_Country Mstr_Country { get; set; }
    }
}
