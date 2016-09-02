using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Curriculum_AppliedAnnotation))]
    public partial class Mstr_Curriculum_Applied
    {
    }

    internal sealed class Mstr_Curriculum_AppliedAnnotation
    {
        [Required]
        public int Curiculum_Applied_Id { get; set; }

        [Required]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }


        [Required(ErrorMessage = "Kurikulum Harus dipilih")]
        [Display(Name = "Kurikulum")]
        public Nullable<short> Curriculum_Id { get; set; }

        [Required(ErrorMessage = "Kelas Program Harus dipilih")]
        [Display(Name = "Kelas Program")]
        [Remote("IsDataExists", "CurriculumApplied", AdditionalFields = "Department_Id, Curriculum_Id", ErrorMessage = "Kombinasi Kurikulum & Kelas Program telah digunakan")]
        public Nullable<short> Class_Prog_Id { get; set; }

        public Nullable<short> Term_Start_Id { get; set; }

        [Display(Name = "SKS Wajib")]
        public Nullable<decimal> Total_Sks_Core { get; set; }

        [Display(Name = "SKS Pilihan")]
        public Nullable<decimal> Total_Sks_Elective { get; set; }

        [Display(Name = "Min GPA Lulus")]
        public Nullable<decimal> Min_Cum_Gpa { get; set; }

        [Display(Name = "Min SKS Lulus")]
        public Nullable<int> Sks_Completion { get; set; }
    }
}