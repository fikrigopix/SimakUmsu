//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIA_Universitas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SUB_LOKASI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUB_LOKASI()
        {
            this.Reg_SOLD_FORMS = new HashSet<Reg_SOLD_FORMS>();
        }
    
        public string LOKASI_ID { get; set; }
        public string SUB_LOKASI_ID { get; set; }
        public string NAMA_SUB_LOKASI { get; set; }
        public string SINGKATAN { get; set; }
        public string ALAMAT { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string PROVINCE_CODE { get; set; }
        public string KODE_KOTA { get; set; }
    
        public virtual Reg_LOKASI Reg_LOKASI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reg_SOLD_FORMS> Reg_SOLD_FORMS { get; set; }
    }
}
