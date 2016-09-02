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
    public class FacultyController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Faculty
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
            return View(db.Mstr_Faculty.OrderBy(f => f.Order_Id).ThenBy(f => f.Faculty_Code).ToList());
        }

        // GET: Faculty/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Faculty mstr_Faculty = db.Mstr_Faculty.Find(id);
            if (mstr_Faculty == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Faculty);
        }

        // GET: Faculty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Faculty_Id,Faculty_Code,Faculty_Name,Faculty_Name_Eng,Faculty_Acronym,Order_Id")] Mstr_Faculty mstr_Faculty)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Faculty.Add(mstr_Faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Faculty);
        }

        // GET: Faculty/Edit/5
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
            Mstr_Faculty mstr_Faculty = db.Mstr_Faculty.Find(id);
            if (mstr_Faculty == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Faculty);
        }

        // POST: Faculty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Faculty_Id,Faculty_Code,Faculty_Name,Faculty_Name_Eng,Faculty_Acronym,Order_Id")] Mstr_Faculty mstr_Faculty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Faculty).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Faculty faculty = db.Mstr_Faculty.Find(mstr_Faculty.Faculty_Id);
                    if (faculty == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Fakultas / Nama Fakultas telah ada.";
                    return RedirectToAction("Edit", mstr_Faculty);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Faculty);
        }

        // GET: Faculty/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Faculty mstr_Faculty = db.Mstr_Faculty.Find(id);
        //    if (mstr_Faculty == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Faculty);
        //}

        // POST: Faculty/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Faculty mstr_Faculty = db.Mstr_Faculty.Find(id);
            db.Mstr_Faculty.Remove(mstr_Faculty);
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
        public JsonResult IsFacultyCodeExists(string strFacultyCode)
        {
            strFacultyCode = Request.QueryString["Faculty_Code"];
            return Json(!db.Mstr_Faculty.Any(c => c.Faculty_Code == strFacultyCode), JsonRequestBehavior.AllowGet);
        }
    }
}
