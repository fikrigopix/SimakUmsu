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
    public class StudentSupervisionController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: StudentSupervision
        public ActionResult Index(string currentFilter, string searchString, short? Department_Id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = Department_Id;

            //dropdown
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id).ToList();

            var acd_Student_Supervision = db.Acd_Student_Supervision.Where(x => x.Acd_Student.Department_Id == Department_Id)                              
                                                .GroupBy(x => x.Employee_Id)
                                                 .Select(x => new
                                                 {
                                                     Employee_Id = x.FirstOrDefault().Employee_Id,
                                                     Nik = x.FirstOrDefault().Emp_Employee.Nik,
                                                     Full_Name = x.FirstOrDefault().Emp_Employee.Full_Name,
                                                     Jml_bim = db.Acd_Student_Supervision.Where(a => a.Employee_Id == x.FirstOrDefault().Employee_Id).Count(),
                                                     Jml_lulus = db.Acd_Student_Supervision.Where(a => a.Employee_Id == x.FirstOrDefault().Employee_Id 
                                                                                                && a.Acd_Student.Acd_Yudisium.Graduate_Date != null ).Count()
                                                 });

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student_Supervision = acd_Student_Supervision.Where(s => s.Nik.Contains(searchString) || s.Full_Name.Contains(searchString));
            }

            List<Vm_Student_Supervision> list_vm_Student_Supervision = new List<Vm_Student_Supervision>();

            foreach (var item in acd_Student_Supervision)
            {
                Vm_Student_Supervision vm_Student_Supervision = new Vm_Student_Supervision();
                vm_Student_Supervision.Employee_Id = item.Employee_Id;
                vm_Student_Supervision.Nik = item.Nik;
                vm_Student_Supervision.Full_Name = item.Full_Name;
                vm_Student_Supervision.Jml_bim = item.Jml_bim;
                vm_Student_Supervision.Jml_lulus = item.Jml_lulus;

                list_vm_Student_Supervision.Add(vm_Student_Supervision);
            }

            list_vm_Student_Supervision = list_vm_Student_Supervision.OrderBy(x => x.Full_Name).ToList(); 

            return View(list_vm_Student_Supervision.ToPagedList(pageNumber, pageSize));
        }

        // GET: StudentSupervision/Create
        public ActionResult Create(string currentFilter, string searchString, int? Employee_Id, short? Department_Id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = Department_Id;
            ViewBag.Employee = Employee_Id;

            ViewBag.nav = db.Mstr_Department.Where(x => x.Department_Id == Department_Id).Select(x => x.Department_Name).Single();

            //dropdown
            ViewBag.Employee_Id = new SelectList(db.Acd_Course_Lecturer
                                                    .Where(x => x.Acd_Course.Department_Id == Department_Id)
                                                    .GroupBy(x => x.Employee_Id).Select(i => i.FirstOrDefault())
                                                    .OrderBy(p => p.Emp_Employee.Full_Name), "Employee_Id", "Emp_Employee.Full_Name", Employee_Id).ToList();

            var acd_Student_Supervision = db.Acd_Student_Supervision
                                            .Where(x => x.Acd_Student.Department_Id == Department_Id && x.Employee_Id == Employee_Id);

            if (!String.IsNullOrEmpty(searchString) )
            {
                acd_Student_Supervision = acd_Student_Supervision
                                            .Where(x => x.Acd_Student.Nim.Contains(searchString) || x.Acd_Student.Full_Name.Contains(searchString));
            }

            acd_Student_Supervision = acd_Student_Supervision.OrderBy(x => x.Acd_Student.Nim);

            return View(acd_Student_Supervision.ToPagedList(pageNumber, pageSize));
        }

        // GET: StudentSupervision/CreateStudentByDeparmentAndDosen
        public ActionResult CreateStudentByDeparmentAndDosen(string currentFilter, string searchString, int? Employee_Id, short? Department_Id, short? Entry_Year_Id,  int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = Department_Id;
            ViewBag.Employee = Employee_Id;
            ViewBag.Entry_Year = Entry_Year_Id;

            ViewBag.nav = db.Mstr_Department.Where(x => x.Department_Id == Department_Id).Select(x => x.Department_Name).Single();
            ViewBag.nav1 = db.Emp_Employee.Where(x => x.Employee_Id == Employee_Id).Select(x => x.Full_Name).Single();

            //dropdown
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year
                                                    .OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Code", Entry_Year_Id).ToList();

            var mhs_out = db.Acd_Student_Out.OrderBy(x => x.Student_Id).Select(x => x.Student_Id);

            var mhs_bimbingan = db.Acd_Student_Supervision
                                            .Where(x => x.Acd_Student.Department_Id == Department_Id)
                                            .Select(x => x.Student_Id);

            var acd_Student = db.Acd_Student
                                            .Where(x => x.Department_Id == Department_Id 
                                                && x.Entry_Year_Id == Entry_Year_Id 
                                                && !mhs_out.Contains(x.Student_Id)
                                                && !mhs_bimbingan.Contains(x.Student_Id));

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student = acd_Student.Where(x => x.Nim.Contains(searchString) || x.Full_Name.Contains(searchString));
            }

            acd_Student = acd_Student.OrderBy(x => x.Nim);

            return View(acd_Student.ToPagedList(pageNumber, pageSize));
        }

        // POST: StudentSupervision/CreateStudentByDeparmentAndDosen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudentByDeparmentAndDosen(IEnumerable<long> checkStudent_IdToAdd, short? Department_Id, [Bind(Include = "Student_Supervision_Id,Student_Id,Employee_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Student_Supervision acd_Student_Supervision)
        {
            if (ModelState.IsValid)
            {
                if (checkStudent_IdToAdd != null)
                {
                    foreach (var i in checkStudent_IdToAdd)
                    {
                        acd_Student_Supervision.Student_Id = i;

                        acd_Student_Supervision.Created_Date = DateTime.Now;

                        db.Acd_Student_Supervision.Add(acd_Student_Supervision);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Create", new { Department_Id = Department_Id, Employee_Id = acd_Student_Supervision.Employee_Id });
        }

        // GET: StudentSupervision/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Supervision acd_Student_Supervision = db.Acd_Student_Supervision.Find(id);
            if (acd_Student_Supervision == null)
            {
                return HttpNotFound();
            }
            return View(acd_Student_Supervision);
        }

        // POST: StudentSupervision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Acd_Student_Supervision acd_Student_Supervision = db.Acd_Student_Supervision.Find(id);
            var Department_Id = acd_Student_Supervision.Acd_Student.Department_Id;
            db.Acd_Student_Supervision.Remove(acd_Student_Supervision);
            db.SaveChanges();
            return RedirectToAction("Create", new { 
                Employee_Id = acd_Student_Supervision.Employee_Id,
                Department_Id = Department_Id
            });
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
