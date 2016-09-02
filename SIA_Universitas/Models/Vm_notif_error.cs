using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class Vm_notif_error
    {
        private SIAEntities db = new SIAEntities();
        public long Student_Id { get; set; }
        public data NimNama { 
            get
            {
                return db.Acd_Student.Where(x => x.Student_Id == Student_Id)
                                .Select(x => new data
                                {
                                   Nim = x.Nim,
                                   Full_Name = x.Full_Name,
                                }).First();
            }
            set { }
        }
        public int? sksAllowed { get; set; }
        public decimal? sksAmbil { get; set; }
        public long? SisaSaldoSaatIni { get; set; }
        public string Keterangan {
            get
            {
                if (errorSks && errorSaldo)
                {
                    return "Kuota SKS dan saldo tidak cukup";
                }
                else if (errorSks)
                {
                    return "Kuota SKS tidak cukup.";
                }
                else
                {
                    return "Saldo tidak cukup.";
                }
            }
            set { }
        }

        public bool errorSks { get; set; }
        public bool errorSaldo { get; set; }
    }

    public class data
    {
        public string Nim;
        public string Full_Name;
    }
}