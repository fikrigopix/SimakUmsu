using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Cetak_Student_KhsV2
    {
        private SIAEntities db = new SIAEntities();

        //public short Class_Id { get; set; }
        public long Krs_Id { get; set; }
        [Display(Name = "NIM")]
        public string Nim { get; set; }
        [Display(Name = "Nama Mahasiswa")]
        public string Full_Name { get; set; }

        [Display(Name = "Kode Matakuliah")]
        public string Course_Code { get; set; }
        [Display(Name = "Nama Matakuliah")]
        public string Course_Name { get; set; }
        [Display(Name = "Nilai")]
        public string Grade_Letter { get; set; }
        [Display(Name = "Bobot")]
        public string Weight_Value { get; set; }
        [Display(Name = "SKS KRS")]
        public string Sks { get; set; }

        [Display(Name = "Masuk Transkrip")]
        public string Is_For_Transcript { get; set; }
        [Display(Name = "SKS Transkrip")]
        public string Transcript_Sks { get; set; }

        public string mutu { get; set; }
        public string bbtXjmlSksSmst { get; set; }
        public string jmlh_sks_bernilai { get; set; }
        public string ipSemester { get; set; }

        public string jumlahSksKumulatif { get; set; }
        public string ipKumulatif { get; set; }
        public string semester { get; set; }

        public long Student_Id { get; set; }

        public string dosenPembimbing
        {
            get {
                    string Full_Name = string.Empty;
                    try
                    {
                        Full_Name = db.Acd_Student_Supervision.Where(x => x.Student_Id == Student_Id).FirstOrDefault().Emp_Employee.Full_Name;
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                
                return Full_Name;
            }
            set { }

        }
        public string tanggal { get; set; }

    }
}