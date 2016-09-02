using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Student_KrsAnnotation))]
    public partial class Acd_Student_Krs
    {
    }

    internal sealed class Acd_Student_KrsAnnotation
    {
        [Required]
        public long Krs_Id { get; set; }
        
        public long Student_Id { get; set; }
        
        public short Term_Year_Id { get; set; }

        [Required(ErrorMessage = "Mata kuliah harus dipilih")]
        public int Course_Id { get; set; }
        
        public short Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Kelas harus dipilih")]
        [Display(Name = "Kelas")]
        public short Class_Id { get; set; }

        [Display(Name = "SKS")]
        public decimal Sks { get; set; }

        [Display(Name = "Biaya")]
        public Nullable<int> Amount { get; set; }
        
        public Nullable<int> Nb_Taking { get; set; }
        
        public Nullable<System.DateTime> Krs_Date { get; set; }
        
        public Nullable<System.DateTime> Due_Date { get; set; }
        
        public Nullable<short> Cost_Item_Id { get; set; }
        
        public Nullable<bool> Is_Approved { get; set; }
        
        public Nullable<bool> Is_Locked { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public Nullable<long> Order_Id { get; set; }
        
        public string Created_By { get; set; }
    }
}