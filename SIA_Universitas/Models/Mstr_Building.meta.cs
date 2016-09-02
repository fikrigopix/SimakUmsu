using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_BuildingAnnotation))]
    public partial class Mstr_Building
    {
    }

    internal sealed class Mstr_BuildingAnnotation
    {
        [Required]
        public short Building_Id { get; set; }

        [Required(ErrorMessage = "Kode Gedung Harus diisi")]
        [Display(Name = "Kode Gedung")]
        [Remote("IsBuildingCodeExists", "Building", ErrorMessage = "Kode Gedung telah ada.")]
        public string Building_Code { get; set; }

        [Required(ErrorMessage = "Nama Gedung Harus diisi")]
        [Display(Name = "Nama Gedung")]
        public string Building_Name { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}