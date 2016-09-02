using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Cost_Regular_PersonalAnnotation))]
    public partial class Fnc_Cost_Regular_Personal
    {
    }

    internal sealed class Fnc_Cost_Regular_PersonalAnnotation
    {
        [Required]
        public int Cost_Regular_Personal_Id { get; set; }

        [Required]
        public Nullable<long> Camaru_Id { get; set; }

        [Required(ErrorMessage = "Tahun Semester Harus dipilih")]
        [Display(Name = "Tahun Semester")]
        public short Term_Year_Id { get; set; }

        [Required(ErrorMessage = "Item Pembayaran Harus dipilih")]
        [Remote("IsDataExists", "CostRegPersonNonDPP", AdditionalFields = "Camaru_Id, Term_Year_Id", ErrorMessage = "Kombinasi Tahun Semester & Item Pembayaran telah digunakan")]
        [Display(Name = "Item Pembayaran")]
        public short Cost_Item_Id { get; set; }

        [Required(ErrorMessage = "Jenis Dispensasi Harus dipilih")]
        [Display(Name = "Jenis Dispensasi")]
        public short Bill_Type_Id { get; set; }

        [Display(Name = "Rupiah (Rp)")]
        public Nullable<int> Amount { get; set; }

        [Display(Name = "Persen (%)")]
        public Nullable<decimal> Percentage { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Tagih")]
        public Nullable<System.DateTime> Due_Date { get; set; }

        [Display(Name = "Keterangan")]
        public string Description { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}