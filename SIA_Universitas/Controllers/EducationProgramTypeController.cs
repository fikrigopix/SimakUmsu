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
    public class EducationProgramTypeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: EducationProgramType
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
            return View(db.Mstr_Education_Program_Type.OrderBy(ept => ept.Education_Prog_Type_Code).ToList());
        }

        // GET: EducationProgramType/Details/5
        //public ActionResult Details(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Education_Program_Type mstr_Education_Program_Type = db.Mstr_Education_Program_Type.Find(id);
        //    if (mstr_Education_Program_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Education_Program_Type);
        //}

        // GET: EducationProgramType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EducationProgramType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Education_Prog_Type_Id,Education_Prog_Type_Code,Program_Name,Program_Name_Eng,Acronym,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Education_Program_Type mstr_Education_Program_Type)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Education_Program_Type.Add(mstr_Education_Program_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Education_Program_Type);
        }

        // GET: EducationProgramType/Edit/5
        public ActionResult Edit(short? id)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Education_Program_Type mstr_Education_Program_Type = db.Mstr_Education_Program_Type.Find(id);
            if (mstr_Education_Program_Type == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Education_Program_Type);
        }

        // POST: EducationProgramType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Education_Prog_Type_Id,Education_Prog_Type_Code,Program_Name,Program_Name_Eng,Acronym,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Education_Program_Type mstr_Education_Program_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Education_Program_Type).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["shortMessage"] = "Kode Strata Pendidikan telah ada.";
                    return RedirectToAction("Edit", mstr_Education_Program_Type);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Education_Program_Type);
        }

        // GET: EducationProgramType/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Education_Program_Type mstr_Education_Program_Type = db.Mstr_Education_Program_Type.Find(id);
        //    if (mstr_Education_Program_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Education_Program_Type);
        //}

        // POST: EducationProgramType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Education_Program_Type mstr_Education_Program_Type = db.Mstr_Education_Program_Type.Find(id);
            db.Mstr_Education_Program_Type.Remove(mstr_Education_Program_Type);
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
        public JsonResult IsDataExists(short? intEducationProgTypeCode)
        {
            intEducationProgTypeCode = Convert.ToInt16(Request.QueryString["Education_Prog_Type_Code"]);
            return Json(!db.Mstr_Education_Program_Type.Any(c => c.Education_Prog_Type_Code == intEducationProgTypeCode), JsonRequestBehavior.AllowGet);
        }
    }
}
