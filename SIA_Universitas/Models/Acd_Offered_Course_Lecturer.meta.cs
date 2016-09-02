using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Offered_Course_LecturerAnnotation))]
    public partial class Acd_Offered_Course_Lecturer
    {
        [Required(ErrorMessage = "Dosen Harus dipilih")]
        public int[] Employees { get; set; }
    }

    internal sealed class Acd_Offered_Course_LecturerAnnotation
    {
        [Required]
        public int Acd_Offered_Course_Lecturer1 { get; set; }
        
        public int Offered_Course_id { get; set; }
        
        public int Employee_Id { get; set; }
        
        public Nullable<decimal> Sks_Weight { get; set; }

        [Required(ErrorMessage = "No. Urut Harus diisi")]
        [Display(Name = "No. Urut")]
        public short Order_Id { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}