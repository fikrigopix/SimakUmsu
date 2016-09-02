using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Register_StatusAnnotation))]
    public partial class Mstr_Register_Status
    {
    }

    internal sealed class Mstr_Register_StatusAnnotation
    {
        [Required]
        public short Register_Status_Id { get; set; }

        [Required(ErrorMessage = "Kode Status Daftar Harus diisi")]
        [Display(Name = "Kode Status Daftar")]
        [Remote("IsRegisterStatusCodeExists", "RegisterStatus", ErrorMessage = "Kode Status Daftar telah ada.")]
        public Nullable<short> Register_Status_Code { get; set; }

        [Display(Name = "Nama Status Daftar")]
        public string Register_Status_Name { get; set; }

        [Display(Name = "Akronim Status Daftar")]
        public string Register_Status_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}