using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Sched_SessionAnnotation))]
    public partial class Acd_Sched_Session
    {
    }

    internal sealed class Acd_Sched_SessionAnnotation
    {
        public short Sched_Session_Id { get; set; }

        [Display(Name = "Tipe")]
        public short Sched_Type_Id { get; set; }

        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Required(ErrorMessage = "Hari harus dipilih")]
        [Display(Name = "Hari")]
        public short Day_Id { get; set; }

        [Required(ErrorMessage = "Sesi harus dipilih")]
        [Display(Name = "Sesi")]
        [Remote("IsDataExists", "SchedSession", AdditionalFields = "Sched_Type_Id, Term_Year_Id, Day_Id", ErrorMessage = "Data Sudah Ada")]
        public Nullable<short> Order_Id { get; set; }

        [Display(Name = "Kelas Program")]
        public Nullable<short> Class_Prog_Id { get; set; }

        [StringJamValidator]
        [Display(Name = "Jam Mulai")]
        public string Time_Start { get; set; }

        [StringJamValidator]
        [Display(Name = "Jam Selesai")]
        public string Time_End { get; set; }

        [Display(Name = "Deskripsi")]
        public string Description { get; set; }
        
        public Nullable<short> Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public Nullable<short> Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}