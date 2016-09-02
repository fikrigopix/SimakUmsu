//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using System.Web.Mvc;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_Study_levelAnnotation))]
    public partial class Mstr_Study_Level
    {
    }

    internal sealed class Mstr_Study_levelAnnotation
    {
        public short Study_Level_Id { get; set; }
        [Display(Name = "SMT")]
        public Nullable<short> Study_Level_Code { get; set; }
        public string Level_Name { get; set; }
        public string Description { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
    
}