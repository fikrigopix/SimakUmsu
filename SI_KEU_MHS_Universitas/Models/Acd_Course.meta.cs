using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Acd_CourseAnnotation))]
    public partial class Acd_Course
    {
    }

    internal sealed class Acd_CourseAnnotation
    {
        public int Course_Id { get; set; }
        
        public short Department_Id { get; set; }
        
        public short Course_Type_Id { get; set; }

        [Display(Name = "Kode")]
        public string Course_Code { get; set; }

        [Display(Name = "Mata Kuliah")]
        public string Course_Name { get; set; }

        [Display(Name = "Mata Kuliah (English)")]
        public string Course_Name_Eng { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<int> Order_Id { get; set; }
    }
}