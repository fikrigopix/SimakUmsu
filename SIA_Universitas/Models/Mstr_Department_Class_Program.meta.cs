using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Department_Class_ProgramAnnotation))]
    public partial class Mstr_Department_Class_Program
    {
        [Required(ErrorMessage = "Program Kelas Harus dipilih")]
        [Display(Name = "Program Kelas")]
        public short[] Class_Progs { get; set; }
    }

    internal sealed class Mstr_Department_Class_ProgramAnnotation
    {
        [Required]
        public short Department_Class_Prog_Id { get; set; }

        [Required(ErrorMessage = "Prodi Harus dipilih")]
        [Display(Name = "Prodi")]
        public short Department_Id { get; set; }

        //[Required(ErrorMessage = "Program Kelas Harus dipilih")]
        //[Display(Name = "Program Kelas")]
        public short Class_Prog_Id { get; set; }

        [Display(Name = "Nama")]
        public string Department_Class_Prog_Name { get; set; }

        [Display(Name = "Acronym")]
        public string Acronym { get; set; }

        [Display(Name = "Status Akreditasi")]
        public string Acreditation_Status { get; set; }

        [Display(Name = "No. Akreditasi")]
        public string Acreditation_Number { get; set; }

        [Display(Name = "Tgl. Akreditasi")]
        public Nullable<System.DateTime> Acreditation_date { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}