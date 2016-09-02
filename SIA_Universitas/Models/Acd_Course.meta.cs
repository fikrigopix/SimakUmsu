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
    [MetadataType(typeof(Acd_CourseAnnotation))]
    public partial class Acd_Course
    {
        public string NameCode
        {
            get
            {
                return Course_Name + " (" + Course_Code + ")";
            }
        }
    }

    internal sealed class Acd_CourseAnnotation
    {
        [Required]
        public int Course_Id { get; set; }

        [Required]
        [Display(Name = "Prodi")]
        [Index("IX_ProdiAndCode", 1, IsUnique = true)]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "Jenis Matakuliah Harus diisi")]
        [Display(Name = "Jenis Matakuliah")]
        public short Course_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Matakuliah Harus diisi")]
        [Display(Name = "Kode Matakuliah")]
        [Index("IX_ProdiAndCode", 2, IsUnique = true)]
        [Remote("IsCourse_CodeExists", "Course", AdditionalFields = "Department_Id", ErrorMessage = "Kode Matakuliah telah ada.")]
        public string Course_Code { get; set; }

        [Required(ErrorMessage = "Nama Matakuliah Harus diisi")]
        [Display(Name = "Nama Matakuliah")]
        public string Course_Name { get; set; }

        [Display(Name = "Nama Matakuliah (English)")]
        public string Course_Name_Eng { get; set; }

        public string Created_By { get; set; }

        public Nullable<System.DateTime> Created_Date { get; set; }

        public string Modified_By { get; set; }

        public Nullable<System.DateTime> Modified_Date { get; set; }

        public Nullable<int> Order_Id { get; set; }
    }
}