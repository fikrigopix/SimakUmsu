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
    
    public partial class Mstr_Province
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mstr_Province()
        {
            this.Mstr_City = new HashSet<Mstr_City>();
        }
    
        public short Province_Id { get; set; }
        public Nullable<short> Country_Id { get; set; }
        public string Province_Code { get; set; }
        public string Province_Name { get; set; }
        public string Province_Acronym { get; set; }
        public Nullable<byte> Order_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mstr_City> Mstr_City { get; set; }
        public virtual Mstr_Country Mstr_Country { get; set; }
    }
}
