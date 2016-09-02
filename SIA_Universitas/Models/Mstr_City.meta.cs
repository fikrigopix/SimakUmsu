using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_CityAnnotation))]
    public partial class Mstr_City
    {
    }

    internal sealed class Mstr_CityAnnotation
    {
        [Required]
        public short City_Id { get; set; }

        [Required(ErrorMessage = "Propinsi Harus diisi")]
        [Display(Name = "Propinsi")]
        public short Province_Id { get; set; }

        [Required(ErrorMessage = "Kode Kota Harus diisi")]
        [Display(Name = "Kode Kota")]
        [Remote("IsCityCodeExists", "City", ErrorMessage = "Kode Kota ini telah ada.")]
        public string City_Code { get; set; }

        [Display(Name = "Nama Kota")]
        public string City_Name { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}