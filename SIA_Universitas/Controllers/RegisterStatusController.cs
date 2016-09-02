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
    public class RegisterStatusController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: RegisterStatus
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
            return View(db.Mstr_Register_Status.ToList());
        }

        // GET: RegisterStatus/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Register_Status mstr_Register_Status = db.Mstr_Register_Status.Find(id);
            if (mstr_Register_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Register_Status);
        }

        // GET: RegisterStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Register_Status_Id,Register_Status_Code,Register_Status_Name,Register_Status_Acronym,Order_Id")] Mstr_Register_Status mstr_Register_Status)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Register_Status.Add(mstr_Register_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Register_Status);
        }

        // GET: RegisterStatus/Edit/5
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
            Mstr_Register_Status mstr_Register_Status = db.Mstr_Register_Status.Find(id);
            if (mstr_Register_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Register_Status);
        }

        // POST: RegisterStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Register_Status_Id,Register_Status_Code,Register_Status_Name,Register_Status_Acronym,Order_Id")] Mstr_Register_Status mstr_Register_Status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Register_Status).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Register_Status register_status = db.Mstr_Register_Status.Find(mstr_Register_Status.Register_Status_Id);
                    if (register_status == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Status Daftar telah ada.";
                    return RedirectToAction("Edit", mstr_Register_Status);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Register_Status);
        }

        // GET: RegisterStatus/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Register_Status mstr_Register_Status = db.Mstr_Register_Status.Find(id);
        //    if (mstr_Register_Status == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Register_Status);
        //}

        // POST: RegisterStatus/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Register_Status mstr_Register_Status = db.Mstr_Register_Status.Find(id);
            db.Mstr_Register_Status.Remove(mstr_Register_Status);
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
        public JsonResult IsRegisterStatusCodeExists(string strRegisterStatusCode)
        {
            strRegisterStatusCode = Request.QueryString["Register_Status_Code"];
            Int32 intRegisterSatusCode = Convert.ToInt32(strRegisterStatusCode);
            return Json(!db.Mstr_Register_Status.Any(c => c.Register_Status_Code == intRegisterSatusCode), JsonRequestBehavior.AllowGet);
        }
    }
}
