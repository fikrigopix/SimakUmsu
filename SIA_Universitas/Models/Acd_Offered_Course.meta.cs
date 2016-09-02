using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Offered_CourseAnnotation))]
    public partial class Acd_Offered_Course
    {
        private SIAEntities db = new SIAEntities();

        [Required(ErrorMessage = "Kelas Harus dipilih")]
        public short[] Classes { get; set; }

        [Display(Name = "Nama Dosen")]
        public string dosen
        {
            get
            {
                var result = (from e in db.Emp_Employee
                              join ocl in db.Acd_Offered_Course_Lecturer on e.Employee_Id equals ocl.Employee_Id
                              join oc in db.Acd_Offered_Course on ocl.Offered_Course_id equals oc.Offered_Course_id
                              where (oc.Offered_Course_id == Offered_Course_id)
                              orderby ocl.Order_Id
                              select new
                              {
                                  e.Full_Name
                              }).ToArray();
                var i = 1;
                string finalResult = "";
                foreach (var d in result)
                {
                    if (i == result.Length)
                    {
                        finalResult += d.Full_Name;
                    }
                    else
                    {
                        finalResult += d.Full_Name + "; ";
                    }
                    i++;
                }
                return finalResult;
            }
            set { }
        }
    }

    internal sealed class Acd_Offered_CourseAnnotation
    {
        [Required]
        public int Offered_Course_id { get; set; }

        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Display(Name = "Prodi")]
        public Nullable<short> Department_Id { get; set; }

        [Display(Name = "Program Kelas")]
        public Nullable<short> Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Mata Kuliah Harus dipilih")]
        [Display(Name = "Nama Mata Kuliah")]
        public int Course_Id { get; set; }

        [Required(ErrorMessage = "Kelas Harus dipilih")]
        [Remote("IsDataExists", "OfferedCourse", AdditionalFields = "Term_Year_Id, Course_Id", ErrorMessage = "Data Sudah Ada")]
        [Display(Name = "Kelas")]
        public short Class_Id { get; set; }
        
        public Nullable<short> Total_Meeting { get; set; }

        [Display(Name = "Kapasitas")]
        public Nullable<short> Class_Capacity { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}