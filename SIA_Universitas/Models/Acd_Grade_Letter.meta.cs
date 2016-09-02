using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Grade_LetterAnnotation))]
    public partial class Acd_Grade_Letter
    {
    }

    internal sealed class Acd_Grade_LetterAnnotation
    {
        [Required]
        public short Grade_Letter_Id { get; set; }

        [Required(ErrorMessage = "Nilai Huruf Harus diisi")]
        [Display(Name = "Nilai Huruf")]
        [Remote("IsGradeLetterExists", "GradeLetter", ErrorMessage = "Nilai Huruf telah ada.")]
        public string Grade_Letter { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<short> Order_Id { get; set; }
    }
}