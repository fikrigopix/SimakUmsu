using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SIA_Universitas.Models;
using System.Data.Entity.Infrastructure;

namespace SIA_Universitas.Controllers
{
    public class OfferedCourseController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: OfferedCourse
        public ActionResult Index(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id,
                                  short? currentTermYear, short? currentDept, short? currentClassProg, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; } else { searchString = currentFilter; }
            if (Term_Year_Id != null) { page = 1; } else { Term_Year_Id = currentTermYear; }
            if (Department_Id != null) { page = 1; } else { Department_Id = currentDept; }
            if (Class_Prog_Id != null) { page = 1; } else { Class_Prog_Id = currentClassProg; }

            //if (TempData["shortMessage"] != null) { ViewBag.message = TempData["shortMessage"].ToString(); }
            if (TempData["gagalHapus"] != null) { ViewBag.gagalHapus = TempData["gagalHapus"].ToString(); }
            if (TempData["berhasilHapus"] != null) { ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString(); }

            var CurriculumEntryYear = db.Acd_Curriculum_Entry_Year.Where(cey => cey.Term_Year_Id == Term_Year_Id && cey.Department_Id == Department_Id && cey.Class_Prog_Id == Class_Prog_Id).ToList();
            if (CurriculumEntryYear.Count() == 0)
            {
                ViewBag.messageCey = "Kurikulum Angkatan belum disetting, Silahkan setting terlebih dahulu."; 
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentTermYear = Term_Year_Id;
            ViewBag.CurrentDept = Department_Id;
            ViewBag.CurrentClassProg = Class_Prog_Id;

            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", Class_Prog_Id);
            //ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => c.Department_Id == Department_Id).OrderBy(c => c.Course_Name), "Course_Id", "NameCode");

            var acd_Offered_Course = db.Acd_Offered_Course.Where(oc => oc.Term_Year_Id == Term_Year_Id && oc.Department_Id == Department_Id && oc.Class_Prog_Id == Class_Prog_Id).Include(a => a.Acd_Course).Include(a => a.Mstr_Class).Include(a => a.Mstr_Class_Program).Include(a => a.Mstr_Department).Include(a => a.Mstr_Term_Year);
            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Offered_Course = acd_Offered_Course.Where(oc => oc.Acd_Course.Course_Name.Contains(searchString));
            }
            acd_Offered_Course = acd_Offered_Course.OrderBy(oc => oc.Acd_Course.Course_Name).ThenBy(oc => oc.Class_Id);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(acd_Offered_Course.ToPagedList(pageNumber, pageSize));
        }

        // GET: OfferedCourse/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(id);
            if (acd_Offered_Course == null)
            {
                return HttpNotFound();
            }
            return View(acd_Offered_Course);
        }

        // GET: OfferedCourse/Create
        public ActionResult Create(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? Course_Id, string UrlReferrer)
        {
            var acd_Offered_Course = db.Acd_Offered_Course.Where(oc => oc.Term_Year_Id == Term_Year_Id && oc.Course_Id == Course_Id && oc.Class_Prog_Id == Class_Prog_Id).ToList();
            var exceptionClass = db.Acd_Offered_Course.Where(oc => oc.Term_Year_Id == Term_Year_Id && oc.Course_Id == Course_Id && oc.Class_Prog_Id == Class_Prog_Id).Select(oc => oc.Class_Id).ToList();
            var course = db.Acd_Course_Curriculum.Where(cc => cc.Department_Id == Department_Id).Select(cc => cc.Course_Id).ToList();

            ViewBag.Class_Id = new SelectList(db.Mstr_Class.Where(c => !exceptionClass.Contains(c.Class_Id)), "Class_Id", "Class_Name");
            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => course.Contains(c.Course_Id)), "Course_Id", "Course_Name", Course_Id);
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).FirstOrDefault();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).FirstOrDefault();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).FirstOrDefault();
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            return View(acd_Offered_Course);
        }

        // POST: OfferedCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Offered_Course_id,Term_Year_Id,Department_Id,Class_Prog_Id,Course_Id,Class_Id, Classes,Total_Meeting,Class_Capacity,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course acd_Offered_Course, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < acd_Offered_Course.Classes.Length; i++)
                {
                    acd_Offered_Course.Class_Id = acd_Offered_Course.Classes[i];
                    db.Acd_Offered_Course.Add(acd_Offered_Course);
                    db.SaveChanges();
                }
                return RedirectToAction("Create", new { Term_Year_Id = acd_Offered_Course.Term_Year_Id, Department_Id = acd_Offered_Course.Department_Id, Class_Prog_Id = acd_Offered_Course.Class_Prog_Id, Course_Id = acd_Offered_Course.Course_Id, UrlReferrer = UrlReferrer });
            }

            var acd_Offered_Courses = db.Acd_Offered_Course.Where(oc => oc.Term_Year_Id == acd_Offered_Course.Term_Year_Id && oc.Course_Id == acd_Offered_Course.Course_Id).ToList();
            var exceptionClass = db.Acd_Offered_Course.Where(oc => oc.Term_Year_Id == acd_Offered_Course.Term_Year_Id && oc.Course_Id == acd_Offered_Course.Course_Id && oc.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id).Select(oc => oc.Class_Id).ToList();

            ViewBag.Class_Id = new SelectList(db.Mstr_Class.Where(c => !exceptionClass.Contains(c.Class_Id)), "Class_Id", "Class_Name");
            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => c.Department_Id == acd_Offered_Course.Department_Id), "Course_Id", "Course_Name", acd_Offered_Course.Course_Id);
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id).FirstOrDefault();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Offered_Course.Department_Id).FirstOrDefault();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Offered_Course.Term_Year_Id).FirstOrDefault();
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return View(acd_Offered_Courses);
        }

        // GET: OfferedCourse/EditKelas/5
        public ActionResult EditKelas(int? id)
        {
            string UrlReferrer = null;
            if (TempData["UrlReferrer"] != null) { UrlReferrer = TempData["UrlReferrer"].ToString(); }
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(id);
            if (acd_Offered_Course == null)
            {
                return HttpNotFound();
            }

            List<short> exceptionKelas = db.Acd_Offered_Course.Where(oc => oc.Course_Id == acd_Offered_Course.Course_Id &&
                                                                 oc.Term_Year_Id == acd_Offered_Course.Term_Year_Id &&
                                                                 oc.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id &&
                                                                 oc.Class_Id != acd_Offered_Course.Class_Id).Select(oc => oc.Class_Id).ToList();

            ViewBag.Class_Id = new SelectList(db.Mstr_Class.Where(c => !exceptionKelas.Contains(c.Class_Id)), "Class_Id", "Class_Name", acd_Offered_Course.Class_Id);
            ViewBag.checkPeserta = db.Acd_Student_Krs.Count(sk => sk.Term_Year_Id == acd_Offered_Course.Term_Year_Id &&
                                                            sk.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id &&
                                                            sk.Course_Id == acd_Offered_Course.Course_Id &&
                                                            sk.Class_Id == acd_Offered_Course.Class_Id);
            return View(acd_Offered_Course);
        }

        // POST: OfferedCourse/EditKelas/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKelas([Bind(Include = "Offered_Course_id,Term_Year_Id,Department_Id,Class_Prog_Id,Course_Id,Class_Id,Classes,Total_Meeting,Class_Capacity,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course acd_Offered_Course, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Offered_Course).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["shortMessage"] = "Data telah ada.";
                    TempData["UrlReferrer"] = UrlReferrer;
                    return RedirectToAction("EditKelas", acd_Offered_Course);
                    throw;
                }
                //return RedirectToAction("Index", new { Term_Year_Id = acd_Offered_Course.Term_Year_Id, Department_Id = acd_Offered_Course.Department_Id, Class_Prog_Id = acd_Offered_Course.Class_Prog_Id });
                return Redirect(UrlReferrer);
            }

            List<short> exceptionKelas = db.Acd_Offered_Course.Where(oc => oc.Course_Id == acd_Offered_Course.Course_Id &&
                                                                 oc.Term_Year_Id == acd_Offered_Course.Term_Year_Id &&
                                                                 oc.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id).Select(oc => oc.Class_Id).ToList();

            ViewBag.Class_Id = new SelectList(db.Mstr_Class.Where(c => !exceptionKelas.Contains(c.Class_Id)), "Class_Id", "Class_Name", acd_Offered_Course.Class_Id);
            ViewBag.checkPeserta = db.Acd_Student_Krs.Count(sk => sk.Term_Year_Id == acd_Offered_Course.Term_Year_Id &&
                                                            sk.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id &&
                                                            sk.Course_Id == acd_Offered_Course.Course_Id &&
                                                            sk.Class_Id == acd_Offered_Course.Class_Id);
            return View(acd_Offered_Course);
        }

        // GET: OfferedCourse/EditDosen/5
        public ActionResult EditDosen(int? id, string UrlReferrer)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(id);
            if (acd_Offered_Course == null)
            {
                return HttpNotFound();
            }

            List<int> exceptionList = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == id).Select(ocl => ocl.Employee_Id).ToList();

            ViewBag.Matakuliah = acd_Offered_Course.Acd_Course.Course_Name + "(" + acd_Offered_Course.Acd_Course.Course_Code + ")";
            ViewBag.Kelas = acd_Offered_Course.Mstr_Class.Class_Name;
            ViewBag.Class_Prog_Id = acd_Offered_Course.Class_Prog_Id;
            ViewBag.Department_Id = acd_Offered_Course.Department_Id;
            ViewBag.Term_Year_Id = acd_Offered_Course.Term_Year_Id;
            ViewBag.Employee_Id = new SelectList(db.Acd_Department_Lecturer.Where(dl => !exceptionList.Contains(dl.Employee_Id) && dl.Department_Id == acd_Offered_Course.Department_Id), "Emp_Employee.Employee_Id", "Emp_Employee.Full_Name");
            ViewBag.Offered_Course_id = id;

            var acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == id).OrderBy(ocl => ocl.Order_Id);
            //ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Offered_Course.Course_Id);
            //ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Offered_Course.Class_Id);

            return View(acd_Offered_Course_Lecturer.ToList());
        }

        // POST: OfferedCourse/EditDosen/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDosen([Bind(Include = "Acd_Offered_Course_Lecturer1,Offered_Course_id,Employee_Id, Employees,Sks_Weight,Order_Id,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer, string UrlReferrer)
        {
            var acd_Offered_Course_Lecturers = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == acd_Offered_Course_Lecturer.Offered_Course_id).OrderBy(ocl => ocl.Order_Id);
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Where(oc => oc.Offered_Course_id == acd_Offered_Course_Lecturer.Offered_Course_id).First();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < acd_Offered_Course_Lecturer.Employees.Length; i++)
                {
                    short lastOrder = 0;
                    if (acd_Offered_Course_Lecturers.ToList().Count() != 0)
                    {
                        lastOrder = acd_Offered_Course_Lecturers.Max(ocl => ocl.Order_Id);
                    }
                    acd_Offered_Course_Lecturer.Employee_Id = acd_Offered_Course_Lecturer.Employees[i];
                    acd_Offered_Course_Lecturer.Order_Id = Convert.ToInt16(lastOrder + 1);
                    db.Acd_Offered_Course_Lecturer.Add(acd_Offered_Course_Lecturer);
                    db.SaveChanges();
                }
                return RedirectToAction("EditDosen", new { id = acd_Offered_Course_Lecturer.Offered_Course_id, UrlReferrer = UrlReferrer });
            }
            List<int> exceptionList = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == acd_Offered_Course.Offered_Course_id).Select(ocl => ocl.Employee_Id).ToList();

            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            ViewBag.Matakuliah = acd_Offered_Course.Acd_Course.Course_Name + "(" + acd_Offered_Course.Acd_Course.Course_Code + ")";
            ViewBag.Kelas = acd_Offered_Course.Mstr_Class.Class_Name;
            ViewBag.Class_Prog_Id = acd_Offered_Course.Class_Prog_Id;
            ViewBag.Department_Id = acd_Offered_Course.Department_Id;
            ViewBag.Term_Year_Id = acd_Offered_Course.Term_Year_Id;
            ViewBag.Employee_Id = new SelectList(db.Acd_Department_Lecturer.Where(dl => !exceptionList.Contains(dl.Employee_Id)), "Emp_Employee.Employee_Id", "Emp_Employee.Full_Name");
            ViewBag.Offered_Course_id = acd_Offered_Course_Lecturer.Offered_Course_id;

            return View(acd_Offered_Course_Lecturers);
        }

        // GET: OfferedCourse/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(id);
        //    if (acd_Offered_Course == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Offered_Course);
        //}

        // POST: OfferedCourse/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string UrlReferrer)
        {
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(id);
            db.Acd_Offered_Course.Remove(acd_Offered_Course);
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

            //RedirectToAction("Index", new { Term_Year_Id = Convert.ToInt16(ViewData["ty"]) });
            return Redirect(UrlReferrer);
        }

        public ActionResult DeleteDosen(int id, string UrlReferrer)
        {
            Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Find(id);
            int x = db.Acd_Offered_Course.Where(oc => oc.Offered_Course_id == acd_Offered_Course_Lecturer.Offered_Course_id).Select(oc => oc.Offered_Course_id).First();
            db.Acd_Offered_Course_Lecturer.Remove(acd_Offered_Course_Lecturer);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("EditDosen", new { id = x, UrlReferrer = UrlReferrer });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("EditDosen", new { id = x, UrlReferrer = UrlReferrer });
            //return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
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
        public JsonResult IsDataExists(int? intTerm_Year_Id, int? intCourse_Id, int? intClass_Id)
        {

            if (Request.QueryString["Class_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Course_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intTerm_Year_Id = Convert.ToInt32(Request.QueryString["Term_Year_Id"]);
                intCourse_Id = Convert.ToInt32(Request.QueryString["Course_Id"]);
                intClass_Id = Convert.ToInt32(Request.QueryString["Class_Id"]);
                var model = db.Acd_Offered_Course.Where(oc => (intTerm_Year_Id.HasValue) ?
                    (oc.Class_Id == intClass_Id && oc.Term_Year_Id == intTerm_Year_Id && oc.Course_Id == intCourse_Id) :
                    (oc.Class_Id == intClass_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
