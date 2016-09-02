using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Student_Khs_EditList
    {
        public long Krs_Id { get; set; }
        public string Course_Code { get; set; }
        public string Course_Name { get; set; }
        public string Class_Name { get; set; }

        public string Nim { get; set; }
        [Display(Name = "Nama Mhs")]
        public string Full_Name { get; set; }
        [Display(Name = "Kode Nilai")]
        public string Grade_Letter { get; set; }
        [Display(Name = "Bobot")]
        public Nullable<decimal> Weight_Value { get; set; }
        [Display(Name = "SKS KRS")]
        public decimal Sks { get; set; }

        [Display(Name = "Masuk Transkrip")]
        public Nullable<bool> Is_For_Transcript { get; set; }
        [Display(Name = "SKS Transkrip")]
        public Nullable<decimal> Transcript_Sks { get; set; }

        public string[] duo { get; set; }
    }
}