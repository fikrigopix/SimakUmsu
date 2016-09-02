using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Entry_YearAnnotation))]
    public partial class Mstr_Entry_Year
    {
    }

    internal sealed class Mstr_Entry_YearAnnotation
    {
        public short Entry_Year_Id { get; set; }

        [Display(Name = "Kode Angkatan")]
        public Nullable<short> Entry_Year_Code { get; set; }

        [Display(Name = "Nama Angkatan")]
        public string Entry_Year_Name { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}