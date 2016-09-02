//using SIA_Universitas.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Controllers
{
    public class CobaChosenController : Controller
    {
        //private SIAEntities db = new SIAEntities();
        // GET: CobaChosen
        public ActionResult Index()
        {
            //ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Full_Name");
            return View();
        }
    }
}