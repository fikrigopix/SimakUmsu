using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Student_Khs
    {
        public short Class_Id { get; set; }
        public int Course_Id { get; set; }
        [Display(Name = "Kode Matakuliah")]
        public string Course_Code { get; set; }
        [Display(Name = "Nama Matakuliah")]
        public string Course_Name { get; set; }
        [Display(Name = "Kelas")]
        public string Class_Name { get; set; }
        [Display(Name = "Peserta")]
        public int Jml_Peserta { get; set; }
        [Display(Name = "Sudah Dinilai")]
        public int Sudah_Dinilai { get; set; }
    }
}