using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_DepartmentAnnotation))]
    public partial class Mstr_Department
    {
        private SIAEntities db = new SIAEntities();

        [Display(Name = "Program Kelas")]
        public string ClassProgs
        {
            get
            {
                var result = (from cp in db.Mstr_Class_Program
                              join dcp in db.Mstr_Department_Class_Program on cp.Class_Prog_Id equals dcp.Class_Prog_Id
                              join d in db.Mstr_Department on dcp.Department_Id equals d.Department_Id
                              where (d.Department_Id==Department_Id)
                              select new
                              {
                                  cp.Class_Program_Name
                              }
                              
                             ).ToArray();
                var i = 1;
                string finalResult = "";
                foreach (var cp in result)
                {
                    if (i == result.Length)
                    {
                        finalResult += cp.Class_Program_Name;
                    }
                    else
                    {
                        finalResult += cp.Class_Program_Name + "; ";
                    }
                    i++;
                }
                return finalResult;
            }
            set { }
        }
    }

    internal sealed class Mstr_DepartmentAnnotation
    {
        [Required]
        public short Department_Id { get; set; }

        [Required(ErrorMessage = "{0} Harus diisi")]
        [Display(Name = "Kode Program Studi")]
        [StringLength(10, ErrorMessage = "{0} maksimal 20 karakter")]
        [Remote("IsDepartmentCodeExists", "Department", ErrorMessage = "{0} telah ada.")]
        public string Department_Code { get; set; }

        [Display(Name = "Fakultas")]
        public Nullable<short> Faculty_Id { get; set; }

        [Display(Name = "Nama Program Studi")]
        public string Department_Name { get; set; }

        [Display(Name = "Nama Program Studi Eng")]
        public string Department_Name_Eng { get; set; }

        [Display(Name = "Akronim Program Studi")]
        public string Department_Acronym { get; set; }

        [Display(Name = "Nomor Urut")]
        public Nullable<byte> Order_Id { get; set; }

        [Display(Name = "No. SK Dikti")]
        public string Department_Dikti_Sk_Number { get; set; }

        [Display(Name = "Tgl. SK Dikti")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Department_Dikti_Sk_Date { get; set; }

        [Required(ErrorMessage = "Strata Pendidikan Harus diisi")]
        [Display(Name = "Strata Pendidikan")]
        public short Education_Prog_Type_Id { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}