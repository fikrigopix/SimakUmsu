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
    public class MaritalStatusController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: MaritalStatus
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
            return View(db.Mstr_Marital_Status.ToList());
        }

        // GET: MaritalStatus/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Marital_Status mstr_Marital_Status = db.Mstr_Marital_Status.Find(id);
            if (mstr_Marital_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Marital_Status);
        }

        // GET: MaritalStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaritalStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Marital_Status_Id,Marital_Status_Code,Marital_Status_Type,Order_Id")] Mstr_Marital_Status mstr_Marital_Status)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Marital_Status.Add(mstr_Marital_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Marital_Status);
        }

        // GET: MaritalStatus/Edit/5
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
            Mstr_Marital_Status mstr_Marital_Status = db.Mstr_Marital_Status.Find(id);
            if (mstr_Marital_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Marital_Status);
        }

        // POST: MaritalStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Marital_Status_Id,Marital_Status_Code,Marital_Status_Type,Order_Id")] Mstr_Marital_Status mstr_Marital_Status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Marital_Status).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Marital_Status marital_status = db.Mstr_Marital_Status.Find(mstr_Marital_Status.Marital_Status_Id);
                    if (marital_status == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Status Perkawinan telah ada.";
                    return RedirectToAction("Edit", mstr_Marital_Status);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Marital_Status);
        }

        // GET: MaritalStatus/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Marital_Status mstr_Marital_Status = db.Mstr_Marital_Status.Find(id);
        //    if (mstr_Marital_Status == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Marital_Status);
        //}

        // POST: MaritalStatus/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Marital_Status mstr_Marital_Status = db.Mstr_Marital_Status.Find(id);
            db.Mstr_Marital_Status.Remove(mstr_Marital_Status);
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
        public JsonResult IsMaritalStatusCodeExists(string strMaritalStatusCode)
        {
            strMaritalStatusCode = Request.QueryString["Marital_Status_Code"];
            return Json(!db.Mstr_Marital_Status.Any(c => c.Marital_Status_Code == strMaritalStatusCode), JsonRequestBehavior.AllowGet);
        }
    }
}
