using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_MonthAnnotation))]
    public partial class Mstr_Month
    {
    }

    internal sealed class Mstr_MonthAnnotation
    {
        [Required]
        public byte Month_Id { get; set; }

        [Required(ErrorMessage = "Kode Bulan Harus diisi")]
        [Display(Name = "Kode Bulan")]
        [Remote("IsMonthCodeExists", "Month", ErrorMessage = "Kode Bulan telah ada.")]
        public Nullable<byte> Month_Code { get; set; }

        [Display(Name = "Nama Bulan")]
        public string Month_Name { get; set; }
    }
}