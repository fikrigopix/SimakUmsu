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
    public class CitizenshipController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Citizenship
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
            return View(db.Mstr_Citizenship.ToList());
        }

        // GET: Citizenship/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Citizenship mstr_Citizenship = db.Mstr_Citizenship.Find(id);
            if (mstr_Citizenship == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Citizenship);
        }

        // GET: Citizenship/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Citizenship/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Citizenship_Id,Citizenship_Code,Citizenship_Name,Order_Id")] Mstr_Citizenship mstr_Citizenship)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Citizenship.Add(mstr_Citizenship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Citizenship);
        }

        // GET: Citizenship/Edit/5
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
            Mstr_Citizenship mstr_Citizenship = db.Mstr_Citizenship.Find(id);
            if (mstr_Citizenship == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Citizenship);
        }

        // POST: Citizenship/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Citizenship_Id,Citizenship_Code,Citizenship_Name,Order_Id")] Mstr_Citizenship mstr_Citizenship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Citizenship).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Citizenship citizenship = db.Mstr_Citizenship.Find(mstr_Citizenship.Citizenship_Id);
                    if (citizenship == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Kewarganegaraan telah ada.";
                    return RedirectToAction("Edit", mstr_Citizenship);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Citizenship);
        }

        // GET: Citizenship/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Citizenship mstr_Citizenship = db.Mstr_Citizenship.Find(id);
        //    if (mstr_Citizenship == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Citizenship);
        //}

        // POST: Citizenship/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Citizenship mstr_Citizenship = db.Mstr_Citizenship.Find(id);
            db.Mstr_Citizenship.Remove(mstr_Citizenship);
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
        public JsonResult IsCitizenshipCodeExists(string strCitizenshipCode)
        {
            strCitizenshipCode = Request.QueryString["Citizenship_Code"];
            return Json(!db.Mstr_Citizenship.Any(c => c.Citizenship_Code == strCitizenshipCode), JsonRequestBehavior.AllowGet);
        }
    }
}
