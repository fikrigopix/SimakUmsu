using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_GraduationReg_Standar
    {
        //standar
        public long? Graduation_Reg_Id { get; set; }

        public long Student_Id { get; set; }

        [Display(Name = "NIM")]
        public string Nim { get; set; }

        [Display(Name = "Nama")]
        public string Full_Name { get; set; }

        [Display(Name = "Ttl")]
        public string Ttl
        {
            get {
                    if (Birth_Date.HasValue)
                    {
                        return Birth_Place + ", " + String.Format("{0:dd-MM-yyyy}", Birth_Date); 
                    }
                    else
                    {
                        return Birth_Place;
                    }
                }
        }
        public string Birth_Place { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Birth_Date { get; set; }

        [Display(Name = "L/P")]
        public string Gender_Type { get; set; }

        [Display(Name = "IPK")]
        public Nullable<decimal> Gpa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        [Display(Name = "Tgl Lulus Yudisium")]
        public Nullable<System.DateTime> Yudisium_Date { get; set; }

        [Display(Name = "Cuti")]
        public Nullable<short> Total_Smt_Vacation { get; set; }

        [Display(Name = "Smst Masuk")]
        public string Term_Name { get; set; }

        [Display(Name = "Lama Studi")]
        public Nullable<short> Total_Smt_Study { get; set; }

        [Display(Name = "Usia")]
        public Nullable<short> Age_Year { get; set; }

        [Display(Name = "Predikat")]
        public string Predicate_Name { get; set; }

        [Display(Name = "Status")]
        public string Register_Status_Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "No. Tlp")]
        public string Phone { get; set; }

        [Display(Name = "Nama Ortu")]
        public string Parent_Name { get; set; }

        [Display(Name = "Alamat Asal")]
        public string Address_0 { get; set; }

        [Display(Name = "Judul Skripsi")]
        public string Thesis_Title { get; set; }

        [Display(Name = "Title of Thesis")]
        public string Thesis_Title_Eng { get; set; }

        [Display(Name = "No.SK Yudisium")]
        public string Sk_Num { get; set; }

        [Display(Name = "No.Transkrip")]
        public string Transcript_Num { get; set; }

        [Display(Name = "No.Ijazah")]
        public string Certificate_Serial_Full { get; set; }

        //lengkap
        [Display(Name = "Dosen Pembimbing TA 1")]
        public string DosenPemb1 { get; set; }

        [Display(Name = "Dosen Pembimbing TA 2")]
        public string DosenPemb2 { get; set; }

        [Display(Name = "Dosen Penguji TA 1")]
        public string DosenPenguji1 { get; set; }

        [Display(Name = "Dosen Penguji TA 2")]
        public string DosenPenguji2 { get; set; }

        [Display(Name = "Tgl Mohon TA")]
        public Nullable<System.DateTime> Application_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        [Display(Name = "Tgl Pendadaran")]
        public Nullable<System.DateTime> Thesis_Exam_Date { get; set; }

        [Display(Name = "Nilai TA")]
        public string Grade { get; set; }

        //resume
        public string Faculty_Name { get; set; }

        public string Period_Name { get; set; }

        //create get
        public long? Graduation_Reg_Temp_Id { get; set; }

        //edit get
        public short? Entry_Year_Code { get; set; }
    }
}