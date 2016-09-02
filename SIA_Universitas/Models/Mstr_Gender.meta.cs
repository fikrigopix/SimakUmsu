using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_GenderAnnotation))]
    public partial class Mstr_Gender
    {
    }

    internal sealed class Mstr_GenderAnnotation
    {
        [Required]
        public byte Gender_Id { get; set; }

        [Display(Name = "Nama Jenis Kelamin")]
        public string Gender_Type { get; set; }

        [Display(Name = "Jenis Kelamin Eng")]
        public string Gender_Type_Eng { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }
    }
}