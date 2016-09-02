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
    public class ProvinceController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Province
        public ActionResult Index(string currentFilter, string SearchString, int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            if (SearchString != null) { page = 1; }
            else{ SearchString = currentFilter; }

            ViewBag.currentFilter = SearchString;

            var mstr_Province = db.Mstr_Province.Include(m => m.Mstr_Country);
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                mstr_Province = mstr_Province.Where(m => m.Province_Name.Contains(SearchString) || m.Province_Code.Contains(SearchString));   
            }

            mstr_Province = mstr_Province.OrderBy(s => s.Province_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);
            return View(mstr_Province.ToPagedList(pageNumber,pageSize));
        }

        // GET: Province/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Province mstr_Province = db.Mstr_Province.Find(id);
            if (mstr_Province == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Province);
        }

        // GET: Province/Create
        public ActionResult Create()
        {
            ViewBag.Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Name");
            return View();
        }

        // POST: Province/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Province_Id,Country_Id,Province_Code,Province_Name,Province_Acronym,Order_Id")] Mstr_Province mstr_Province)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Province.Add(mstr_Province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Name", mstr_Province.Country_Id);
            return View(mstr_Province);
        }

        // GET: Province/Edit/5
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
            Mstr_Province mstr_Province = db.Mstr_Province.Find(id);
            if (mstr_Province == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Name", mstr_Province.Country_Id);
            return View(mstr_Province);
        }

        // POST: Province/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Province_Id,Country_Id,Province_Code,Province_Name,Province_Acronym,Order_Id")] Mstr_Province mstr_Province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Province).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Province province = db.Mstr_Province.Find(mstr_Province.Province_Id);
                    if (province == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Propinsi telah ada.";
                    return RedirectToAction("Edit", mstr_Province);
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Name", mstr_Province.Country_Id);
            return View(mstr_Province);
        }

        // GET: Province/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Province mstr_Province = db.Mstr_Province.Find(id);
        //    if (mstr_Province == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Province);
        //}

        // POST: Province/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Mstr_Province mstr_Province = db.Mstr_Province.Find(id);
            db.Mstr_Province.Remove(mstr_Province);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return Redirect(UrlReferrer);
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return Redirect(UrlReferrer);
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
        public JsonResult IsProvinceCodeExists(string strProvinceCode)
        {
            strProvinceCode = Request.QueryString["Province_Code"];
            return Json(!db.Mstr_Province.Any(c => c.Province_Code == strProvinceCode), JsonRequestBehavior.AllowGet);
        }
    }
}
