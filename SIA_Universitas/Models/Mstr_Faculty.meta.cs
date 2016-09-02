using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_FacultyAnnotation))]
    public partial class Mstr_Faculty
    {
    }

    internal sealed class Mstr_FacultyAnnotation
    {
        [Required]
        public short Faculty_Id { get; set; }

        [Required(ErrorMessage = "Kode Fakultas Harus diisi")]
        [Display(Name = "Kode Fakultas")]
        [Remote("IsFacultyCodeExists", "Faculty", ErrorMessage = "Kode Fakultas telah ada.")]
        public string Faculty_Code { get; set; }

        [Display(Name = "Nama Fakultas")]
        public string Faculty_Name { get; set; }

        [Display(Name = "Nama Fakultas Eng")]
        public string Faculty_Name_Eng { get; set; }

        [Display(Name = "Akronim Fakultas")]
        [StringLength(10, ErrorMessage = "{0} maksimal 10 karakter")]
        public string Faculty_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}