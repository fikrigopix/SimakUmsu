using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
