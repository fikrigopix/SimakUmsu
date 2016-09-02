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
    public class Class_ProgramController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Class_Program
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
            return View(db.Mstr_Class_Program.ToList());
        }

        // GET: Class_Program/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Find(id);
            if (mstr_Class_Program == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Class_Program);
        }

        // GET: Class_Program/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Class_Program/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Class_Prog_Id,Class_Prog_Code,Class_Program_Name,Class_Program_Name_Eng,Order_Id")] Mstr_Class_Program mstr_Class_Program)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Class_Program.Add(mstr_Class_Program);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Class_Program);
        }

        // GET: Class_Program/Edit/5
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
            Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Find(id);
            if (mstr_Class_Program == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Class_Program);
        }

        // POST: Class_Program/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Class_Prog_Id,Class_Prog_Code,Class_Program_Name,Class_Program_Name_Eng,Order_Id")] Mstr_Class_Program mstr_Class_Program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Class_Program).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Class_Program class_program = db.Mstr_Class_Program.Find(mstr_Class_Program.Class_Prog_Id);
                    if (class_program == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Program Kelas telah ada.";
                    return RedirectToAction("Edit", mstr_Class_Program);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Class_Program);
        }

        // GET: Class_Program/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Find(id);
        //    if (mstr_Class_Program == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Class_Program);
        //}

        // POST: Class_Program/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Find(id);
            db.Mstr_Class_Program.Remove(mstr_Class_Program);
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
        public JsonResult IsClasProgCodeExists(string strClasProgCode)
        {
            strClasProgCode = Request.QueryString["Class_Prog_Code"];
            return Json(!db.Mstr_Class_Program.Any(c => c.Class_Prog_Code == strClasProgCode), JsonRequestBehavior.AllowGet);
        }
    }
}
