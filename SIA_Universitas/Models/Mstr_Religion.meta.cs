using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_ReligionAnnotation))]
    public partial class Mstr_Religion
    {
    }

    internal sealed class Mstr_ReligionAnnotation
    {
        [Required]
        public byte Religion_Id { get; set; }

        [Required(ErrorMessage = "Kode Agama Harus diisi")]
        [Display(Name = "Kode Agama")]
        [Remote("IsReligionCodeExists", "Religion", ErrorMessage = "Kode Agama ini telah ada.")]
        public string Religion_Code { get; set; }

        [Display(Name = "Nama Agama")]
        public string Religion_Name { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}