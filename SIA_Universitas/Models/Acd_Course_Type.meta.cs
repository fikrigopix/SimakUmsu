using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Course_TypeAnnotation))]
    public partial class Acd_Course_Type
    {
    }

    internal sealed class Acd_Course_TypeAnnotation
    {
        [Required]
        public short Course_Type_Id { get; set; }

        [Required(ErrorMessage = "Kode Tipe Matakuliah Harus diisi")]
        [Display(Name = "Kode Tipe Matakuliah")]
        [Remote("IsCourseTypeCodeExists", "Course_Type", ErrorMessage = "{0} telah ada.")]
        public Nullable<short> Course_Type_Code { get; set; }

        [Display(Name = "Tipe Matakuliah")]
        [StringLength(200, ErrorMessage = "{0} maksimal 200 karakter")]
        public string Course_Type_Name { get; set; }

        [Required(ErrorMessage = "Id Karakter Harus diisi")]
        [Display(Name = "Id Karakter")]
        [StringLength(3, ErrorMessage = "{0} maksimal 3 karakter")]
        public string Id_Character { get; set; }

        [Display(Name = "Dibuat oleh")]
        public string Created_By { get; set; }

        [Display(Name = "Tanggal Pembuatan")]
        public Nullable<System.DateTime> Created_Date { get; set; }

        [Display(Name = "Diubah oleh")]
        public string Modified_By { get; set; }

        [Display(Name = "Tanggal diubah")]
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}