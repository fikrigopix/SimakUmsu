using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Cost_RegularAnnotation))]
    public partial class Fnc_Cost_Regular
    {
    }

    internal sealed class Fnc_Cost_RegularAnnotation
    {
        [Required]
        public int Cost_Regular_Id { get; set; }

        [Required]
        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Required]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        [Required]
        [Display(Name = "Program Kelas")]
        public Nullable<short> Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Angkatan Harus dipilih")]
        [Display(Name = "Angkatan")]
        public short Entry_Year_Id { get; set; }

        [Required]
        [Display(Name = "Aturan")]
        public Nullable<short> Entry_Period_Type_Id { get; set; }

        [Required(ErrorMessage = "Nama Biaya Harus dipilih")]
        [Remote("IsDataExists", "CostRegNonDPP", AdditionalFields = "Term_Year_Id, Department_Id, Class_Prog_Id, Entry_Year_Id, Entry_Period_Type_Id", ErrorMessage = "Kombinasi Nama Biaya & Angkatan telah digunakan")]
        [Display(Name = "Nama Biaya")]
        public short Cost_Item_Id { get; set; }

        [Required(ErrorMessage = "Biaya Harus diisi")]
        [Display(Name = "Biaya")]
        public Nullable<int> Amount { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}