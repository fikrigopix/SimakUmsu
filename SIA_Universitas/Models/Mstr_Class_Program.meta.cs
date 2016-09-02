using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Class_ProgramAnnotation))]
    public partial class Mstr_Class_Program
    {
    }

    internal sealed class Mstr_Class_ProgramAnnotation
    {
        [Required]
        public short Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Kode Program Kelas Harus diisi")]
        [Display(Name = "Kode Program Kelas")]
        [Remote("IsClasProgCodeExists", "Class_Program", ErrorMessage = "Kode Program Kelas telah ada.")]
        public string Class_Prog_Code { get; set; }

        [Display(Name = "Nama Program Kelas")]
        public string Class_Program_Name { get; set; }

        [Display(Name = "Nama Program Kelas Eng")]
        public string Class_Program_Name_Eng { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}