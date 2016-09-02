using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Job_CategoryAnnotation))]
    public partial class Mstr_Job_Category
    {
    }

    internal sealed class Mstr_Job_CategoryAnnotation
    {
        [Required]
        public byte Job_Category_Id { get; set; }

        [Required(ErrorMessage = "Kode Kategori Pekerjaan Harus diisi")]
        [Display(Name = "Kode Kategori Pekerjaan")]
        [Remote("IsJobCategoryCodeExists", "JobCategory", ErrorMessage = "Kode Kategori Pekerjaan telah ada.")]
        public Nullable<short> Job_Category_Code { get; set; }

        [Display(Name = "Nama Kategori Pekerjaan")]
        public string Job_Category_Name { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}