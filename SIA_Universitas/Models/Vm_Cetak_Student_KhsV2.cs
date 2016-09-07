using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Cetak_Student_KhsV2
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
        public string Weight_Value { get; set; }
        [Display(Name = "SKS KRS")]
        public string Sks { get; set; }

        [Display(Name = "Masuk Transkrip")]
        public string Is_For_Transcript { get; set; }
        [Display(Name = "SKS Transkrip")]
        public string Transcript_Sks { get; set; }

        public string mutu { get; set; }
        public string bbtXjmlSksSmst { get; set; }
        public string jmlh_sks_bernilai { get; set; }
        public string IP { get; set; }

    }
}