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
    
    public partial class Reg_SOLD_FORMS
    {
        public string Kuitansi_Id { get; set; }
        public short Year_Id { get; set; }
        public string Entry_Period_Id { get; set; }
        public string Education_Group_Id { get; set; }
        public Nullable<byte> Register_Status_Id { get; set; }
        public Nullable<bool> Is_Ulang { get; set; }
        public Nullable<long> Prev_Camaru_Id { get; set; }
        public string Full_Name { get; set; }
        public Nullable<byte> Gender_Id { get; set; }
        public Nullable<float> Sold_Payment { get; set; }
        public string Sold_Paid { get; set; }
        public Nullable<System.DateTime> Sold_Date { get; set; }
        public string Sold_Officer { get; set; }
        public Nullable<int> Max_Return_Days { get; set; }
        public Nullable<System.DateTime> Sold_Time { get; set; }
        public Nullable<System.DateTime> First_Created { get; set; }
        public Nullable<System.DateTime> Last_Modified { get; set; }
        public string User_Created { get; set; }
        public string User_Modified { get; set; }
        public string jns_bayar { get; set; }
        public Nullable<System.DateTime> tgl_kirim { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public Nullable<long> Camaru_Id { get; set; }
        public string LOKASI_ID { get; set; }
        public string SUB_LOKASI_ID { get; set; }
    
        public virtual SUB_LOKASI SUB_LOKASI { get; set; }
    }
}
