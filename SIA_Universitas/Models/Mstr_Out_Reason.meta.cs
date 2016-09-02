using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Out_ReasonAnnotation))]
    public partial class Mstr_Out_Reason
    {
    }

    internal sealed class Mstr_Out_ReasonAnnotation
    {
        [Required]
        public short Out_Reason_ID { get; set; }

        [Required(ErrorMessage = "Kode Alasan Keluar Harus Diisi")]
        [Display(Name = "Kode Alasan Keluar")]
        [Remote("IsOutReasonCodeExists", "OutReason", ErrorMessage = "Kode Alasan Keluar telah ada.")]
        public Nullable<short> Out_Reason_Code { get; set; }
        
        [Required(ErrorMessage = "Alasan Keluar Harus Diisi")]
        [Display(Name = "Alasan Keluar")]
        public string Description { get; set; }
    }
}