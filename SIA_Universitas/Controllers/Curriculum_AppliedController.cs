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
    public class Curriculum_AppliedController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Curriculum_Applied
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

        // GET: Curriculum_Applied/Details/5
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

        // GET: Curriculum_Applied/Create
        public ActionResult Create(short id)
        {
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(x => x.Department_Id == id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name").ToList();
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name");
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == id).FirstOrDefault();

            return View();
        }

        // POST: Curriculum_Applied/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curiculum_Applied_Id,Department_Id,Curriculum_Id,Class_Prog_Id,Term_Start_Id,Total_Sks_Core,Total_Sks_Elective,Min_Cum_Gpa,Sks_Completion")] Mstr_Curriculum_Applied mstr_Curriculum_Applied)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Curriculum_Applied.Add(mstr_Curriculum_Applied);
                db.SaveChanges();
            }

            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(x => x.Department_Id == mstr_Curriculum_Applied.Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", mstr_Curriculum_Applied.Class_Prog_Id).ToList();
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name", mstr_Curriculum_Applied.Curriculum_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Curriculum_Applied.Department_Id).FirstOrDefault();
            return View(mstr_Curriculum_Applied);
        }

        // GET: Curriculum_Applied/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", mstr_Curriculum_Applied.Class_Prog_Id);
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name", mstr_Curriculum_Applied.Curriculum_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Curriculum_Applied.Department_Id).FirstOrDefault();
            return View(mstr_Curriculum_Applied);
        }

        // POST: Curriculum_Applied/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curiculum_Applied_Id,Department_Id,Curriculum_Id,Class_Prog_Id,Term_Start_Id,Total_Sks_Core,Total_Sks_Elective,Min_Cum_Gpa,Sks_Completion")] Mstr_Curriculum_Applied mstr_Curriculum_Applied)
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
                    Mstr_Curriculum_Applied curriculum_applied = db.Mstr_Curriculum_Applied.Find(mstr_Curriculum_Applied.Curiculum_Applied_Id);
                    if (curriculum_applied == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Data Ganda, data kurikulum yang ditambahkan sudah ada diprodi ini.";
                    return RedirectToAction("Edit", mstr_Curriculum_Applied);
                    throw;
                }
                ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", mstr_Curriculum_Applied.Department_Id);
                return RedirectToAction("Index", new { Department_Id = mstr_Curriculum_Applied.Department_Id });
            }
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", mstr_Curriculum_Applied.Class_Prog_Id);
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Name", mstr_Curriculum_Applied.Curriculum_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Curriculum_Applied.Department_Id).FirstOrDefault();
            return View(mstr_Curriculum_Applied);
        }

        // GET: Curriculum_Applied/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Curriculum_Applied/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mstr_Curriculum_Applied mstr_Curriculum_Applied = db.Mstr_Curriculum_Applied.Find(id);
            db.Mstr_Curriculum_Applied.Remove(mstr_Curriculum_Applied);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index", new { Department_Id = mstr_Curriculum_Applied.Department_Id });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", mstr_Curriculum_Applied.Department_Id);
            return RedirectToAction("Index", new { Department_Id = mstr_Curriculum_Applied.Department_Id });
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
        public JsonResult IsClass_Prog_IdExists(int? intDepartment_Id, int? intCurriculum_Id, int? intClass_Prog_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Curriculum_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);
                intCurriculum_Id = Convert.ToInt32(Request.QueryString["Curriculum_Id"]);
                intClass_Prog_Id = Convert.ToInt32(Request.QueryString["Class_Prog_Id"]);
                var model = db.Mstr_Curriculum_Applied.Where(acd => (intDepartment_Id.HasValue) ?
                    (acd.Department_Id == intDepartment_Id && acd.Curriculum_Id == intCurriculum_Id && acd.Class_Prog_Id == intClass_Prog_Id) :
                    (acd.Class_Prog_Id == intClass_Prog_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}
