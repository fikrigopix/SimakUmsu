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
    public class GraduatePredicateController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: GraduatePredicate
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
            return View(db.Mstr_Graduate_Predicate.ToList());
        }

        // GET: GraduatePredicate/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Graduate_Predicate mstr_Graduate_Predicate = db.Mstr_Graduate_Predicate.Find(id);
            if (mstr_Graduate_Predicate == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Graduate_Predicate);
        }

        // GET: GraduatePredicate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GraduatePredicate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Graduate_Predicate_Id,Graduate_Predicate_Code,Predicate_Name,Predicate_Name_Eng,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Graduate_Predicate mstr_Graduate_Predicate)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Graduate_Predicate.Add(mstr_Graduate_Predicate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Graduate_Predicate);
        }

        // GET: GraduatePredicate/Edit/5
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
            Mstr_Graduate_Predicate mstr_Graduate_Predicate = db.Mstr_Graduate_Predicate.Find(id);
            if (mstr_Graduate_Predicate == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Graduate_Predicate);
        }

        // POST: GraduatePredicate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Graduate_Predicate_Id,Graduate_Predicate_Code,Predicate_Name,Predicate_Name_Eng,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Graduate_Predicate mstr_Graduate_Predicate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Graduate_Predicate).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Graduate_Predicate GraduatePredicate = db.Mstr_Graduate_Predicate.Find(mstr_Graduate_Predicate.Graduate_Predicate_Id);
                    if (GraduatePredicate == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Predikat Lulus telah ada.";
                    return RedirectToAction("Edit", mstr_Graduate_Predicate);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Graduate_Predicate);
        }

        // GET: GraduatePredicate/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Graduate_Predicate mstr_Graduate_Predicate = db.Mstr_Graduate_Predicate.Find(id);
        //    if (mstr_Graduate_Predicate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Graduate_Predicate);
        //}

        // POST: GraduatePredicate/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Graduate_Predicate mstr_Graduate_Predicate = db.Mstr_Graduate_Predicate.Find(id);
            db.Mstr_Graduate_Predicate.Remove(mstr_Graduate_Predicate);
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
        public JsonResult IsGPCodeExists(string strGPCode)
        {
            strGPCode = Request.QueryString["Graduate_Predicate_Code"];
            Int32 intGPCode = Convert.ToInt32(strGPCode);
            return Json(!db.Mstr_Graduate_Predicate.Any(gp => gp.Graduate_Predicate_Code == intGPCode), JsonRequestBehavior.AllowGet);
        }
    }
}
