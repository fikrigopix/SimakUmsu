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

namespace SIA_Universitas.Controllers
{
    public class OfferedCourseSchedController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: OfferedCourseSched
        public ActionResult Index(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id,
                                  short? currentTermYear, short? currentDept, short? currentClassProg, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; } else { searchString = currentFilter; }
            if (Term_Year_Id != null) { page = 1; } else { Term_Year_Id = currentTermYear; }
            if (Department_Id != null) { page = 1; } else { Department_Id = currentDept; }
            if (Class_Prog_Id != null) { page = 1; } else { Class_Prog_Id = currentClassProg; }

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

        // GET: OfferedCourseSched/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
            if (acd_Offered_Course_Sched == null)
            {
                return HttpNotFound();
            }
            return View(acd_Offered_Course_Sched);
        }

        // GET: OfferedCourseSched/Create
        public ActionResult Create()
        {
            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By");
            ViewBag.Sched_Session_Id = new SelectList(db.Acd_Sched_Session, "Sched_Session_Id", "Time_Start");
            ViewBag.Room_Id = new SelectList(db.Mstr_Room, "Room_Id", "Room_Code");
            return View();
        }

        // POST: OfferedCourseSched/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Offered_Course_Sched_id,Offered_Course_id,Sched_Session_Id,Room_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Sched acd_Offered_Course_Sched)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Offered_Course_Sched.Add(acd_Offered_Course_Sched);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Sched.Offered_Course_id);
            ViewBag.Sched_Session_Id = new SelectList(db.Acd_Sched_Session, "Sched_Session_Id", "Time_Start", acd_Offered_Course_Sched.Sched_Session_Id);
            ViewBag.Room_Id = new SelectList(db.Mstr_Room, "Room_Id", "Room_Code", acd_Offered_Course_Sched.Room_Id);
            return View(acd_Offered_Course_Sched);
        }

        // GET: OfferedCourseSched/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
            if (acd_Offered_Course_Sched == null)
            {
                return HttpNotFound();
            }
            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Sched.Offered_Course_id);
            ViewBag.Sched_Session_Id = new SelectList(db.Acd_Sched_Session, "Sched_Session_Id", "Time_Start", acd_Offered_Course_Sched.Sched_Session_Id);
            ViewBag.Room_Id = new SelectList(db.Mstr_Room, "Room_Id", "Room_Code", acd_Offered_Course_Sched.Room_Id);
            return View(acd_Offered_Course_Sched);
        }

        // POST: OfferedCourseSched/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Offered_Course_Sched_id,Offered_Course_id,Sched_Session_Id,Room_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Sched acd_Offered_Course_Sched)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Offered_Course_Sched).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Sched.Offered_Course_id);
            ViewBag.Sched_Session_Id = new SelectList(db.Acd_Sched_Session, "Sched_Session_Id", "Time_Start", acd_Offered_Course_Sched.Sched_Session_Id);
            ViewBag.Room_Id = new SelectList(db.Mstr_Room, "Room_Id", "Room_Code", acd_Offered_Course_Sched.Room_Id);
            return View(acd_Offered_Course_Sched);
        }

        // GET: OfferedCourseSched/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
            if (acd_Offered_Course_Sched == null)
            {
                return HttpNotFound();
            }
            return View(acd_Offered_Course_Sched);
        }

        // POST: OfferedCourseSched/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
            db.Acd_Offered_Course_Sched.Remove(acd_Offered_Course_Sched);
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
