using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_High_School_MajorAnnotation))]
    public partial class Mstr_High_School_Major
    {
    }

    internal sealed class Mstr_High_School_MajorAnnotation
    {
        [Required]
        public byte High_School_Major_Id { get; set; }

        [Required(ErrorMessage = "Kode Jurusan SMA Asal Harus diisi")]
        [Display(Name = "Kode Jurusan SMA Asal")]
        [Remote("IsHighSchoolCodeExists", "HighSchoolMajor", ErrorMessage = "Kode Jurusan SMA Asal telah ada.")]
        public string High_School_Major_Code { get; set; }

        [Display(Name = "Nama Jurusan SMA Asal")]
        public string High_School_Major_Name { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}