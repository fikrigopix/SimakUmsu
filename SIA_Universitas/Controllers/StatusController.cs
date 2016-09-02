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
    public class StatusController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Status
        public ActionResult Index()
        {
            return View(db.Mstr_Status.ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Status mstr_Status = db.Mstr_Status.Find(id);
            if (mstr_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Status);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Status_Id,Status_Code,Status_Name")] Mstr_Status mstr_Status)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Status.Add(mstr_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Status);
        }

        // GET: Status/Edit/5
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
            Mstr_Status mstr_Status = db.Mstr_Status.Find(id);
            if (mstr_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Status_Id,Status_Code,Status_Name")] Mstr_Status mstr_Status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Status).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Status status = db.Mstr_Status.Find(mstr_Status.Status_Id);
                    if (status == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Status telah ada.";
                    return RedirectToAction("Edit", mstr_Status);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Status);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Status mstr_Status = db.Mstr_Status.Find(id);
            if (mstr_Status == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Mstr_Status mstr_Status = db.Mstr_Status.Find(id);
            db.Mstr_Status.Remove(mstr_Status);
            db.SaveChanges();
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
        public JsonResult IsStatusCodeExists(string strStatusCode)
        {
            strStatusCode = Request.QueryString["Status_Code"];
            return Json(!db.Mstr_Status.Any(c => c.Status_Code == strStatusCode), JsonRequestBehavior.AllowGet);
        }
    }
}
