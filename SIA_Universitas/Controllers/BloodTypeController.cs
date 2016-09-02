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
    public class BloodTypeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: BloodType
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
            return View(db.Mstr_Blood_Type.ToList());
        }

        // GET: BloodType/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Blood_Type mstr_Blood_Type = db.Mstr_Blood_Type.Find(id);
            if (mstr_Blood_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Blood_Type);
        }

        // GET: BloodType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BloodType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Blood_Type_Id,Blood_Code,Blood_Type,Order_Id")] Mstr_Blood_Type mstr_Blood_Type)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Blood_Type.Add(mstr_Blood_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Blood_Type);
        }

        // GET: BloodType/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Blood_Type mstr_Blood_Type = db.Mstr_Blood_Type.Find(id);
            if (mstr_Blood_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Blood_Type);
        }

        // POST: BloodType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Blood_Type_Id,Blood_Code,Blood_Type,Order_Id")] Mstr_Blood_Type mstr_Blood_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Blood_Type).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Country country = db.Mstr_Country.Find(mstr_Blood_Type.Blood_Type_Id);
                    if (country == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Golongan Darah telah ada.";
                    return RedirectToAction("Edit", mstr_Blood_Type);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Blood_Type);
        }

        // GET: BloodType/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Blood_Type mstr_Blood_Type = db.Mstr_Blood_Type.Find(id);
        //    if (mstr_Blood_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Blood_Type);
        //}

        // POST: BloodType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Blood_Type mstr_Blood_Type = db.Mstr_Blood_Type.Find(id);
            db.Mstr_Blood_Type.Remove(mstr_Blood_Type);
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
        public JsonResult IsBloodCodeExists(string strBloodCode)
        {
            strBloodCode = Request.QueryString["Blood_Code"];
            return Json(!db.Mstr_Blood_Type.Any(c => c.Blood_Code == strBloodCode), JsonRequestBehavior.AllowGet);
        }
    }
}
