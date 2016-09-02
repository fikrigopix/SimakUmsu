using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Fnc_Course_Cost_SksAnnotation))]
    public partial class Fnc_Course_Cost_Sks
    {
    }

    internal sealed class Fnc_Course_Cost_SksAnnotation
    {
        [Required]
        public int Course_Cost_Sks_Id { get; set; }

        [Required]
        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Required(ErrorMessage = "Prodi Harus dipilih")]
        [Remote("IsDataExists", "CourseCostSks", AdditionalFields = "Term_Year_Id, Class_Prog_Id, Entry_Year_Id", ErrorMessage = "Kombinasi Prodi & Angkatan telah digunakan")]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        [Required]
        [Display(Name = "Program Kelas")]
        public short Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Angkatan Harus dipilih")]
        [Display(Name = "Angkatan")]
        public short Entry_Year_Id { get; set; }

        [Required(ErrorMessage = "Biaya Harus diisi")]
        [Display(Name = "Biaya")]
        public Nullable<int> Amount_Per_Sks { get; set; }

        public string Created_By { get; set; }

        public Nullable<System.DateTime> Created_Date { get; set; }

        public string Modified_By { get; set; }

        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}