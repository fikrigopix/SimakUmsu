using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Education_Program_TypeAnnotation))]
    public partial class Mstr_Education_Program_Type
    {
    }

    internal sealed class Mstr_Education_Program_TypeAnnotation
    {
        [Required]
        public short Education_Prog_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Strata Pendidikan Harus diisi")]
        [Display(Name = "Kode Strata Pendidikan")]
        [Remote("IsDataExists", "EducationProgramType", ErrorMessage = "Kode Strata Pendidikan telah ada.")]
        public Nullable<short> Education_Prog_Type_Code { get; set; }

        [Required]
        [Display(Name = "Nama")]
        public string Program_Name { get; set; }

        [Display(Name = "Nama (Eng)")]
        public string Program_Name_Eng { get; set; }

        [Display(Name = "Akronim")]
        public string Acronym { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}