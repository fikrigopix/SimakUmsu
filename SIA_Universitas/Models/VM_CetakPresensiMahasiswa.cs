using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class VM_CetakPresensiMahasiswa
    {
        public string Nim { get; set; }
        public string Full_Name { get; set; }
        public string Gender { get; set; }
        public string Faculty_Name { get; set; }
        public string Department_Name { get; set; }
        public string Class_Name { get; set; }
        public string Semester { get; set; }
        public string Course_Code { get; set; }
        public string Course_Name { get; set; }
        public string Dosen_Full_Name { get; set; }
        public string Jadwal { get; set; }
        public string Judul { get; set; }
        public string Hari { get; set; }
        public string Tanggal { get; set; }
        public string Waktu { get; set; }
        public string Ruang { get; set; }
    }
}