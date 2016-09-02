using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (email == "admin@umsu.ac.id" && password == "admin@pass")
            {
                Response.Cookies["user"].Value = "Administrator";
                Response.Cookies["user"].Expires = DateTime.Now.AddHours(1);
            }
            return RedirectToAction("index", "Home");
        }

        public ActionResult Logout()
        {
            Response.Cookies["user"].Expires = DateTime.Now.AddHours(-1);
            return RedirectToAction("index", "home");
        }
	}
}