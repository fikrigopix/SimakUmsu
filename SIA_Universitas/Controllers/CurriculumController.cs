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
    public class CurriculumController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Curriculum
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
            return View(db.Mstr_Curriculum.ToList());
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Curriculum mstr_Curriculum = db.Mstr_Curriculum.Find(id);
            if (mstr_Curriculum == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Curriculum);
        }

        // GET: Curriculum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curriculum_Id,Curriculum_Code,Curriculum_Name,Order_Id")] Mstr_Curriculum mstr_Curriculum)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Curriculum.Add(mstr_Curriculum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Curriculum);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["errorCurriculum_Code"] != null)
            {
                ViewBag.errorCurriculum_Code = TempData["errorCurriculum_Code"].ToString();
            }
            Mstr_Curriculum mstr_Curriculum = db.Mstr_Curriculum.Find(id);
            if (mstr_Curriculum == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Curriculum);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curriculum_Id,Curriculum_Code,Curriculum_Name,Order_Id")] Mstr_Curriculum mstr_Curriculum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Curriculum).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Curriculum Curriculum_Code = db.Mstr_Curriculum.Find(mstr_Curriculum.Curriculum_Id);
                    if (Curriculum_Code == null)
                    {
                        return HttpNotFound();
                    }

                    TempData["errorCurriculum_Code"] = "Kode Kurikulum telah ada.";
                    return RedirectToAction("Edit", mstr_Curriculum);
                    throw;
                }
                
                return RedirectToAction("Index");
            }
            return View(mstr_Curriculum);
        }

        // GET: Curriculum/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Curriculum mstr_Curriculum = db.Mstr_Curriculum.Find(id);
        //    if (mstr_Curriculum == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Curriculum);
        //}

        // POST: Curriculum/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Curriculum mstr_Curriculum = db.Mstr_Curriculum.Find(id);
            db.Mstr_Curriculum.Remove(mstr_Curriculum);
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
        public JsonResult IsCurriculumCodeExists(string strCurriculumCode)
        {
            strCurriculumCode = Request.QueryString["Curriculum_Code"];
            return Json(!db.Mstr_Curriculum.Any(c => c.Curriculum_Code == strCurriculumCode), JsonRequestBehavior.AllowGet);
        }
    }
}
