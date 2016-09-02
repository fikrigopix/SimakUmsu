using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    
    public class Vm_Student_KrsV1
    {
        public int Course_Id { get; set; }

        public short Class_Id { get; set; }

        [Display(Name = "Kode Matakuliah")]
        public string Course_Code { get; set; }

        [Display(Name = "Nama Matakuliah")]
        public string Course_Name { get; set; }

        [Display(Name = "Kelas")]
        public string Class_Name { get; set; }

        [Display(Name = "Kapasitas Kelas")]
        public Nullable<short> Class_Capacity { get; set; }

        [Display(Name = "Peserta")]
        public Nullable<int> jmlPeserta { get; set; }

        public int Offered_Course_id { get; set; }

        [Display(Name = "Sisa Kuota")]
        public Nullable<int> sisaKuota
        {
            get
            {
                return Class_Capacity - jmlPeserta;
            }
            set { }
        }
    }
}