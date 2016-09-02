using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_CurriculumAnnotation))]
    public partial class Mstr_Curriculum
    {
    }

    internal sealed class Mstr_CurriculumAnnotation
    {
        [Required]
        public short Curriculum_Id { get; set; }

        [Required(ErrorMessage = "{0} Harus diisi")]
        [Display(Name = "Kode Kurikulum")]
        [Remote("IsCurriculumCodeExists", "Curriculum", ErrorMessage = "{0} telah ada.")]
        [StringLength(8, ErrorMessage = "{0} maksimal 8 karakter")]
        public string Curriculum_Code { get; set; }

        [Required(ErrorMessage = "{0} Harus diisi")]
        [Display(Name = "Nama Kurikulum")]
        [StringLength(200, ErrorMessage = "{0} maksimal 200 karakter")]
        public string Curriculum_Name { get; set; }

        [Display(Name = "Order Id")]
        public Nullable<byte> Order_Id { get; set; }
    }
}