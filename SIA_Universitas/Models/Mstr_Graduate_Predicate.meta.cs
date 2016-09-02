using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Graduate_PredicateAnnotation))]
    public partial class Mstr_Graduate_Predicate
    {
    }

    internal sealed class Mstr_Graduate_PredicateAnnotation
    {

        [Required]
        public short Graduate_Predicate_Id { get; set; }

        [Required(ErrorMessage = "Kode Predikat Lulus Harus diisi")]
        [Display(Name = "Kode Predikat Lulus")]
        [Remote("IsGPCodeExists", "GraduatePredicate", ErrorMessage = "Kode Predikat Lulus telah ada.")]
        public short Graduate_Predicate_Code { get; set; }

        [Display(Name = "Predikat Lulus")]
        public string Predicate_Name { get; set; }

        [Display(Name = "Predikat Lulus (English)")]
        public string Predicate_Name_Eng { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}