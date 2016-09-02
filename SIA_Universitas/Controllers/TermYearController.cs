using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace SIA_Universitas.Controllers
{
    public class TermYearController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: TermYear
        public ActionResult Index(short? Entry_Year_Id)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            ViewBag.currentYear = Entry_Year_Id;
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(y => y.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Id", Entry_Year_Id);

            var mstr_Term_Year = db.Mstr_Term_Year.OrderByDescending(ty => ty.Year_Id).Where(ty => ty.Year_Id == Entry_Year_Id).Include(m => m.Mstr_Term).Include(m => m.Mstr_Entry_Year);

            return View(mstr_Term_Year);
        }

        // GET: TermYear/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Term_Year mstr_Term_Year = db.Mstr_Term_Year.Find(id);
            if (mstr_Term_Year == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Term_Year);
        }

        // GET: TermYear/Create
        public ActionResult Create(short id)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            ViewBag.Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Name");
            ViewBag.currentYear = id;
            return View();
        }

        // POST: TermYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Term_Year_Id,Year_Id,Term_Id,Term_Year_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Term_Year mstr_Term_Year)
        {
            string TermName = db.Mstr_Term.Where(t => t.Term_Id == mstr_Term_Year.Term_Id).Select(t => t.Term_Name).First();
            string TermYearId = Convert.ToString(mstr_Term_Year.Year_Id) + Convert.ToString(mstr_Term_Year.Term_Id);
            mstr_Term_Year.Term_Year_Id = Convert.ToInt16(TermYearId);
            mstr_Term_Year.Term_Year_Name = Convert.ToString(mstr_Term_Year.Year_Id) + "/" + TermName;

            if (ModelState.IsValid)
            {
                db.Mstr_Term_Year.Add(mstr_Term_Year);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["shortMessage"] = "Semester Berlaku telah ada.";
                    return RedirectToAction("Create", new { id = mstr_Term_Year.Year_Id });
                    throw;
                }

                return RedirectToAction("Index", new { Entry_Year_Id = mstr_Term_Year.Year_Id });
            }

            ViewBag.Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", mstr_Term_Year.Term_Id);
            ViewBag.Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", mstr_Term_Year.Year_Id);
            return View(mstr_Term_Year);
        }

        // GET: TermYear/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Term_Year mstr_Term_Year = db.Mstr_Term_Year.Find(id);
            if (mstr_Term_Year == null)
            {
                return HttpNotFound();
            }
            ViewBag.Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", mstr_Term_Year.Term_Id);
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", mstr_Term_Year.Year_Id);
            return View(mstr_Term_Year);
        }

        // POST: TermYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Term_Year_Id,Year_Id,Term_Id,Term_Year_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Term_Year mstr_Term_Year)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Term_Year).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Entry_Year_Id = mstr_Term_Year.Year_Id });
            }
            ViewBag.Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", mstr_Term_Year.Term_Id);
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", mstr_Term_Year.Year_Id);
            return View(mstr_Term_Year);
        }

        // GET: TermYear/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Term_Year mstr_Term_Year = db.Mstr_Term_Year.Find(id);
        //    if (mstr_Term_Year == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Term_Year);
        //}

        // POST: TermYear/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Mstr_Term_Year mstr_Term_Year = db.Mstr_Term_Year.Find(id);
            db.Mstr_Term_Year.Remove(mstr_Term_Year);
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
    }
}
