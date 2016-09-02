using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_Student_KrsV2
    {
        public long Krs_Id { get; set; }
        public long Student_Id { get; set; }
        public short Term_Year_Id { get; set; }
        public int Course_Id { get; set; }
        public short Class_Prog_Id { get; set; }
        public short Class_Id { get; set; }
        public decimal Sks { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Nb_Taking { get; set; }
        public Nullable<System.DateTime> Krs_Date { get; set; }
        public Nullable<System.DateTime> Due_Date { get; set; }
        public Nullable<short> Cost_Item_Id { get; set; }
        public Nullable<bool> Is_Approved { get; set; }
        public Nullable<bool> Is_Locked { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<long> Order_Id { get; set; }
        public string Created_By { get; set; }
    }
}