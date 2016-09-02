using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    //public partial class Mstr_CountryMetadata
    //{
    //    [Required]
    //    public short Country_Id { get; set; }

    //    [Required(ErrorMessage="Kode Negara Harus diisi.")]
    //    [Display(Name = "Kode Negara")]
    //    [Remote("IsCountryCodeExists", "Country", ErrorMessage = "Kode Negara ini telah ada.")]
    //    public string Country_Code { get; set; }

    //    [Display(Name = "Nama Negara")]
    //    public string Country_Name { get; set; }

    //    [Display(Name = "Akronim Negara")]
    //    public string Country_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_ProvinceMetadata
    //{
    //    [Required]
    //    public short Province_Id { get; set; }

    //    [Required(ErrorMessage = "Negara Harus diisi")]
    //    [Display(Name = "Negara")]
    //    public short Country_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Propinsi Harus diisi")]
    //    [Display(Name = "Kode Propinsi")]
    //    [Remote("IsProvinceCodeExists", "Province", ErrorMessage = "Kode Propinsi ini telah ada.")]
    //    public string Province_Code { get; set; }

    //    [Display(Name = "Nama Propinsi")]
    //    public string Province_Name { get; set; }

    //    [Display(Name = "Akronim Propinsi")]
    //    public string Province_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }

    //}

    //public partial class Mstr_CityMetadata
    //{
    //    [Required]
    //    public short City_Id { get; set; }

    //    [Required(ErrorMessage = "Propinsi Harus diisi")]
    //    [Display(Name = "Propinsi")]
    //    public short Province_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Kota Harus diisi")]
    //    [Display(Name = "Kode Kota")]
    //    [Remote("IsCityCodeExists", "City", ErrorMessage = "Kode Kota ini telah ada.")]
    //    public string City_Code { get; set; }

    //    [Display(Name = "Nama Kota")]
    //    public string City_Name { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }

    //}

    //public partial class Mstr_GenderMetadata
    //{
    //    [Required]
    //    public byte Gender_Id { get; set; }

    //    [Display(Name = "Nama Jenis Kelamin")]
    //    public string Gender_Type { get; set; }

    //    [Display(Name = "Jenis Kelamin Eng")]
    //    public string Gender_Type_Eng { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_ReligionMetadata
    //{
    //    [Required]
    //    public byte Religion_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Agama Harus diisi")]
    //    [Display(Name = "Kode Agama")]
    //    [Remote("IsReligionCodeExists", "Religion", ErrorMessage = "Kode Agama ini telah ada.")]
    //    public string Religion_Code { get; set; }

    //    [Display(Name = "Nama Agama")]
    //    public string Religion_Name { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_FacultyMetadata
    //{
    //    [Required]
    //    public short Faculty_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Fakultas Harus diisi")]
    //    [Display(Name = "Kode Fakultas")]
    //    [Remote("IsFacultyCodeExists", "Faculty", ErrorMessage = "Kode Fakultas telah ada.")]
    //    public string Faculty_Code { get; set; }

    //    [Display(Name = "Nama Fakultas")]
    //    public string Faculty_Name { get; set; }

    //    [Display(Name = "Nama Fakultas Eng")]
    //    public string Faculty_Name_Eng { get; set; }

    //    [Display(Name = "Akronim Fakultas")]
    //    public string Faculty_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_DepartmentMetadata
    //{
    //    [Required]
    //    public short Department_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Program Studi Harus diisi")]
    //    [Display(Name = "Kode Program Studi")]
    //    [Remote("IsDepartmentCodeExists", "Department", ErrorMessage = "Kode Program Studi telah ada.")]
    //    public string Department_Code { get; set; }

    //    [Display(Name = "Fakultas")]
    //    public Nullable<short> Faculty_Id { get; set; }

    //    [Display(Name = "Nama Program Studi")]
    //    public string Department_Name { get; set; }

    //    [Display(Name = "Nama Program Studi Eng")]
    //    public string Department_Name_Eng { get; set; }

    //    [Display(Name = "Akronim Program Studi")]
    //    public string Department_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_Class_ProgramMetadata
    //{
    //    [Required]
    //    public short Class_Prog_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Program Kelas Harus diisi")]
    //    [Display(Name = "Kode Program Kelas")]
    //    [Remote("IsClasProgCodeExists", "Class_Program", ErrorMessage = "Kode Program Kelas telah ada.")]
    //    public string Class_Prog_Code { get; set; }

    //    [Display(Name = "Nama Program Kelas")]
    //    public string Class_Program_Name { get; set; }

    //    [Display(Name = "Nama Program Kelas Eng")]
    //    public string Class_Program_Name_Eng { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_Blood_TypeMetadata
    //{
    //    [Required]
    //    public byte Blood_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Golongan Darah Harus diisi")]
    //    [Display(Name = "Kode Golongan Darah")]
    //    [Remote("IsBloodCodeExists", "Blood", ErrorMessage = "Kode Golongan Darah telah ada.")]
    //    public string Blood_Code { get; set; }

    //    [Display(Name = "Jenis Golongan Darah")]
    //    public string Blood_Type { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_CitizenshipMetadata
    //{
    //    [Required]
    //    public byte Citizenship_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Kewarganegaraan Harus diisi")]
    //    [Display(Name = "Kode Kewarganegaraan")]
    //    [Remote("IsCitizenshipCodeExists", "Citizenship", ErrorMessage = "Kode Kewarganegaraan telah ada.")]
    //    public string Citizenship_Code { get; set; }

    //    [Display(Name = "Nama Kewarganegaraan")]
    //    public string Citizenship_Name { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_ConcentrationMetadata
    //{
    //    [Required]
    //    public short Concentration_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Konsentrasi Harus diisi")]
    //    [Display(Name = "Kode Konsentrasi")]
    //    [Remote("IsConcentrationCodeExists", "Concentration", ErrorMessage = "Kode Konsentrasi telah ada.")]
    //    public string Concentration_Code { get; set; }

    //    [Display(Name = "Departemen")]
    //    public short Department_Id { get; set; }

    //    [Display(Name = "Nama Konsentrasi")]
    //    public string Concentration_Name { get; set; }

    //    [Display(Name = "Nama Konsentrasi Eng")]
    //    public string Concentration_Name_Eng { get; set; }

    //    [Display(Name = "Akronim Konsentrasi")]
    //    public string Concentration_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_High_School_MajorMetadata
    //{
    //    [Required]
    //    public byte High_School_Major_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Jurusan SMA Asal Harus diisi")]
    //    [Display(Name = "Kode Jurusan SMA Asal")]
    //    [Remote("IsHighSchoolCodeExists", "HighSchoolMajor", ErrorMessage = "Kode Jurusan SMA Asal telah ada.")]
    //    public string High_School_Major_Code { get; set; }

    //    [Display(Name = "Nama Jurusan SMA Asal")]
    //    public string High_School_Major_Name { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_Job_CategoryMetadata
    //{
    //    [Required]
    //    public byte Job_Category_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Kategori Pekerjaan Harus diisi")]
    //    [Display(Name = "Kode Kategori Pekerjaan")]
    //    [Remote("IsJobCategoryCodeExists", "JobCategory", ErrorMessage = "Kode Kategori Pekerjaan telah ada.")]
    //    public Nullable<short> Job_Category_Code { get; set; }

    //    [Display(Name = "Nama Kategori Pekerjaan")]
    //    public string Job_Category_Name { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_MonthsMetadata
    //{
    //    [Required]
    //    public byte Month_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Bulan Harus diisi")]
    //    [Display(Name = "Kode Bulan")]
    //    [Remote("IsMonthCodeExists", "Months", ErrorMessage = "Kode Bulan telah ada.")]
    //    public Nullable<byte> Month_Code { get; set; }

    //    [Display(Name = "Nama Bulan")]
    //    public string Month_Name { get; set; }
    //}

    //public partial class Mstr_Marital_StatusMetadata
    //{
    //    [Required]
    //    public byte Marital_Status_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Status Perkawinan Harus diisi")]
    //    [Display(Name = "Kode Status Perkawinan")]
    //    [Remote("IsMaritalStatusCodeExists", "MaritalStatus", ErrorMessage = "Kode Status Perkawinan telah ada.")]
    //    public string Marital_Status_Code { get; set; }

    //    [Display(Name = "Nama Status Perkawinan")]
    //    public string Marital_Status_Type { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_Register_StatusMetadata
    //{
    //    [Required]
    //    public short Register_Status_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Status Daftar Harus diisi")]
    //    [Display(Name = "Kode Status Daftar")]
    //    [Remote("IsRegisterStatusCodeExists", "RegisterStatus", ErrorMessage = "Kode Status Daftar telah ada.")]
    //    public Nullable<short> Register_Status_Code { get; set; }

    //    [Display(Name = "Nama Status Daftar")]
    //    public string Register_Status_Name { get; set; }

    //    [Display(Name = "Akronim Status Daftar")]
    //    public string Register_Status_Acronym { get; set; }

    //    [Display(Name = "Nomor Urut")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}

    //public partial class Mstr_StatusMetadata
    //{
    //    [Required]
    //    public byte Status_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Status Harus diisi")]
    //    [Display(Name = "Kode Status")]
    //    [Remote("IsStatusCodeExists", "Status", ErrorMessage = "Kode Status telah ada.")]
    //    public string Status_Code { get; set; }

    //    [Display(Name = "Nama Status")]
    //    public string Status_Name { get; set; }
    //}

    //public partial class Acd_CourseMetadata
    //{
    //    [Required]
    //    public int Course_Id { get; set; }

    //    [Required]
    //    [Display(Name = "Prodi")]
    //    [Index("IX_ProdiAndCode", 1, IsUnique = true)]
    //    public short Department_Id { get; set; }

    //    [Required(ErrorMessage = "Jenis Matakuliah Harus diisi")]
    //    [Display(Name = "Jenis Matakuliah")]
    //    public short Course_Type_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Matakuliah Harus diisi")]
    //    [Display(Name = "Kode Matakuliah")]
    //    [Index("IX_ProdiAndCode", 2, IsUnique = true)]
    //    //[Remote("IsCourseCodeExists", "Course", ErrorMessage = "Kode Matakuliah telah ada.")]
    //    public string Course_Code { get; set; }

    //    [Required(ErrorMessage = "Nama Matakuliah Harus diisi")]
    //    [Display(Name = "Nama Matakuliah")]
    //    public string Course_Name { get; set; }

    //    [Display(Name = "Nama Matakuliah (English)")]
    //    public string Course_Name_Eng { get; set; }

    //    public Nullable<short> Created_By { get; set; }

    //    public Nullable<System.DateTime> Created_Date { get; set; }

    //    public Nullable<short> Modified_By { get; set; }

    //    public Nullable<System.DateTime> Modified_Date { get; set; }

    //    public Nullable<int> Order_Id { get; set; }
    //}

    //public partial class Mstr_Curriculum_AppliedMetadata
    //{
    //    [Required]
    //    public int Curiculum_Applied_Id { get; set; }

    //    [Required]
    //    [Display(Name = "Prodi")]
    //    public short Department_Id { get; set; }


    //    [Required(ErrorMessage = "Kurikulum Harus diisi")]
    //    [Display(Name = "Kurikulum")]
    //    public Nullable<short> Curiculum_Id { get; set; }

    //    [Required(ErrorMessage = "Kelas Program Harus diisi")]
    //    [Display(Name = "Kelas Program")]
    //    public Nullable<short> Class_Prog_Id { get; set; }

    //    public Nullable<short> Term_Start_Id { get; set; }

    //    [Display(Name = "SKS Wajib")]
    //    public Nullable<decimal> Total_Sks_Core { get; set; }

    //    [Display(Name = "SKS Pilihan")]
    //    public Nullable<decimal> Total_Sks_Elective { get; set; }

    //    [Display(Name = "Min GPA Lulus")]
    //    public Nullable<decimal> Min_Cum_Gpa { get; set; }

    //    [Display(Name = "Min SKS Lulus")]
    //    public Nullable<int> Sks_Completion { get; set; }
    //}

    //public partial class Acd_Course_TypeMetadata
    //{
    //    [Required]
    //    public short Course_Type_Id { get; set; }

    //    [Required(ErrorMessage = "Kode Tipe Matakuliah Harus diisi")]
    //    [Display(Name = "Kode Tipe Matakuliah")]
    //    [Remote("IsCourseTypeCodeExists", "Course_Type", ErrorMessage = "{0} telah ada.")]
    //    public Nullable<short> Course_Type_Code { get; set; }

    //    [Display(Name = "Tipe Matakuliah")]
    //    [StringLength(200, ErrorMessage = "{0} maksimal 200 karakter")]
    //    public string Course_Type_Name { get; set; }

    //    [Required(ErrorMessage = "Id Karakter Harus diisi")]
    //    [Display(Name = "Id Karakter")]
    //    [StringLength(3, ErrorMessage = "{0} maksimal 3 karakter")]
    //    public string Id_Character { get; set; }

    //    [Display(Name = "Dibuat oleh")]
    //    public Nullable<short> Created_By { get; set; }

    //    [Display(Name = "Tanggal Pembuatan")]
    //    public Nullable<System.DateTime> Created_Date { get; set; }

    //    [Display(Name = "Diubah oleh")]
    //    public Nullable<short> Modified_By { get; set; }

    //    [Display(Name = "Tanggal diubah")]
    //    public Nullable<System.DateTime> Modified_Date { get; set; }
    //}

    //public partial class Mstr_CurriculumMetadata
    //{
    //    [Required]
    //    public short Curriculum_Id { get; set; }

    //    [Required(ErrorMessage = "{0} Harus diisi")]
    //    [Display(Name = "Kode Kurikulum")]
    //    [Remote("IsCurriculumCodeExists", "Curriculum", ErrorMessage = "{0} telah ada.")]
    //    [StringLength(8, ErrorMessage = "{0} maksimal 8 karakter")]
    //    public string Curriculum_Code { get; set; }

    //    [Required(ErrorMessage = "{0} Harus diisi")]
    //    [Display(Name = "Nama Kurikulum")]
    //    [StringLength(200, ErrorMessage = "{0} maksimal 200 karakter")]
    //    public string Curriculum_Name { get; set; }

    //    [Display(Name = "Order Id")]
    //    public Nullable<byte> Order_Id { get; set; }
    //}
}