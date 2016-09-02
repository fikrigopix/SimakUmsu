using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Curriculum_TypeAnnotation))]
    public partial class Mstr_Curriculum_Type
    {
    }
    internal sealed class Mstr_Curriculum_TypeAnnotation
    {
        [Required]
        public short Curriculum_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Jenis Kurikulum Harus diisi")]
        [Display(Name = "Kode Jenis Kurikulum")]
        [Remote("IsCTCodeExists", "CurriculumType", ErrorMessage = "Kode Jenis Kurikulum ini telah ada.")]
        public string Curriculum_Type_Code { get; set; }

        [Display(Name = "Jenis Kurikulum")]
        public string Curriculum_Type_Name { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}