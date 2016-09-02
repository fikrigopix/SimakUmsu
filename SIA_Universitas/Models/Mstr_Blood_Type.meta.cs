using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Blood_TypeAnnotation))]
    public partial class Mstr_Blood_Type
    {
    }

    internal sealed class Mstr_Blood_TypeAnnotation
    {
        //[Required]
        public byte Blood_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Golongan Darah Harus diisi")]
        [Display(Name = "Kode Golongan Darah")]
        [Remote("IsBloodCodeExists", "BloodType", ErrorMessage = "Kode Golongan Darah telah ada.")]
        public string Blood_Code { get; set; }

        [Display(Name = "Jenis Golongan Darah")]
        public string Blood_Type { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}