using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Models
{
    [MetadataType(typeof(Fnc_Cost_Sched_DetailAnnotation))]
    public partial class Fnc_Cost_Sched_Detail
    {
        public string SAmount
        {
            get
            { 
                return Amount.ToString();
            }
            set
            {
                this.Amount = (int)decimal.Parse(Regex.Replace(value, @"[^\d.]", ""));
            }
        }
    }

    internal sealed class Fnc_Cost_Sched_DetailAnnotation
    {
        [Required]
        public int Cost_Sched_Detail_Id { get; set; }

        [Required]
        public int Cost_Sched_Id { get; set; }

        [Required(ErrorMessage = "Nama Biaya Harus dipilih")]
        [Remote("IsDataExists", "CostSchedDetail", AdditionalFields = "Cost_Sched_Id", ErrorMessage = "Nama Biaya sudah ada")]
        [Display(Name = "Nama Biaya")]
        public short Cost_Item_id { get; set; }

        [Required(ErrorMessage = "Biaya Harus diisi")]
        public string SAmount { get; set; }

        [Display(Name = "Biaya")]
        public int Amount { get; set; }

        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}