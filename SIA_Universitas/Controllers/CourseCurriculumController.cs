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
    public class CourseCurriculumController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CourseCurriculum
        public ActionResult Index(string currentFilter, string searchString, short? Department_Id, short? Class_Prog_Id, short? Curriculum_Id, int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = Department_Id;
            ViewBag.Class_Program = Class_Prog_Id;
            ViewBag.Curriculum = Curriculum_Id;
            
            //dropdown
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(p => p.Department_Code), "Department_Id", "Department_Name", Department_Id).ToList();
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(x => x.Department_Id == Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", Class_Prog_Id).ToList();
            ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum_Applied.Where(x => x.Department_Id == Department_Id && x.Class_Prog_Id == Class_Prog_Id).GroupBy(x => x.Curriculum_Id).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.Curriculum_Id), "Curriculum_Id", "Mstr_Curriculum.Curriculum_Name", Curriculum_Id).ToList();
            
            var acd_Course_Curriculum = db.Acd_Course_Curriculum.Where(x=> x.Department_Id == Department_Id && x.Class_Prog_Id == Class_Prog_Id && x.Curriculum_Id == Curriculum_Id)
                                        .Include(a => a.Acd_Course)
                                        .Include(a => a.Mstr_Curriculum_Type)
                                        .Include(a => a.Mstr_Department)
                                        .Include(a => a.Mstr_Study_Level)
                                        .Include(a => a.Acd_Course_Group)
                                        .Include(a => a.Mstr_Class_Program)
                                        .Include(a => a.Mstr_Curriculum);

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Course_Curriculum = acd_Course_Curriculum.Where(x => x.Acd_Course.Course_Code.Contains(searchString)
                                                                            || x.Acd_Course.Course_Name.Contains(searchString));
            }

            acd_Course_Curriculum = acd_Course_Curriculum.OrderBy(o => o.Order_Id);
            return View(acd_Course_Curriculum.ToPagedList(pageNumber, pageSize));
        }

        // GET: CourseCurriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Curriculum acd_Course_Curriculum = db.Acd_Course_Curriculum.Find(id);
            if (acd_Course_Curriculum == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Curriculum);
        }

        // GET: CourseCurriculum/Create
        public ActionResult Create(string currentFilter, string searchString, short? Department_Id, short? Class_Prog_Id, short? Curriculum_Id, int? page, int? rowPerPage)
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
            ViewBag.Class_Program = Class_Prog_Id;
            ViewBag.Curriculum = Curriculum_Id;

            var acd_Course_Curriculum = db.Acd_Course_Curriculum.Where(x => x.Department_Id == Department_Id && x.Class_Prog_Id == Class_Prog_Id && x.Curriculum_Id == Curriculum_Id).Select(x => x.Course_Id);
            var acd_Course = db.Acd_Course.Where(x => x.Department_Id == Department_Id);
            
            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Course = acd_Course.Where(x => x.Course_Code.Contains(searchString)
                                                || x.Course_Name.Contains(searchString));
            }

            acd_Course = acd_Course.Where(x => !acd_Course_Curriculum.Contains(x.Course_Id));
            acd_Course = acd_Course.OrderBy(x => x.Course_Code);

            return View(acd_Course.ToPagedList(pageNumber, pageSize));
        }

        // POST: CourseCurriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<int> checkCourse_IdToAdd, [Bind(Include = "Course_Cur_Id,Department_Id,Class_Prog_Id,Curriculum_Id,Course_Id,Course_Group_Id,Study_Level_Id,Study_Level_Sub,Applied_Sks,Transcript_Sks,Is_For_Transcript,Is_Required,Is_For_Concentration,Curriculum_Type_Id,Order_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Curriculum acd_Course_Curriculum)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (checkCourse_IdToAdd != null)
                    {
                        foreach (var i in checkCourse_IdToAdd)
                        {
                            acd_Course_Curriculum.Course_Id = i;

                            acd_Course_Curriculum.Applied_Sks = 0;
                            acd_Course_Curriculum.Is_For_Transcript = true;
                            acd_Course_Curriculum.Is_Required = true;
                            acd_Course_Curriculum.Course_Group_Id = 1; //kelompok
                            acd_Course_Curriculum.Study_Level_Id = 0;
                            acd_Course_Curriculum.Study_Level_Sub = 0;
                            acd_Course_Curriculum.Curriculum_Type_Id = 1; //jns kurikulum
                            acd_Course_Curriculum.Is_Valid = false;
                            acd_Course_Curriculum.Created_Date = DateTime.Now;
                            acd_Course_Curriculum.Transcript_Sks = 0;

                            db.Acd_Course_Curriculum.Add(acd_Course_Curriculum);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return RedirectToAction("Create", new { Department_Id = acd_Course_Curriculum.Department_Id, Class_Prog_Id = acd_Course_Curriculum.Class_Prog_Id, Curriculum_Id = acd_Course_Curriculum.Curriculum_Id });
            }

            return RedirectToAction("Index", new { Department_Id = acd_Course_Curriculum.Department_Id, Class_Prog_Id = acd_Course_Curriculum.Class_Prog_Id, Curriculum_Id = acd_Course_Curriculum.Curriculum_Id});
        }

        // GET: CourseCurriculum/Edit/5
        public ActionResult Edit(int? id, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Curriculum acd_Course_Curriculum = db.Acd_Course_Curriculum.Find(id);
            if (acd_Course_Curriculum == null)
            {
                return HttpNotFound();
            }

            //for url
            ViewBag.searchString = searchString;

            ViewBag.Course_Group_Id = new SelectList(db.Acd_Course_Group, "Course_Group_Id", "Name_Of_Group", acd_Course_Curriculum.Course_Group_Id);
            ViewBag.Study_Level_Id = new SelectList(db.Mstr_Study_Level, "Study_Level_Id", "Level_Name", acd_Course_Curriculum.Study_Level_Id);
            ViewBag.Curriculum_Type_Id = new SelectList(db.Mstr_Curriculum_Type, "Curriculum_Type_Id", "Curriculum_Type_Name", acd_Course_Curriculum.Curriculum_Type_Id);
            
            return View(acd_Course_Curriculum);
        }

        // POST: CourseCurriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Cur_Id,Department_Id,Class_Prog_Id,Curriculum_Id,Course_Id,Course_Group_Id,Study_Level_Id,Study_Level_Sub,Applied_Sks,Transcript_Sks,Is_For_Transcript,Is_Required,Is_For_Concentration,Curriculum_Type_Id,Order_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Curriculum acd_Course_Curriculum, string searchString)
        {
            if (ModelState.IsValid)
            {
                acd_Course_Curriculum.Is_Valid = true;

                db.Acd_Course_Curriculum.Attach(acd_Course_Curriculum); // attach in the Unchanged state
                db.Entry(acd_Course_Curriculum).Property(r => r.Applied_Sks).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Is_For_Transcript).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Transcript_Sks).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Is_Required).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Course_Group_Id).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Study_Level_Id).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Study_Level_Sub).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Curriculum_Type_Id).IsModified = true;
                db.Entry(acd_Course_Curriculum).Property(r => r.Is_Valid).IsModified = true;

                //db.Entry(acd_Course_Curriculum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { 
                            searchString = searchString,
                            Department_Id = acd_Course_Curriculum.Department_Id,
                            Class_Prog_Id = acd_Course_Curriculum.Class_Prog_Id,
                            Curriculum_Id = acd_Course_Curriculum.Curriculum_Id } );
            }

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = acd_Course_Curriculum.Department_Id;
            ViewBag.Class_Program = acd_Course_Curriculum.Class_Prog_Id;
            ViewBag.Curriculum = acd_Course_Curriculum.Curriculum_Id;

            ViewBag.Course_Group_Id = new SelectList(db.Acd_Course_Group, "Course_Group_Id", "Name_Of_Group", acd_Course_Curriculum.Course_Group_Id);
            ViewBag.Study_Level_Id = new SelectList(db.Mstr_Study_Level, "Study_Level_Id", "Level_Name", acd_Course_Curriculum.Study_Level_Id);
            ViewBag.Curriculum_Type_Id = new SelectList(db.Mstr_Curriculum_Type, "Curriculum_Type_Id", "Curriculum_Type_Name", acd_Course_Curriculum.Curriculum_Type_Id);

            return View(acd_Course_Curriculum);
        }

        // GET: CourseCurriculum/Delete/5
        public ActionResult Delete(int? id, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Curriculum acd_Course_Curriculum = db.Acd_Course_Curriculum.Find(id);
            if (acd_Course_Curriculum == null)
            {
                return HttpNotFound();
            }
            //for url
            ViewBag.searchString = searchString;
            return View(acd_Course_Curriculum);
        }

        // POST: CourseCurriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string searchString)
        {
            Acd_Course_Curriculum acd_Course_Curriculum = db.Acd_Course_Curriculum.Find(id);
            db.Acd_Course_Curriculum.Remove(acd_Course_Curriculum);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index", new
                {
                    searchString = searchString,
                    Department_Id = acd_Course_Curriculum.Department_Id,
                    Class_Prog_Id = acd_Course_Curriculum.Class_Prog_Id,
                    Curriculum_Id = acd_Course_Curriculum.Curriculum_Id
                });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Index", new
            {
                searchString = searchString,
                Department_Id = acd_Course_Curriculum.Department_Id,
                Class_Prog_Id = acd_Course_Curriculum.Class_Prog_Id,
                Curriculum_Id = acd_Course_Curriculum.Curriculum_Id 
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
