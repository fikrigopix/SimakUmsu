using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Student_KhsV2
    {
        //public short Class_Id { get; set; }
        public long Krs_Id { get; set; }
        [Display(Name = "NIM")]
        public string Nim { get; set; }
        [Display(Name = "Nama Mahasiswa")]
        public string Full_Name { get; set; }

        [Display(Name = "Kode Matakuliah")]
        public string Course_Code { get; set; }
        [Display(Name = "Nama Matakuliah")]
        public string Course_Name { get; set; }
        [Display(Name = "Nilai")]
        public string Grade_Letter { get; set; }
        [Display(Name = "Bobot")]
        public Nullable<decimal> Weight_Value { get; set; }
        [Display(Name = "SKS KRS")]
        public decimal Sks { get; set; }

        [Display(Name = "Masuk Transkrip")]
        public Nullable<bool> Is_For_Transcript { get; set; }
        [Display(Name = "SKS Transkrip")]
        public Nullable<decimal> Transcript_Sks { get; set; }

        public string semester { get; set; }
        public long Student_Id { get; set; }
    }

    public class Vm_Student_KhsV2_List
    {
        /// <summary>  
        /// To hold list of orders  
        /// </summary>  
        public List<Vm_Student_KhsV2> Vm_Student_KhsV2_Lists { get; set; }

    }
}