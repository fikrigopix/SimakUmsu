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
    public class JobCategoryController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: JobCategory
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
            return View(db.Mstr_Job_Category.ToList());
        }

        // GET: JobCategory/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Job_Category mstr_Job_Category = db.Mstr_Job_Category.Find(id);
            if (mstr_Job_Category == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Job_Category);
        }

        // GET: JobCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Job_Category_Id,Job_Category_Code,Job_Category_Name,Order_Id")] Mstr_Job_Category mstr_Job_Category)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Job_Category.Add(mstr_Job_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Job_Category);
        }

        // GET: JobCategory/Edit/5
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
            Mstr_Job_Category mstr_Job_Category = db.Mstr_Job_Category.Find(id);
            if (mstr_Job_Category == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Job_Category);
        }

        // POST: JobCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Job_Category_Id,Job_Category_Code,Job_Category_Name,Order_Id")] Mstr_Job_Category mstr_Job_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Job_Category).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Job_Category job_category = db.Mstr_Job_Category.Find(mstr_Job_Category.Job_Category_Id);
                    if (job_category == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Kategori Pekerjaan telah ada.";
                    return RedirectToAction("Edit", mstr_Job_Category);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Job_Category);
        }

        // GET: JobCategory/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Job_Category mstr_Job_Category = db.Mstr_Job_Category.Find(id);
        //    if (mstr_Job_Category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Job_Category);
        //}

        // POST: JobCategory/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Job_Category mstr_Job_Category = db.Mstr_Job_Category.Find(id);
            db.Mstr_Job_Category.Remove(mstr_Job_Category);
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
        public JsonResult IsJobCategoryCodeExists(string strJobCategoryCode)
        {
            strJobCategoryCode = Request.QueryString["Job_Category_Code"];
            Int32 intJobCategoryCode = Convert.ToInt32(strJobCategoryCode);
            return Json(!db.Mstr_Job_Category.Any(c => c.Job_Category_Code == intJobCategoryCode), JsonRequestBehavior.AllowGet);
        }
    }
}
