//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SI_KEU_MHS_Universitas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reg_LOKASI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reg_LOKASI()
        {
            this.SUB_LOKASI = new HashSet<SUB_LOKASI>();
        }
    
        public string LOKASI_ID { get; set; }
        public string NAMA_LOKASI { get; set; }
        public string SINGKATAN { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string PROVINCE_CODE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUB_LOKASI> SUB_LOKASI { get; set; }
    }
}
