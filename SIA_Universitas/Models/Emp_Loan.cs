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
    
    public partial class Emp_Loan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Emp_Loan()
        {
            this.Emp_Loan_Installment = new HashSet<Emp_Loan_Installment>();
        }
    
        public int Loan_Id { get; set; }
        public int Employee_Id { get; set; }
        public int Loan_Type_Id { get; set; }
        public double Installment_Num { get; set; }
        public short Time_Period { get; set; }
        public System.DateTime Start_Period { get; set; }
        public int Salary_Or_Teach { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Emp_Employee Emp_Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp_Loan_Installment> Emp_Loan_Installment { get; set; }
        public virtual Emp_Loan_Type Emp_Loan_Type { get; set; }
    }
}
