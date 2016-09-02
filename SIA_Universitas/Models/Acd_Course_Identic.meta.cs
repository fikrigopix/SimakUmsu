using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Course_IdenticAnnotation))]
    public partial class Acd_Course_Identic
    {
        private SIAEntities db = new SIAEntities();

        [Display(Name = "Mata Kuliah")]
        public string MataKuliah
        {
            get
            {
                var result = (from c in db.Acd_Course
                              join cid in db.Acd_Course_Identic_Detail on c.Course_Id equals cid.Course_Id
                              join ci in db.Acd_Course_Identic on cid.Course_Identic_Id equals ci.Course_Identic_Id
                              where (ci.Course_Identic_Id == Course_Identic_Id)
                              orderby cid.Acd_Course.Course_Name
                              select new
                              {
                                  c.Course_Name,
                                  c.Course_Code
                              }).ToArray();
                var i = 1;
                string finalResult = "";
                foreach (var d in result)
                {
                    if (i == result.Length)
                    {
                        finalResult += d.Course_Name + "(" + d.Course_Code + ")";
                    }
                    else
                    {
                        finalResult += d.Course_Name + "(" + d.Course_Code + ")" + "; ";
                    }
                    i++;
                }
                return finalResult;
            }
            set { }
        }
    }

    internal sealed class Acd_Course_IdenticAnnotation
    {
        [Required]
        [Display(Name = "ID")]
        public short Course_Identic_Id { get; set; }

        [Required(ErrorMessage = "Nama Harus dipilih")]
        [Display(Name = "Mata Kuliah Setara")]
        [Remote("IsDataExists", "CourseIdentic", ErrorMessage = "Nama telah digunakan.")]
        public string Identic_Name { get; set; }

        [Required]
        public short Department_Id { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}