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
    public class HighSchoolMajorController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: HighSchoolMajor
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
            return View(db.Mstr_High_School_Major.ToList());
        }

        // GET: HighSchoolMajor/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_High_School_Major mstr_High_School_Major = db.Mstr_High_School_Major.Find(id);
            if (mstr_High_School_Major == null)
            {
                return HttpNotFound();
            }
            return View(mstr_High_School_Major);
        }

        // GET: HighSchoolMajor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HighSchoolMajor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "High_School_Major_Id,High_School_Major_Code,High_School_Major_Name,Order_Id")] Mstr_High_School_Major mstr_High_School_Major)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_High_School_Major.Add(mstr_High_School_Major);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_High_School_Major);
        }

        // GET: HighSchoolMajor/Edit/5
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
            Mstr_High_School_Major mstr_High_School_Major = db.Mstr_High_School_Major.Find(id);
            if (mstr_High_School_Major == null)
            {
                return HttpNotFound();
            }
            return View(mstr_High_School_Major);
        }

        // POST: HighSchoolMajor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "High_School_Major_Id,High_School_Major_Code,High_School_Major_Name,Order_Id")] Mstr_High_School_Major mstr_High_School_Major)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_High_School_Major).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_High_School_Major high_school_major = db.Mstr_High_School_Major.Find(mstr_High_School_Major.High_School_Major_Id);
                    if (high_school_major == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Jurusan SMA Asal telah ada.";
                    return RedirectToAction("Edit", mstr_High_School_Major);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_High_School_Major);
        }

        // GET: HighSchoolMajor/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_High_School_Major mstr_High_School_Major = db.Mstr_High_School_Major.Find(id);
        //    if (mstr_High_School_Major == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_High_School_Major);
        //}

        // POST: HighSchoolMajor/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_High_School_Major mstr_High_School_Major = db.Mstr_High_School_Major.Find(id);
            db.Mstr_High_School_Major.Remove(mstr_High_School_Major);
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
        public JsonResult IsHighSchoolCodeExists(string strHighSchoolCode)
        {
            strHighSchoolCode = Request.QueryString["High_School_Major_Code"];
            return Json(!db.Mstr_High_School_Major.Any(c => c.High_School_Major_Code == strHighSchoolCode), JsonRequestBehavior.AllowGet);
        }
    }
}
