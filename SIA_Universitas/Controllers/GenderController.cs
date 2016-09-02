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
    public class GenderController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Gender
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
            return View(db.Mstr_Gender.ToList());
        }

        // GET: Gender/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Gender mstr_Gender = db.Mstr_Gender.Find(id);
            if (mstr_Gender == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Gender);
        }

        // GET: Gender/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gender/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gender_Id,Gender_Type,Gender_Type_Eng,Order_Id")] Mstr_Gender mstr_Gender)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Gender.Add(mstr_Gender);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Gender);
        }

        // GET: Gender/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Gender mstr_Gender = db.Mstr_Gender.Find(id);
            if (mstr_Gender == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Gender);
        }

        // POST: Gender/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gender_Id,Gender_Type,Gender_Type_Eng,Order_Id")] Mstr_Gender mstr_Gender)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Gender).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mstr_Gender);
        }

        // GET: Gender/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Gender mstr_Gender = db.Mstr_Gender.Find(id);
        //    if (mstr_Gender == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Gender);
        //}

        // POST: Gender/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Gender mstr_Gender = db.Mstr_Gender.Find(id);
            db.Mstr_Gender.Remove(mstr_Gender);
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
