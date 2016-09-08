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
                ViewBag.message = "Kurikulum Angkatan belum disetting, Silahkan setting terlebih dahulu.!";
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
            if (acd_Offered_Course.Count() == 0)
            {
                ViewBag.message = "Mata Kuliah ditawarkan belum disetting, Silahkan setting terlebih dahulu.!";
            }
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
        public ActionResult Create(int? Offered_Course_id, short? Sched_Session_Id, string UrlReferrer)
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
            if (Offered_Course_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course acd_Offered_Course = db.Acd_Offered_Course.Find(Offered_Course_id);
            if (acd_Offered_Course == null)
            {
                return HttpNotFound();
            }

            ViewBag.Prodi = acd_Offered_Course.Mstr_Department.Department_Name;

            List<short> SchedSessionx = db.Acd_Offered_Course_Sched.Where(ocs => ocs.Offered_Course_id == Offered_Course_id).Select(ocs => ocs.Sched_Session_Id).ToList();
            List<short> Roomx = db.Acd_Offered_Course_Sched.Where(ocs => ocs.Sched_Session_Id == Sched_Session_Id).Select(ocs => ocs.Room_Id).ToList();

            ViewBag.Matakuliah = acd_Offered_Course.Acd_Course.Course_Name + "(" + acd_Offered_Course.Acd_Course.Course_Code + ")";
            ViewBag.Kelas = acd_Offered_Course.Mstr_Class.Class_Name;
            if (acd_Offered_Course.Mstr_Class_Program.Class_Program_Name == "Reguler" || acd_Offered_Course.Mstr_Class_Program.Class_Program_Name == "Eksekutif")
            {
                ViewBag.AcdSchedSession = db.Acd_Sched_Session.Where(ss => !SchedSessionx.Contains(ss.Sched_Session_Id) && ss.Mstr_Sched_Type.Sched_Type_Name == "KULIAH").OrderBy(ss => ss.Day_Id).ThenBy(ss => ss.Time_Start).ToList();
            }
            else
            {
                ViewBag.AcdSchedSession = db.Acd_Sched_Session.Where(ss => !SchedSessionx.Contains(ss.Sched_Session_Id) && ss.Mstr_Sched_Type.Sched_Type_Name == "KULIAH" && ss.Class_Prog_Id == acd_Offered_Course.Class_Prog_Id).OrderBy(ss => ss.Day_Id).ThenBy(ss => ss.Time_Start).ToList();
            }
            List<Mstr_Room> room = new List<Mstr_Room>();
            room = db.Mstr_Room.Where(r => !Roomx.Contains(r.Room_Id)).ToList();
            if (room.Count() == 0)
            {
                ViewBag.message = "Tidak ada ruang kosong untuk sesi yang anda pilih.";
            }
            ViewBag.Room_Id = room;
            ViewBag.Offered_Course_id = Offered_Course_id;

            var acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Where(ocs => ocs.Offered_Course_id == Offered_Course_id);
            return View(acd_Offered_Course_Sched.ToList());
        }

        // POST: OfferedCourseSched/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Offered_Course_Sched_id,Offered_Course_id,Sched_Session_Id,Room_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Sched acd_Offered_Course_Sched, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Offered_Course_Sched.Add(acd_Offered_Course_Sched);
                db.SaveChanges();
                return RedirectToAction("Create", new { Offered_Course_id = acd_Offered_Course_Sched.Offered_Course_id, UrlReferrer = UrlReferrer });
            }

            //ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Sched.Offered_Course_id);
            //ViewBag.Sched_Session_Id = new SelectList(db.Acd_Sched_Session, "Sched_Session_Id", "Time_Start", acd_Offered_Course_Sched.Sched_Session_Id);
            //ViewBag.Room_Id = new SelectList(db.Mstr_Room, "Room_Id", "Room_Code", acd_Offered_Course_Sched.Room_Id);
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
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
        //    if (acd_Offered_Course_Sched == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Offered_Course_Sched);
        //}

        // POST: OfferedCourseSched/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, string UrlReferrer)
        {
            Acd_Offered_Course_Sched acd_Offered_Course_Sched = db.Acd_Offered_Course_Sched.Find(id);
            int x = db.Acd_Offered_Course.Where(oc => oc.Offered_Course_id == acd_Offered_Course_Sched.Offered_Course_id).Select(oc => oc.Offered_Course_id).First();
            db.Acd_Offered_Course_Sched.Remove(acd_Offered_Course_Sched);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Create", new { Offered_Course_id = x, UrlReferrer = UrlReferrer });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Create", new { Offered_Course_id = x, UrlReferrer = UrlReferrer });
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
