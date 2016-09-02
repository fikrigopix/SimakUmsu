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
    public class CurriculumAppliedController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CurriculumApplied
        public ActionResult Index(short? Department_Id)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            var deptId = Department_Id ?? 0;
            ViewBag.deptId = Department_Id;
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name");

            var mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Where(m => m.Department_Id == deptId).Include(m => m.Mstr_Class_Program).Include(m => m.Mstr_Curriculum).Include(m => m.Mstr_Department);
            return View(mstr_Curriculum_Applied.ToList());
        }

        // GET: CurriculumApplied/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Curriculum_Applied mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Find(id);
            if (mstr_Curriculum_Applied == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Curriculum_Applied);
        }

        // GET: CurriculumApplied/Create
        public ActionResult Create(short? Department_Id)
        {
            if (Department_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Class_Prog_Id = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id).ToList();
            ViewBag.Curriculum_Id = db.Mstr_Curriculum.ToList();
            ViewBag.Department = db.Mstr_Department.Where(dcp => dcp.Department_Id == Department_Id).Single();
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return View();
        }

        // POST: CurriculumApplied/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curiculum_Applied_Id,Department_Id,Curriculum_Id,Class_Prog_Id,Term_Start_Id,Total_Sks_Core,Total_Sks_Elective,Min_Cum_Gpa,Sks_Completion,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Curriculum_Applied mstr_Curriculum_Applied, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Curriculum_Applied.Add(mstr_Curriculum_Applied);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            ViewBag.Class_Prog_Id = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id).ToList();
            ViewBag.Curriculum_Id = db.Mstr_Curriculum.ToList();
            ViewBag.Department = db.Mstr_Department.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id).Single();
            ViewBag.UrlReferrer = UrlReferrer;
            return View(mstr_Curriculum_Applied);
        }

        // GET: CurriculumApplied/Edit/5
        public ActionResult Edit(int? id, string UrlReferrer)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Mstr_Curriculum_Applied mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Find(id);
            if (mstr_Curriculum_Applied == null)
            {
                return HttpNotFound();
            }

            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", mstr_Curriculum_Applied.Class_Prog_Id);
            ViewBag.Curriculum_Id =  new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name", mstr_Curriculum_Applied.Curriculum_Id);
            ViewBag.Department = db.Mstr_Department.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id).Single();
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return View(mstr_Curriculum_Applied);
        }

        // POST: CurriculumApplied/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curiculum_Applied_Id,Department_Id,Curriculum_Id,Class_Prog_Id,Term_Start_Id,Total_Sks_Core,Total_Sks_Elective,Min_Cum_Gpa,Sks_Completion,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Curriculum_Applied mstr_Curriculum_Applied, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Curriculum_Applied).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["shortMessage"] = "Data telah ada.";
                    return RedirectToAction("Edit", new { id = mstr_Curriculum_Applied.Curiculum_Applied_Id, UrlReferrer = UrlReferrer });
                    throw;
                }
                return Redirect(UrlReferrer);
            }
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", mstr_Curriculum_Applied.Class_Prog_Id);
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name", mstr_Curriculum_Applied.Curriculum_Id);
            ViewBag.Department = db.Mstr_Department.Where(dcp => dcp.Department_Id == mstr_Curriculum_Applied.Department_Id).Single();
            ViewBag.UrlReferrer = UrlReferrer;
            return View(mstr_Curriculum_Applied);
        }

        // GET: CurriculumApplied/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Curriculum_Applied mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Find(id);
        //    if (mstr_Curriculum_Applied == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Curriculum_Applied);
        //}

        // POST: CurriculumApplied/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Mstr_Curriculum_Applied mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Find(id);
            db.Mstr_Curriculum_Applied.Remove(mstr_Curriculum_Applied);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return Redirect(UrlReferrer);
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return Redirect(UrlReferrer);
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
        public JsonResult IsDataExists(short? intDepartment_Id, short? intCurriculum_Id, short? intClass_Prog_Id)
        {
            if (Request.QueryString["Department_Id"].Equals("") || Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Curriculum_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intDepartment_Id = Convert.ToInt16(Request.QueryString["Department_Id"]);
                intCurriculum_Id = Convert.ToInt16(Request.QueryString["Curriculum_Id"]);
                intClass_Prog_Id = Convert.ToInt16(Request.QueryString["Class_Prog_Id"]);
                var model = db.Mstr_Curriculum_Applied.Where(ca => (intDepartment_Id.HasValue) ?
                    (ca.Department_Id == intDepartment_Id && ca.Curriculum_Id == intCurriculum_Id && ca.Class_Prog_Id == intClass_Prog_Id) :
                    (ca.Class_Prog_Id == intClass_Prog_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
