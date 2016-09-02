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
    public class OutReasonController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: OutReason
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
            return View(db.Mstr_Out_Reason.ToList());
        }

        // GET: OutReason/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Out_Reason mstr_Out_Reason = db.Mstr_Out_Reason.Find(id);
            if (mstr_Out_Reason == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Out_Reason);
        }

        // GET: OutReason/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutReason/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Out_Reason_ID,Out_Reason_Code,Description")] Mstr_Out_Reason mstr_Out_Reason)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Out_Reason.Add(mstr_Out_Reason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Out_Reason);
        }

        // GET: OutReason/Edit/5
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
            Mstr_Out_Reason mstr_Out_Reason = db.Mstr_Out_Reason.Find(id);
            if (mstr_Out_Reason == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Out_Reason);
        }

        // POST: OutReason/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Out_Reason_ID,Out_Reason_Code,Description")] Mstr_Out_Reason mstr_Out_Reason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Out_Reason).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Out_Reason Out_Reason = db.Mstr_Out_Reason.Find(mstr_Out_Reason.Out_Reason_ID);
                    if (Out_Reason == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Alasan Keluar telah ada.";
                    return RedirectToAction("Edit", mstr_Out_Reason);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Out_Reason);
        }

        // GET: OutReason/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Out_Reason mstr_Out_Reason = db.Mstr_Out_Reason.Find(id);
        //    if (mstr_Out_Reason == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Out_Reason);
        //}

        // POST: OutReason/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Out_Reason mstr_Out_Reason = db.Mstr_Out_Reason.Find(id);
            db.Mstr_Out_Reason.Remove(mstr_Out_Reason);
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
        public JsonResult IsOutReasonCodeExists(string strOutReasonCode)
        {
            strOutReasonCode = Request.QueryString["Out_Reason_Code"];
            short intOutReasonCode = Convert.ToInt16(strOutReasonCode);
            return Json(!db.Mstr_Out_Reason.Any(c => c.Out_Reason_Code == intOutReasonCode), JsonRequestBehavior.AllowGet);
        }
    }
}
