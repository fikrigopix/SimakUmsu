using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Student_Supervision
    {
        public int Employee_Id { get; set; }

        [Display(Name = "NIK")]
        public string Nik { get; set; }

        [Display(Name = "Dosen")]
        public string Full_Name { get; set; }

        [Display(Name = "Jumlah Bimbingan")]
        public int Jml_bim { get; set; }

        [Display(Name = "Jumlah Lulus")]
        public int Jml_lulus { get; set; }

        [Display(Name = "Jumlah Belum Lulus")]
        public int Jml_blm_lulus 
        {
            get
            {
                return Jml_bim - Jml_lulus;
            }
        }
    }
}