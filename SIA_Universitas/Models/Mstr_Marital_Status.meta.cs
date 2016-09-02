using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Marital_StatusAnnotation))]
    public partial class Mstr_Marital_Status
    {
    }

    internal sealed class Mstr_Marital_StatusAnnotation
    {
        [Required]
        public byte Marital_Status_Id { get; set; }

        [Required(ErrorMessage = "Kode Status Perkawinan Harus diisi")]
        [Display(Name = "Kode Status Perkawinan")]
        [Remote("IsMaritalStatusCodeExists", "MaritalStatus", ErrorMessage = "Kode Status Perkawinan telah ada.")]
        public string Marital_Status_Code { get; set; }

        [Display(Name = "Nama Status Perkawinan")]
        public string Marital_Status_Type { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}