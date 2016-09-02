using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_ConcentrationAnnotation))]
    public partial class Mstr_Concentration
    {
    }

    internal sealed class Mstr_ConcentrationAnnotation
    {
        [Required]
        public short Concentration_Id { get; set; }

        [Required(ErrorMessage = "Kode Konsentrasi Harus diisi")]
        [Display(Name = "Kode Konsentrasi")]
        [Remote("IsConcentrationCodeExists", "Concentration", ErrorMessage = "Kode Konsentrasi telah ada.")]
        public string Concentration_Code { get; set; }

        [Display(Name = "Departemen")]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "Nama Konsentrasi Harus diisi")]
        [Display(Name = "Nama Konsentrasi")]
        public string Concentration_Name { get; set; }

        [Display(Name = "Nama Konsentrasi Eng")]
        public string Concentration_Name_Eng { get; set; }

        [StringLength(5, ErrorMessage = "Max 5 Karakter. ")]
        [Display(Name = "Akronim Konsentrasi")]
        public string Concentration_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}