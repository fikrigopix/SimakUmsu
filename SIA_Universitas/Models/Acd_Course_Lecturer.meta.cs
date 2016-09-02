using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Course_LecturerAnnotation))]
    public partial class Acd_Course_Lecturer
    {
    }

    internal sealed class Acd_Course_LecturerAnnotation
    {
        [Required]
        public int Course_Lecturer_Id { get; set; }

        public int Employee_Id { get; set; }

        public int Course_Id { get; set; }

        public string Created_By { get; set; }

        public Nullable<System.DateTime> Created_Date { get; set; }

        public string Modified_By { get; set; }

        public Nullable<System.DateTime> Modified_Date { get; set; }

        public Nullable<int> Order_Id { get; set; }
    }
}