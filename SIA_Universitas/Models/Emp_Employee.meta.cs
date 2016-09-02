using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Emp_EmployeeAnnotation))]
    public partial class Emp_Employee
    {
        public string NameNik
        {
            get
            {
                return Full_Name + " [" + Nik + "]";
            }
        }
    }

    internal sealed class Emp_EmployeeAnnotation
    {
        public int Employee_Id { get; set; }
        public string Nik { get; set; }
        public string Name { get; set; }
        public string First_Title { get; set; }
        public string Last_Title { get; set; }
        [Display(Name = "Nama Dosen")]
        public string Full_Name { get; set; }
        public string Birth_Place { get; set; }
        public Nullable<System.DateTime> Birth_Date { get; set; }
        public string Address { get; set; }
        public Nullable<byte> Gender_Id { get; set; }
        public Nullable<byte> Religion_Id { get; set; }
        public Nullable<int> Identity_Type_Id { get; set; }
        public string Identity_Number { get; set; }
        public Nullable<int> Bank_Id { get; set; }
        public string Rec_Num { get; set; }
        public string Phone_Mobile { get; set; }
        public string Phone_Home { get; set; }
        public Nullable<byte> Employee_Status_Id { get; set; }
        public Nullable<byte> Blood_Type_Id { get; set; }
        public string Nbm { get; set; }
        public string Nidn { get; set; }
        public string Email_General { get; set; }
        public string Email_Corporate { get; set; }
        public string Role { get; set; }
        public Nullable<byte> Active_Status_Id { get; set; }
        public string Npwp { get; set; }
        public string Nik_Salary { get; set; }
        public string Photos { get; set; }
        public string Password { get; set; }
        public string Nik_Finger_Print { get; set; }
        public Nullable<int> Fingerprint_Id { get; set; }
        public string Document_Serdos { get; set; }
        public string Document_Serdos_Ext { get; set; }
        public Nullable<int> Work_Unit_Id { get; set; }
        public Nullable<int> Department_Id { get; set; }
        public string Employee_Role { get; set; }
        public string Forum_Role { get; set; }
        public string Payroll_Role { get; set; }
        public int internal_eksternal { get; set; }
        public string Rfid { get; set; }
        public Nullable<int> Card_Accepted { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }

    }
}