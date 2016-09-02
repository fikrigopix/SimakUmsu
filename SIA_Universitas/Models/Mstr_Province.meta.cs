using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_ProvinceAnnotation))]
    public partial class Mstr_Province
    {
    }

    internal sealed class Mstr_ProvinceAnnotation
    {
        [Required]
        public short Province_Id { get; set; }

        [Required(ErrorMessage = "Negara Harus diisi")]
        [Display(Name = "Negara")]
        public short Country_Id { get; set; }

        [Required(ErrorMessage = "Kode Propinsi Harus diisi")]
        [Display(Name = "Kode Propinsi")]
        [Remote("IsProvinceCodeExists", "Province", ErrorMessage = "Kode Propinsi ini telah ada.")]
        public string Province_Code { get; set; }

        [Display(Name = "Nama Propinsi")]
        public string Province_Name { get; set; }

        [Display(Name = "Akronim Propinsi")]
        public string Province_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}