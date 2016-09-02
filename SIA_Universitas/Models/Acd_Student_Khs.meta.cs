using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Student_KhsAnnotation))]
    public partial class Acd_Student_Khs
    {
    }

    internal sealed class Acd_Student_KhsAnnotation
    {
        public long Khs_Id { get; set; }
        public Nullable<long> Krs_Id { get; set; }
        public Nullable<long> Student_Id { get; set; }
        public Nullable<short> Grade_Letter_Id { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string Modified_By { get; set; }
        [Display(Name = "Bobot")]
        public Nullable<decimal> Weight_Value { get; set; }
        public Nullable<bool> Is_Required { get; set; }
        public bool Is_For_Transkrip { get; set; }
        public Nullable<decimal> Bnk_Value { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Created_By { get; set; }
    }
}