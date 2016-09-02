using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Course_Cost_TypeAnnotation))]
    public partial class Fnc_Course_Cost_Type
    {
    }

    internal sealed class Fnc_Course_Cost_TypeAnnotation
    {
        [Required]
        public int Course_Cost_Type_Id { get; set; }

        public short Term_Year_Id { get; set; }
        
        public short Department_Id { get; set; }
        
        public Nullable<short> Class_Prog_Id { get; set; }
        
        public Nullable<int> Course_Id { get; set; }

        [Display(Name = "Tipe")]
        public Nullable<bool> Is_Sks { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}