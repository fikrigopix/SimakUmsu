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
    public class CurriculumTypeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CurriculumType
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
            return View(db.Mstr_Curriculum_Type.ToList());
        }

        // GET: CurriculumType/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Curriculum_Type mstr_Curriculum_Type = db.Mstr_Curriculum_Type.Find(id);
            if (mstr_Curriculum_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Curriculum_Type);
        }

        // GET: CurriculumType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurriculumType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curriculum_Type_Id,Curriculum_Type_Code,Curriculum_Type_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Curriculum_Type mstr_Curriculum_Type)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Curriculum_Type.Add(mstr_Curriculum_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Curriculum_Type);
        }

        // GET: CurriculumType/Edit/5
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
            Mstr_Curriculum_Type mstr_Curriculum_Type = db.Mstr_Curriculum_Type.Find(id);
            if (mstr_Curriculum_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Curriculum_Type);
        }

        // POST: CurriculumType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curriculum_Type_Id,Curriculum_Type_Code,Curriculum_Type_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Curriculum_Type mstr_Curriculum_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Curriculum_Type).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Curriculum_Type CurriculumType = db.Mstr_Curriculum_Type.Find(mstr_Curriculum_Type.Curriculum_Type_Id);
                    if (CurriculumType == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Jenis Kurikulum telah ada.";
                    return RedirectToAction("Edit", mstr_Curriculum_Type);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Curriculum_Type);
        }

        // GET: CurriculumType/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Curriculum_Type mstr_Curriculum_Type = db.Mstr_Curriculum_Type.Find(id);
        //    if (mstr_Curriculum_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Curriculum_Type);
        //}

        // POST: CurriculumType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Curriculum_Type mstr_Curriculum_Type = db.Mstr_Curriculum_Type.Find(id);
            db.Mstr_Curriculum_Type.Remove(mstr_Curriculum_Type);
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
        public JsonResult IsCTCodeExists(string strCTCode)
        {
            strCTCode = Request.QueryString["Curriculum_Type_Code"];
            return Json(!db.Mstr_Curriculum_Type.Any(ct => ct.Curriculum_Type_Code == strCTCode), JsonRequestBehavior.AllowGet);
        }
    }
}
