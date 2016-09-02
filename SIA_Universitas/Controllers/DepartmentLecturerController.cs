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
using System.Data.Entity.Infrastructure;

namespace SIA_Universitas.Controllers
{
    public class DepartmentLecturerController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: DepartmentLecturer
        public ActionResult Index(short? Department_Id, short? currentDept, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (Department_Id != null)
            {
                page = 1;
            }
            else
            {
                Department_Id = currentDept;
            }
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentDept = Department_Id;

            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);
            var exceptionEmployees = db.Acd_Department_Lecturer.Where(dl => dl.Department_Id == Department_Id).Select(dl => dl.Employee_Id).ToList();
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e => !exceptionEmployees.Contains(e.Employee_Id)), "Employee_Id", "Full_Name");

            var acd_Department_Lecturer = db.Acd_Department_Lecturer.Where(dl => dl.Department_Id == Department_Id).Include(a => a.Emp_Employee).Include(a => a.Mstr_Department);
            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Department_Lecturer = acd_Department_Lecturer.Where(s => s.Emp_Employee.Full_Name.Contains(searchString));
            }
            acd_Department_Lecturer = acd_Department_Lecturer.OrderBy(s => s.Emp_Employee.Full_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(acd_Department_Lecturer.ToPagedList(pageNumber, pageSize));
        }

        // GET: DepartmentLecturer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Department_Lecturer acd_Department_Lecturer = db.Acd_Department_Lecturer.Find(id);
            if (acd_Department_Lecturer == null)
            {
                return HttpNotFound();
            }
            return View(acd_Department_Lecturer);
        }

        // GET: DepartmentLecturer/Create
        //public ActionResult Create(short Department_Id)
        //{
        //    var exceptionEmployees = db.Acd_Department_Lecturer.Where(dl => dl.Department_Id == Department_Id).Select(dl => dl.Employee_Id).ToList();
        //    ViewBag.Employee_Id = new SelectList(db.Emp_Employee.Where(e=>!exceptionEmployees.Contains(e.Employee_Id)), "Employee_Id", "Full_Name");
        //    ViewBag.CurrentDept = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).FirstOrDefault();
        //    ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
        //    //ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code");
        //    return View();
        //}

        // POST: DepartmentLecturer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_Lecturer_Id,Department_Id,Employee_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Department_Lecturer acd_Department_Lecturer, int[] Employees)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (Employees == null)
            {
                TempData["shortMessage"] = "Anda Belum Memilih Dosen.";
                return Redirect(UrlReferrer);
            }
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Employees.Length; i++)
                {
                    acd_Department_Lecturer.Employee_Id = Employees[i];
                    db.Acd_Department_Lecturer.Add(acd_Department_Lecturer);
                    db.SaveChanges();
                }
                return Redirect(UrlReferrer);
            }

            //ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_Department_Lecturer.Employee_Id);
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", acd_Department_Lecturer.Department_Id);
            return View(acd_Department_Lecturer);
        }

        // GET: DepartmentLecturer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Department_Lecturer acd_Department_Lecturer = db.Acd_Department_Lecturer.Find(id);
            if (acd_Department_Lecturer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_Department_Lecturer.Employee_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code", acd_Department_Lecturer.Department_Id);
            return View(acd_Department_Lecturer);
        }

        // POST: DepartmentLecturer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_Lecturer_Id,Department_Id,Employee_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Department_Lecturer acd_Department_Lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Department_Lecturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_Department_Lecturer.Employee_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code", acd_Department_Lecturer.Department_Id);
            return View(acd_Department_Lecturer);
        }

        // GET: DepartmentLecturer/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Department_Lecturer acd_Department_Lecturer = db.Acd_Department_Lecturer.Find(id);
        //    if (acd_Department_Lecturer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Department_Lecturer);
        //}

        // POST: DepartmentLecturer/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Acd_Department_Lecturer acd_Department_Lecturer = db.Acd_Department_Lecturer.Find(id);
            db.Acd_Department_Lecturer.Remove(acd_Department_Lecturer);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            //return RedirectToAction("Index");
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
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
