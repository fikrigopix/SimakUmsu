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
    public class EntryYearController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: EntryYear
        public ActionResult Index(int? page, int? rowPerPage)
        {
            if (TempData["addMessage"] != null)
            {
                ViewBag.add = TempData["addMessage"].ToString();
            }
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            var mstr_Entry_Year = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_Entry_Year.ToPagedList(pageNumber, pageSize));
        }

        // GET: EntryYear/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Entry_Year mstr_Entry_Year = db.Mstr_Entry_Year.Find(id);
            if (mstr_Entry_Year == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Entry_Year);
        }

        // GET: EntryYear/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: EntryYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entry_Year_Id,Entry_Year_Code,Entry_Year_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Entry_Year mstr_Entry_Year)
        {
            Mstr_Entry_Year entryYear = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).First();

            mstr_Entry_Year.Entry_Year_Id = Convert.ToInt16(entryYear.Entry_Year_Id + 1);
            mstr_Entry_Year.Entry_Year_Code = mstr_Entry_Year.Entry_Year_Id;
            mstr_Entry_Year.Entry_Year_Name = mstr_Entry_Year.Entry_Year_Id.ToString() + "/" + Convert.ToString(entryYear.Entry_Year_Id + 2);
            mstr_Entry_Year.Created_Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Mstr_Entry_Year.Add(mstr_Entry_Year);
                db.SaveChanges();
                TempData["addMessage"] = "Berhasil menambahkan Tahun Angkatan.";
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        // GET: EntryYear/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Entry_Year mstr_Entry_Year = db.Mstr_Entry_Year.Find(id);
            if (mstr_Entry_Year == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Entry_Year);
        }

        // POST: EntryYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entry_Year_Id,Entry_Year_Code,Entry_Year_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Entry_Year mstr_Entry_Year)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Entry_Year).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mstr_Entry_Year);
        }

        // GET: EntryYear/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Entry_Year mstr_Entry_Year = db.Mstr_Entry_Year.Find(id);
        //    if (mstr_Entry_Year == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Entry_Year);
        //}

        // POST: EntryYear/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Entry_Year mstr_Entry_Year = db.Mstr_Entry_Year.Find(id);
            db.Mstr_Entry_Year.Remove(mstr_Entry_Year);
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
    }
}
