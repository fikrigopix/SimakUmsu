using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace SI_KEU_MHS_Universitas.Models
{
    public class ConDataContext : DataContext
    {
        public ConDataContext()
            : base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ConData"].ConnectionString)
        {
        }

        public ConDataContext(string connection)
            : base(connection)
        {
        }
    }
}