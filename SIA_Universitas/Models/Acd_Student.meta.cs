using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Acd_StudentAnnotation))]
    public partial class Acd_Student
    {
        public string Name
        {
            get
            {
                return First_Title + Full_Name + Last_Title;
            }
        }
    }

    internal sealed class Acd_StudentAnnotation
    {
        [Required]
        public long Student_Id { get; set; }

        [Display(Name = "NIM")]
        public string Nim { get; set; }
        
        public Nullable<long> Register_Id { get; set; }
        
        public string Register_Number { get; set; }
        
        [Display(Name = "Nama Mahasiswa")]
        public string Full_Name { get; set; }

        [Display(Name = "Gelar Depan")]
        public string First_Title { get; set; }

        [Display(Name = "Gelar Belakang")]
        public string Last_Title { get; set; }

        [Display(Name = "Jenis Kelamin")]
        public Nullable<byte> Gender_Id { get; set; }
        
        public Nullable<short> Department_Id { get; set; }
        
        public Nullable<short> Class_Prog_Id { get; set; }

        [Display(Name = "Konsentrasi")]
        public Nullable<short> Concentration_Id { get; set; }
        
        public Nullable<short> Class_Id { get; set; }

        [Display(Name = "Tempat Lahir")]
        public string Birth_Place { get; set; }

        [Display(Name = "Tgl Lahir")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, d MMMM yyyy}")]
        public Nullable<System.DateTime> Birth_Date { get; set; }
        
        public Nullable<short> Birth_Place_Id { get; set; }
        
        public Nullable<short> Birth_Country_Id { get; set; }
        
        public Nullable<byte> Citizenship_Id { get; set; }
        
        public Nullable<short> Entry_Period_Id { get; set; }
        
        public Nullable<short> Entry_Period_Type_Id { get; set; }
        
        public Nullable<short> Entry_Year_Id { get; set; }
        
        public Nullable<byte> Entry_Term_Id { get; set; }
        
        public Nullable<short> Register_Status_Id { get; set; }
        
        public Nullable<byte> Religion_Id { get; set; }
        
        public Nullable<byte> Marital_Status_Id { get; set; }
        
        public Nullable<byte> Job_Id { get; set; }
        
        public Nullable<byte> Blood_Id { get; set; }
        
        public Nullable<byte> High_School_Major_Id { get; set; }
        
        public Nullable<byte> Status_Id { get; set; }
        
        public Nullable<System.DateTime> Registration_Date { get; set; }
        
        public Nullable<short> Registration_Officer_Id { get; set; }
        
        public Nullable<System.DateTime> Completion_Date { get; set; }
        
        public Nullable<System.DateTime> Out_Date { get; set; }
        
        public string Phone_Home { get; set; }

        [Display(Name = "No. Hp")]
        public string Phone_Mobile { get; set; }
        
        public string Email_Corporate { get; set; }
        
        public string Email_General { get; set; }
        
        public string Rfid { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
        
        public Nullable<long> Nisn { get; set; }

        [StringLength(16, ErrorMessage = "The Nik value cannot exceed 16 characters. ")]
        public string Nik { get; set; }

        public Nullable<byte> Source_Fund_Id { get; set; }
        
        public Nullable<byte> Read_Quran { get; set; }
        
        public Nullable<decimal> Transport { get; set; }
        
        public Nullable<byte> Photo_Status { get; set; }

        [Display(Name = "Password Mahasiswa")]
        [DataType(DataType.Password)]
        //[StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long, and cannot exceed {1} characters.", MinimumLength = 5)]  
        public string Student_Password { get; set; }

        [Display(Name = "Password Orang Tua")]
        [DataType(DataType.Password)]
        //[StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long, and cannot exceed {1} characters.", MinimumLength = 5)]  
        public string Parent_Password { get; set; }
        
        public Nullable<short> Hobby_Id { get; set; }
        
        public Nullable<int> Kebutuhan_Khusus { get; set; }
        
        public string Kk_Name { get; set; }
        
        public Nullable<decimal> Recieve_Kps { get; set; }
        
        public string Kps_Number { get; set; }

    }
}