using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Cost_SchedAnnotation))]
    public partial class Fnc_Cost_Sched
    {
    }

    internal sealed class Fnc_Cost_SchedAnnotation
    {
        [Required]
        public int Cost_Sched_Id { get; set; }

        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "Program Kelas Harus dipilih")]
        [Remote("IsDataExists", "CostSched", AdditionalFields = "Department_Id, Payment_Order, Entry_Year_Id, Entry_Period_Type_Id", ErrorMessage = "Kombinasi Data telah digunakan")]
        [Display(Name = "Program Kelas")]
        public short Class_Prog_Id { get; set; }

        [Display(Name = "Angkatan")]
        public short Entry_Year_Id { get; set; }

        [Display(Name = "Aturan")]
        public short Entry_Period_Type_Id { get; set; }

        [Display(Name = "Angsuran")]
        public short Payment_Order { get; set; }

        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Display(Name = "Mulai")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public System.DateTime Start_Date { get; set; }

        [Display(Name = "Selesai")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public System.DateTime End_Date { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}