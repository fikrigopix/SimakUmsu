using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_CitizenshipAnnotation))]
    public partial class Mstr_Citizenship
    {
    }

    internal sealed class Mstr_CitizenshipAnnotation
    {
        [Required]
        public byte Citizenship_Id { get; set; }

        [Required(ErrorMessage = "Kode Kewarganegaraan Harus diisi")]
        [Display(Name = "Kode Kewarganegaraan")]
        [Remote("IsCitizenshipCodeExists", "Citizenship", ErrorMessage = "Kode Kewarganegaraan telah ada.")]
        public string Citizenship_Code { get; set; }

        [Display(Name = "Nama Kewarganegaraan")]
        public string Citizenship_Name { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}