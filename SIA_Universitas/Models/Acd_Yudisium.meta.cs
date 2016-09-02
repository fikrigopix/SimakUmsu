using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_YudisiumAnnotation))]
    public partial class Acd_Yudisium
    {
        private SIAEntities db = new SIAEntities();

        [Display(Name = "No. Ijazah")]
        public string Certificate_Serial_Full
        {
            get
            {
                return db.Acd_Graduation_Final.Where(gf => gf.Student_Id == Student_Id).Select(gf => gf.Certificate_Serial_Full).FirstOrDefault();
            }
            set { }
        }
    }

    internal sealed class Acd_YudisiumAnnotation
    {
        [Required]
        public long Student_Id { get; set; }

        [Display(Name = "Th/Smt")]
        public short Term_Year_Id { get; set; }

        [Display(Name = "SKS")]
        public Nullable<decimal> Sks_Total { get; set; }

        [Display(Name = "Jml Matakuliah")]
        public Nullable<byte> Course_Count { get; set; }
        
        public Nullable<decimal> Bnk { get; set; }

        [Display(Name = "IPK")]
        public Nullable<decimal> Gpa { get; set; }

        [Display(Name = "No.SK Yudisium")]
        public string Sk_Num { get; set; }

        [Display(Name = "Tgl Surat")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Sk_Date { get; set; }

        [Display(Name = "Tgl Yudisium")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Yudisium_Date { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Graduate_Date { get; set; }

        [Display(Name = "Tgl Permohonan")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Application_Date { get; set; }

        [Display(Name = "Status Kelulusan")]
        public Nullable<bool> Is_Graduated { get; set; }

        [Display(Name = "Predikat Lulus")]
        public Nullable<short> Graduate_Predicate_Id { get; set; }

        [Display(Name = "No. Transkrip")]
        public string Transcript_Num { get; set; }
        
        public Nullable<System.DateTime> Transcript_Date { get; set; }

        [Display(Name = "Jabatan Prodi")]
        public string Department_Functionary { get; set; }

        [Display(Name = "Pejabat Prodi")]
        public string Department_Functionary_Name { get; set; }

        [Display(Name = "NIK/NIP Pejabat Prodi")]
        public string Department_Functionary_Nik { get; set; }

        [Display(Name = "Jabatan Fakultas")]
        public string Faculty_Functionary { get; set; }

        [Display(Name = "Pejabat Fakultas")]
        public string Faculty_Functionary_Name { get; set; }

        [Display(Name = "NIK/NIP Pejabat Fakultas")]
        public string Faculty_Functionary_Nik { get; set; }
        
        public string Description { get; set; }
        
        public Nullable<short> Graduation_Period_Id { get; set; }
        
        public Nullable<short> Age_Year { get; set; }
        
        public Nullable<short> Age_Month { get; set; }
        
        public Nullable<short> Age_Day { get; set; }
        
        public Nullable<decimal> Age_Year_Length { get; set; }
        
        public Nullable<int> Age_Day_Length { get; set; }
        
        public Nullable<short> Study_Length_Year { get; set; }
        
        public Nullable<short> Study_Length_Month { get; set; }
        
        public Nullable<short> Study_Length_Day { get; set; }
        
        public Nullable<decimal> Study_Length_Sum_Year { get; set; }
        
        public Nullable<short> Study_Length_Sum_Day { get; set; }
        
        public Nullable<byte> Study_Smt_Length { get; set; }
        
        public Nullable<byte> Study_Smt_Off_Length { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<long> Order_Id { get; set; }
    }
}