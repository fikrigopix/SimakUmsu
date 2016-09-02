using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Education_TypeAnnotation))]
    public partial class Mstr_Education_Type
    {
    }

    internal sealed class Mstr_Education_TypeAnnotation
    {
        [Required]
        public short Education_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Jenjang Pendidikan Harus diisi")]
        [Display(Name = "Kode Jenjang Pendidikan")]
        [Remote("IsETCodeExists", "EducationType", ErrorMessage = "Kode Jenjang Pendidikan telah ada.")]
        public string Education_Type_Code { get; set; }

        [Display(Name = "Nama Jenjang Pendidikan")]
        public string Education_Type_Name { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}