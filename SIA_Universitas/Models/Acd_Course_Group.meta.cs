using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Course_GroupAnnotation))]
    public partial class Acd_Course_Group
    {
    }

    internal sealed class Acd_Course_GroupAnnotation
    {
        [Required]
        public short Course_Group_Id { get; set; }

        [Required(ErrorMessage = "Kode Kelompok MK Harus diisi")]
        [Display(Name = "Kode Kelompok MK")]
        [Remote("IsCourseGroupCodeExists", "CourseGroup", ErrorMessage = "Kode Kelompok MK telah ada.")]
        public string Course_Group_Code { get; set; }

        [Display(Name = "Nama Kelompok MK")]
        public string Name_Of_Group { get; set; }

        [Display(Name = "Deskripsi")]
        public string Description { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<short> Order_Id { get; set; }
    }

     
}