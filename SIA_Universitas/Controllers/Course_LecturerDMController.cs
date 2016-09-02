﻿using System;
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
    public class Course_LecturerDMController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Course_LecturerDM
        public ActionResult Index()
        {
            var acd_Course_Lecturer = db.Acd_Course_Lecturer.Include(a => a.Acd_Course);
            return View(acd_Course_Lecturer.ToList());
        }

        // GET: Course_LecturerDM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);
            if (acd_Course_Lecturer == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Lecturer);
        }

        // GET: Course_LecturerDM/Create
        //public ActionResult Create()
        //{
        //    //ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name"); -> Generated ByJson
        //    ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name");
        //    ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => e.Emp_Employee_Status.Description.Contains("Dosen")).Select(e => new { e.Employee_Id, employeetext = e.Full_Name + " [" + e.Nik + "]" }).ToList().OrderBy(e => e.employeetext), "Employee_Id", "employeetext");
        //    return View();
        //}

        // GET: Course_LecturerDM/Create
        public ActionResult Create(short? Department_Id, int? Course_Id)
        {
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name");
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => e.Emp_Employee_Status.Description.Contains("Dosen"))
                .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + " [" + e.Nik + "]" })
                .ToList()
                .OrderBy(e => e.employeetext), "Employee_Id", "employeetext"); 

            /*Course_Id Selected*/
            if (Course_Id != null)
            {
                Acd_Course acd_course = db.Acd_Course.Find(Course_Id);
                ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => c.Department_Id == Department_Id)
                    .Select(c => new {c.Course_Id, coursetext = c.Course_Name + " [" + c.Course_Code + "] " })
                    .ToList()
                    .OrderBy(c => c.coursetext), "Course_Id", "coursetext", acd_course.Course_Id);
            }
            else
            {
                ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => c.Department_Id == Department_Id)
                    .Select(c => new { c.Course_Id, coursetext = c.Course_Name + " [" + c.Course_Code + "] " })
                    .ToList()
                    .OrderBy(c => c.coursetext), "Course_Id", "coursetext");
            }
            return View();
        }

        public PartialViewResult PVList(int id)
        {
            List<Acd_Course_Lecturer> CourseLecturers;
            CourseLecturers = db.Acd_Course_Lecturer.Where(l => l.Course_Id == id).Include(l => l.Acd_Course).ToList();
            return PartialView(CourseLecturers);
        }

        // POST: Course_LecturerDM/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Lecturer_Id,Employee_Id,Course_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Lecturer acd_Course_Lecturer, int? Department_Id)
        {
            if (ModelState.IsValid)
            {
                //int intDepartment_Id = acd_Course_Lecturer.Acd_Course.Mstr_Department.Department_Id;
                int intCourse_Id = acd_Course_Lecturer.Course_Id;
                db.Acd_Course_Lecturer.Add(acd_Course_Lecturer);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Create", "Course_LecturerDM", new { Department_Id = Department_Id, Course_Id = intCourse_Id });
            }

            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Course_Lecturer.Course_Id);
            
            return View(acd_Course_Lecturer);
        }

        // GET: Course_LecturerDM/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);
            if (acd_Course_Lecturer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Course_Lecturer.Course_Id);
            return View(acd_Course_Lecturer);
        }

        // POST: Course_LecturerDM/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Lecturer_Id,Employee_Id,Course_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Lecturer acd_Course_Lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Course_Lecturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Course_Lecturer.Course_Id);
            return View(acd_Course_Lecturer);
        }

        // GET: Course_LecturerDM/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);

            /*Del*/
            int intDepartment_Id = acd_Course_Lecturer.Acd_Course.Mstr_Department.Department_Id;
            int intCourse_Id = acd_Course_Lecturer.Acd_Course.Course_Id;
            db.Acd_Course_Lecturer.Remove(acd_Course_Lecturer);
            db.SaveChanges();
            /*Del*/

            if (acd_Course_Lecturer == null)
            {
                return HttpNotFound();
            }
            //return View(acd_Course_Lecturer);
            return RedirectToAction("Create", "Course_LecturerDM", new { Department_Id = intDepartment_Id, Course_Id = intCourse_Id });
        }

        // POST: Course_LecturerDM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed(int id)
        {
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);
            db.Acd_Course_Lecturer.Remove(acd_Course_Lecturer);
            db.SaveChanges();
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult AutoComplete(string term)
        {
            var result = (from r in db.Mstr_Department
                          where r.Department_Name.ToLower().Contains(term.ToLower())
                          select new { r.Department_Name, r.Department_Id }).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAcd_CourseBy_Department_Id(int id)
        {
            var acd_courses = db.Acd_Course
                .Where(c => c.Department_Id == id)
                .Select(c => new { c.Course_Id, coursetext = c.Course_Name + " [" + c.Course_Code + "] " }).ToList()
                .OrderBy(c => c.coursetext);
            return Json(new SelectList(acd_courses, "Course_Id", "coursetext"), JsonRequestBehavior.AllowGet);
        }
    }


}
