using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Curriculum_Entry_YearAnnotation))]
    public partial class Acd_Curriculum_Entry_Year
    {
    }

    internal sealed class Acd_Curriculum_Entry_YearAnnotation
    {
        [Required]
        public short Curriculum_Entry_Year_Id { get; set; }

        [Required]
        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Required]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "Program Kelas Harus dipilih")]
        [Display(Name = "Program Kelas")]
        public short Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Angkatan Harus dipilih")]
        [Remote("IsDataExists", "CurriculumEntryYear", AdditionalFields = "Term_Year_Id, Department_Id, Class_Prog_Id", ErrorMessage = "Kombinasi Data telah digunakan")]
        [Display(Name = "Angkatan")]
        public short Entry_Year_Id { get; set; }

        [Required(ErrorMessage = "Kurikulum Harus dipilih")]
        [Display(Name = "Kurikulum")]
        public short Curriculum_Id { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<short> Order_Id { get; set; }
    }
}