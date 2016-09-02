using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_CountryAnnotation))]
    public partial class Mstr_Country
    {
    }

    internal sealed class Mstr_CountryAnnotation
    {
        [Required]
        public short Country_Id { get; set; }

        [Required(ErrorMessage = "Kode Negara Harus diisi.")]
        [Display(Name = "Kode Negara")]
        [Remote("IsCountryCodeExists", "Country", ErrorMessage = "Kode Negara ini telah ada.")]
        public string Country_Code { get; set; }

        [Display(Name = "Nama Negara")]
        public string Country_Name { get; set; }

        [Display(Name = "Akronim Negara")]
        public string Country_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}