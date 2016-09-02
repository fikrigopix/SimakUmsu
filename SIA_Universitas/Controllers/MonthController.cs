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

namespace SIA_Universitas.Controllers
{
    public class MonthController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Month
        public ActionResult Index()
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            return View(db.Mstr_Month.ToList());
        }

        // GET: Month/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Month mstr_Month = db.Mstr_Month.Find(id);
            if (mstr_Month == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Month);
        }

        // GET: Month/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Month/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Month_Id,Month_Code,Month_Name")] Mstr_Month mstr_Month)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Month.Add(mstr_Month);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Month);
        }

        // GET: Month/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Mstr_Month mstr_Month = db.Mstr_Month.Find(id);
            if (mstr_Month == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Month);
        }

        // POST: Month/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Month_Id,Month_Code,Month_Name")] Mstr_Month mstr_Month)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Month).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Country country = db.Mstr_Country.Find(mstr_Month.Month_Id);
                    if (country == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Bulan telah ada.";
                    return RedirectToAction("Edit", mstr_Month);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Month);
        }

        // GET: Month/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Month mstr_Month = db.Mstr_Month.Find(id);
        //    if (mstr_Month == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Month);
        //}

        // POST: Month/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Month mstr_Month = db.Mstr_Month.Find(id);
            db.Mstr_Month.Remove(mstr_Month);
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
        public JsonResult IsMonthCodeExists(string strMonthCode)
        {
            strMonthCode = Request.QueryString["Month_Code"];
            Int16 byteMonthCode = Convert.ToInt16(strMonthCode);
            return Json(!db.Mstr_Month.Any(c => c.Month_Code == byteMonthCode), JsonRequestBehavior.AllowGet);
        }
    }
}
