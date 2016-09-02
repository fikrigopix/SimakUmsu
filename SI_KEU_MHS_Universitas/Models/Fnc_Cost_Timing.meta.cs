using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Cost_TimingAnnotation))]
    public partial class Fnc_Cost_Timing
    {
    }

    internal sealed class Fnc_Cost_TimingAnnotation
    {
        
        public int Cost_Timing_Id { get; set; }

        [Required(ErrorMessage = "Prodi Harus dipilih")]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "Program Kelas Harus dipilih")]
        [Remote("IsDataExists", "CostRegDPP", AdditionalFields = "Department_Id, Payment_Order, Entry_Year_Id, Entry_Period_Type_Id, Cost_Item_Id", ErrorMessage = "Kombinasi Data telah digunakan")]
        [Display(Name = "Program Kelas")]
        public short Class_Prog_Id { get; set; }

        [Display(Name = "Angkatan")]
        public short Entry_Year_Id { get; set; }
        
        public short Cost_Item_Id { get; set; }

        [Display(Name = "Gelombang")]
        public Nullable<short> Entry_Period_Type_Id { get; set; }

        [Required(ErrorMessage = "Angsuran Harus dipilih")]
        [Display(Name = "Angsuran ke")]
        public short Payment_Order { get; set; }

        [Required(ErrorMessage = "Th/Smt Harus dipilih")]
        [Display(Name = "Th/Smt")]
        public Nullable<short> Term_Year_Id { get; set; }

        [Display(Name = "TGL Tagih")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Due_Date { get; set; }

        //[Required(ErrorMessage = "Jml Angsuran Harus dipilih")]
        [Display(Name = "Jml Angsuran")]
        public int Amount { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}