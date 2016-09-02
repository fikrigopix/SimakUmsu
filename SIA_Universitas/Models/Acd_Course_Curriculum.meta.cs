using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_Course_CurriculumAnnotation))]
    public partial class Acd_Course_Curriculum
    {
    }

    internal sealed class Acd_Course_CurriculumAnnotation
    {
        public int Course_Cur_Id { get; set; }

        [Display(Name = "Program Studi")]
        public short Department_Id { get; set; }

        [Display(Name = "Kelas Program")]
        public short Class_Prog_Id { get; set; }

        [Display(Name = "Kurikulum")]
        public short Curriculum_Id { get; set; }

        [Display(Name = "Mata Kuliah")]
        public int Course_Id { get; set; }

        [Display(Name = "Kelompok Mata Kuliah")]
        public short Course_Group_Id { get; set; }

        public Nullable<short> Study_Level_Id { get; set; }

        [Display(Name = "Sub SMT")]
        public short Study_Level_Sub { get; set; }

        [Display(Name = "SKS")]
        public Nullable<decimal> Applied_Sks { get; set; }

        [Display(Name = "SKS Transkrip")]
        public Nullable<decimal> Transcript_Sks { get; set; }

        [Display(Name = "Transkrip")]
        public Nullable<bool> Is_For_Transcript { get; set; }

        [Display(Name = "Sifat")]
        public Nullable<bool> Is_Required { get; set; }

        public Nullable<bool> Is_For_Concentration { get; set; }

        [Display(Name = "Jenis Kurikulum")]
        public Nullable<short> Curriculum_Type_Id { get; set; }

        [Display(Name = "Urutan")]
        public Nullable<short> Order_Id { get; set; }

        [Display(Name = "Dibuat oleh")]
        public string Created_By { get; set; }

        [Display(Name = "Tanggal Dibuat")]
        public Nullable<System.DateTime> Created_Date { get; set; }

        [Display(Name = "Diubah Oleh")]
        public string Modified_By { get; set; }

        [Display(Name = "Tanggal Diubah")]
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}