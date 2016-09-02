using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace SIA_Universitas.Controllers
{
    public class CountryController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Countryfds
        public ActionResult Index(string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }

            ViewBag.currentFilter = searchString;

            var mstr_Country = db.Mstr_Country.Where(m => m.Country_Id != null);

            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_Country = mstr_Country.Where(m => m.Country_Name.Contains(searchString) || m.Country_Code.Contains(searchString));
            }

            mstr_Country = mstr_Country.OrderBy(s => s.Country_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_Country.ToPagedList(pageNumber, pageSize));
        }

        // GET: Country/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Country mstr_Country = db.Mstr_Country.Find(id);
            if (mstr_Country == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Country);
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Country_Id,Country_Code,Country_Name,Country_Acronym,Order_Id")] Mstr_Country mstr_Country)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Country.Add(mstr_Country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Country);
        }

        // GET: Country/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Mstr_Country mstr_Country = db.Mstr_Country.Find(id);
            if (mstr_Country == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Country);
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Country_Id,Country_Code,Country_Name,Country_Acronym,Order_Id")] Mstr_Country mstr_Country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Country).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Country country = db.Mstr_Country.Find(mstr_Country.Country_Id);
                    if (country == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Negara telah ada.";
                    return RedirectToAction("Edit", mstr_Country);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Country);
        }

        // GET: Country/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Country mstr_Country = db.Mstr_Country.Find(id);
        //    if (mstr_Country == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Country);
        //}

        // POST: Country/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Country mstr_Country = db.Mstr_Country.Find(id);
            db.Mstr_Country.Remove(mstr_Country);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index");
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public JsonResult IsCountryCodeExists(string strCountryCode)
        {
            strCountryCode = Request.QueryString["Country_Code"];
            return Json(!db.Mstr_Country.Any(c => c.Country_Code == strCountryCode), JsonRequestBehavior.AllowGet);
        }
    }
}
