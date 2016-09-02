using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;

using PagedList;

namespace SIA_Universitas.Controllers
{
    public class Course_LecturerMDController : Controller
    {
        private SIAEntities db = new SIAEntities();

        /* GET: Course_LecturerMD
        public ActionResult Index()
        {
            var acd_Course_Lecturer = db.Acd_Course_Lecturer.Include(a => a.Acd_Course).Include(a => a.Emp_Employee);
            return View(acd_Course_Lecturer.ToList());
        }*/

        public ActionResult Index(int? halaman, int? rowPerPage)
        {
            var acd_Course_Lecturer = db.Acd_Course_Lecturer.Include(a => a.Acd_Course).Include(a => a.Emp_Employee);

            acd_Course_Lecturer = acd_Course_Lecturer.OrderBy(a => a.Course_Lecturer_Id);

            Session["rowPerPage"] = rowPerPage ?? Session["rowPerPage"];
            int pageSize = (Session["rowPerPage"] == null || rowPerPage < 1) ? 10 : Convert.ToInt32(Session["rowPerPage"]);
            int pageNumber = (halaman == null || halaman < 1) ? 1 : Convert.ToInt32(halaman);

            ViewBag.pageNumber = pageNumber;

            return View(acd_Course_Lecturer.ToPagedList(pageNumber, pageSize));
        }

        // GET: Course_LecturerMD/Details/5
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



        // GET: Course_LecturerMD/Create
        // http://localhost:32244/Course_LecturerMD/Create?Employee_Id=1477
        public ActionResult Create(int? Employee_Id)
        {
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name");
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name");
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => e.Emp_Employee_Status.Description.Contains("Dosen"))
                .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + " [" + e.Nik + "]" })
                .ToList().OrderBy(e => e.employeetext), "Employee_Id", "employeetext");

            /*Employee_Id Selected*/
            if (Employee_Id != null)
            {
                Emp_Employee emp_employee = db.Emp_Employee.Find(Employee_Id);
                ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => e.Emp_Employee_Status.Description.Contains("Dosen"))
                    .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + " [" + e.Nik + "]" })
                    .ToList()
                    .OrderBy(e => e.employeetext), "Employee_Id", "employeetext", emp_employee.Employee_Id);
            }
            else
            {
                ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => e.Emp_Employee_Status.Description.Contains("Dosen"))
                    .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + " [" + e.Nik + "]" })
                    .ToList()
                    .OrderBy(e => e.employeetext), "Employee_Id", "employeetext");
            }

            return View();
        }

        public PartialViewResult PVList(int? id)
        {
            List<Acd_Course_Lecturer> CourseLecturers;

            //if (id != null)
            //{
            CourseLecturers = db.Acd_Course_Lecturer.Where(a => a.Employee_Id == id).Include(a => a.Emp_Employee).ToList();
            //}
            //else
            //{
            //CourseLecturers = db.Acd_Course_Lecturer.ToList();
            //}

            //var Acd_Course_Lecturer = db.Acd_Course_Lecturer.Where(a => a.Employee_Id == id).Include(a => a.Emp_Employee).ToList();
            return PartialView(CourseLecturers);
            //var CourseLecturers = db.Acd_Course_Lecturer.Where(a => a.Employee_Id == 2133).Include(a => a.Emp_Employee).AsEnumerable();
            //return PartialView(CourseLecturers);

        }


        // POST: Course_LecturerMD/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Lecturer_Id,Employee_Id,Course_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Lecturer acd_Course_Lecturer, int? Department_Id)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Course_Lecturer.Add(acd_Course_Lecturer);
                db.SaveChanges();
                return RedirectToAction("Create", "Course_LecturerMD", new { Employee_Id = acd_Course_Lecturer.Employee_Id, Department_Id = Department_Id });
            }

            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name", acd_Course_Lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Full_Name", acd_Course_Lecturer.Employee_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name");
            return View(acd_Course_Lecturer);
        }

        // GET: Course_LecturerMD/Edit/5
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
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name", acd_Course_Lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Full_Name", acd_Course_Lecturer.Employee_Id);
            return View(acd_Course_Lecturer);
        }

        // POST: Course_LecturerMD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name", acd_Course_Lecturer.Course_Id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Full_Name", acd_Course_Lecturer.Employee_Id);
            return View(acd_Course_Lecturer);
        }

        // GET: Course_LecturerMD/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);
            
            /*Del*/
            int intDepartment_Id = acd_Course_Lecturer.Acd_Course.Mstr_Department.Department_Id;
            int intEmployee_Id = acd_Course_Lecturer.Emp_Employee.Employee_Id;
            db.Acd_Course_Lecturer.Remove(acd_Course_Lecturer);
            db.SaveChanges();
            /*Del*/

            if (acd_Course_Lecturer == null)
            {
                return HttpNotFound();
            }
            //return View(acd_Course_Lecturer);
            return RedirectToAction("Create", "Course_LecturerMD", new { Employee_Id = intEmployee_Id, Department_Id = intDepartment_Id });
        }

        // POST: Course_LecturerMD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Acd_Course_Lecturer acd_Course_Lecturer = db.Acd_Course_Lecturer.Find(id);
            db.Acd_Course_Lecturer.Remove(acd_Course_Lecturer);
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

        //http://localhost:32244/Course_LecturerMD/GetDosenByDepartment_Id/3
        public ActionResult GetDosenByDepartment_Id(int id)
        {
            var Emp_Employees = db.Emp_Employee
                .Where(e => e.Department_Id == id)
                .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + "[" + e.Nik + "] " }).ToList()
                .OrderBy(e => e.employeetext);
            return Json(new SelectList(Emp_Employees, "Employee_Id", "employeetext"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDosen()
        {
            var employeeDosens = db.Emp_Employee
                .Where(e => e.Emp_Employee_Status.Description.Contains("Dosen"))
                .Select(e => new { e.Employee_Id, employeetext = e.Full_Name + "[" + e.Nik + "] " }).ToList()
                .OrderBy(e => e.employeetext);
            return Json(new SelectList(employeeDosens, "Employee_Id", "employeetext"), JsonRequestBehavior.AllowGet);
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
