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
    public class ReligionController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Religion
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
            return View(db.Mstr_Religion.ToList());
        }

        // GET: Religion/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Religion mstr_Religion = db.Mstr_Religion.Find(id);
            if (mstr_Religion == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Religion);
        }

        // GET: Religion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Religion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Religion_Id,Religion_Code,Religion_Name,Order_Id")] Mstr_Religion mstr_Religion)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Religion.Add(mstr_Religion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Religion);
        }

        // GET: Religion/Edit/5
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
            Mstr_Religion mstr_Religion = db.Mstr_Religion.Find(id);
            if (mstr_Religion == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Religion);
        }

        // POST: Religion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Religion_Id,Religion_Code,Religion_Name,Order_Id")] Mstr_Religion mstr_Religion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Religion).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Religion religion = db.Mstr_Religion.Find(mstr_Religion.Religion_Id);
                    if (religion == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Agama telah ada.";
                    return RedirectToAction("Edit", mstr_Religion);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Religion);
        }

        // GET: Religion/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Religion mstr_Religion = db.Mstr_Religion.Find(id);
        //    if (mstr_Religion == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Religion);
        //}

        // POST: Religion/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Religion mstr_Religion = db.Mstr_Religion.Find(id);
            db.Mstr_Religion.Remove(mstr_Religion);
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
        public JsonResult IsReligionCodeExists(string strReligionCode)
        {
            strReligionCode = Request.QueryString["Religion_Code"];
            return Json(!db.Mstr_Religion.Any(c => c.Religion_Code == strReligionCode), JsonRequestBehavior.AllowGet);
        }
    }
}
