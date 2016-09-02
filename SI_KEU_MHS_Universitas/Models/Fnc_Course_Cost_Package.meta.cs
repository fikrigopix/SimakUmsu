using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Course_Cost_PackageAnnotation))]
    public partial class Fnc_Course_Cost_Package
    {
    }

    internal sealed class Fnc_Course_Cost_PackageAnnotation
    {
        [Required]
        public int Course_Cost_Package_Id { get; set; }

        [Required(ErrorMessage = "Mata Kuliah Harus dipilih")]
        [Remote("IsDataExists", "CourseCostPackage", AdditionalFields = "Entry_Year_Id", ErrorMessage = "Kombinasi Mata Kuliah & Angkatan telah digunakan")]
        [Display(Name = "Mata Kuliah")]
        public Nullable<int> Course_Cost_Type_Id { get; set; }

        [Required(ErrorMessage = "Angkatan Harus dipilih")]
        [Display(Name = "Angkatan")]
        public Nullable<short> Entry_Year_Id { get; set; }

        [Required(ErrorMessage = "Biaya Harus diisi")]
        [Display(Name = "Biaya")]
        public int Amount_Per_Mk { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}