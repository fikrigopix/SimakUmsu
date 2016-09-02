using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_StatusAnnotation))]
    public partial class Mstr_Status
    {
    }

    internal sealed class Mstr_StatusAnnotation
    {
        [Required]
        public byte Status_Id { get; set; }

        [Required(ErrorMessage = "Kode Status Harus diisi")]
        [Display(Name = "Kode Status")]
        [Remote("IsStatusCodeExists", "Status", ErrorMessage = "Kode Status telah ada.")]
        public string Status_Code { get; set; }

        [Display(Name = "Nama Status")]
        public string Status_Name { get; set; }
    }
}