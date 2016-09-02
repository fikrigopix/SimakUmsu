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
    public class EducationTypeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: EducationType
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
            return View(db.Mstr_Education_Type.ToList());
        }

        // GET: EducationType/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Education_Type mstr_Education_Type = db.Mstr_Education_Type.Find(id);
            if (mstr_Education_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Education_Type);
        }

        // GET: EducationType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EducationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Education_Type_Id,Education_Type_Code,Education_Type_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Education_Type mstr_Education_Type)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Education_Type.Add(mstr_Education_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Education_Type);
        }

        // GET: EducationType/Edit/5
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
            Mstr_Education_Type mstr_Education_Type = db.Mstr_Education_Type.Find(id);
            if (mstr_Education_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Education_Type);
        }

        // POST: EducationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Education_Type_Id,Education_Type_Code,Education_Type_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Education_Type mstr_Education_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Education_Type).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Education_Type EducationType = db.Mstr_Education_Type.Find(mstr_Education_Type.Education_Type_Id);
                    if (EducationType == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Jenjang Pendidikan telah ada.";
                    return RedirectToAction("Edit", mstr_Education_Type);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Education_Type);
        }

        // GET: EducationType/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Education_Type mstr_Education_Type = db.Mstr_Education_Type.Find(id);
        //    if (mstr_Education_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Education_Type);
        //}

        // POST: EducationType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Education_Type mstr_Education_Type = db.Mstr_Education_Type.Find(id);
            db.Mstr_Education_Type.Remove(mstr_Education_Type);
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
        public JsonResult IsETCodeExists(string strETCode)
        {
            strETCode = Request.QueryString["Education_Type_Code"];
            return Json(!db.Mstr_Education_Type.Any(et => et.Education_Type_Code == strETCode), JsonRequestBehavior.AllowGet);
        }
    }
}
