using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Term_YearAnnotation))]
    public partial class Mstr_Term_Year
    {
    }

    internal sealed class Mstr_Term_YearAnnotation
    {
        
        public short Term_Year_Id { get; set; }

        [Display(Name = "Tahun")]
        public short Year_Id { get; set; }

        [Display(Name = "Semester")]
        public byte Term_Id { get; set; }

        [Display(Name = "Semester Berlaku")]
        public string Term_Year_Name { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}