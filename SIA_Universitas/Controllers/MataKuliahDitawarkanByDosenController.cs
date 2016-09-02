using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;

namespace SIA_Universitas.Controllers
{
    public class MataKuliahDitawarkanByDosenController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: /MataKuliahDitawarkanByDosen/
        public ActionResult Index()
        {
            var acd_course_lecturer = db.Acd_Course_Lecturer.Include(a => a.Acd_Course).Include(a => a.Emp_Employee);
            return View(acd_course_lecturer.ToList());
        }



        // GET: /MataKuliahDitawarkanByDosen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_course_lecturer = db.Acd_Course_Lecturer.Find(id);
            if (acd_course_lecturer == null)
            {
                return HttpNotFound();
            }
            return View(acd_course_lecturer);
        }

        // GET: /MataKuliahDitawarkanByDosen/Create
        public ActionResult Create()
        {
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code");
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik");
            return View();
        }

        // POST: /MataKuliahDitawarkanByDosen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Course_Lecturer_Id,Employee_Id,Course_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Lecturer acd_course_lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Course_Lecturer.Add(acd_course_lecturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_course_lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_course_lecturer.Employee_Id);
            return View(acd_course_lecturer);
        }

        // GET: /MataKuliahDitawarkanByDosen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_course_lecturer = db.Acd_Course_Lecturer.Find(id);
            if (acd_course_lecturer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_course_lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_course_lecturer.Employee_Id);
            return View(acd_course_lecturer);
        }

        // POST: /MataKuliahDitawarkanByDosen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Course_Lecturer_Id,Employee_Id,Course_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Lecturer acd_course_lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_course_lecturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_course_lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_course_lecturer.Employee_Id);
            return View(acd_course_lecturer);
        }

        // GET: /MataKuliahDitawarkanByDosen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_course_lecturer = db.Acd_Course_Lecturer.Find(id);
            if (acd_course_lecturer == null)
            {
                return HttpNotFound();
            }
            return View(acd_course_lecturer);
        }

        // POST: /MataKuliahDitawarkanByDosen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Acd_Course_Lecturer acd_course_lecturer = db.Acd_Course_Lecturer.Find(id);
            db.Acd_Course_Lecturer.Remove(acd_course_lecturer);
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
    }
}
