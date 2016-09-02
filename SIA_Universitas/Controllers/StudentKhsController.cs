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
    public class StudentKhsController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: StudentKhs
        public ActionResult Index(string currentFilter, string searchString, short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? page, int? rowPerPage)
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
            ViewBag.Term_Year = Term_Year_Id;
            ViewBag.Class_Prog = Class_Prog_Id;

            //dropdown
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id).ToList();
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id).ToList();
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", Class_Prog_Id).ToList();
            
            List<Vm_Student_Khs> list_vm_Student_Khs = new List<Vm_Student_Khs>();

            if (Term_Year_Id != null && Department_Id != null && Class_Prog_Id != null)
            {
                //var khs_sudah_dinilai = db.Acd_Student_Khs
                //                           .Where(x => x.Acd_Student_Krs.Term_Year_Id == Term_Year_Id
                //                                       && x.Acd_Student.Department_Id == Department_Id
                //                                       && x.Acd_Student.Class_Prog_Id == Class_Prog_Id
                //    && x.Grade_Letter_Id != null
                //                                       )

                //                           .Select(x => x.Krs_Id);

                var acd_Student_Khs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                                    && x.Acd_Course.Department_Id == Department_Id
                                                                    && x.Acd_Student.Class_Prog_Id == Class_Prog_Id
                                                                    )
                                            .GroupBy(y => new 
                                            { 
                                                y.Acd_Course.Course_Code,
                                                y.Acd_Course.Course_Name,
                                                y.Mstr_Class.Class_Name
                                            })
                                            .OrderBy(y => y.FirstOrDefault().Acd_Course.Course_Code).ThenBy(y => y.FirstOrDefault().Class_Id)
                                            .Select(y => new Vm_Student_Khs
                                            {
                                                Course_Id = y.FirstOrDefault().Course_Id,
                                                Class_Id = y.FirstOrDefault().Class_Id,
                                                Course_Code = y.FirstOrDefault().Acd_Course.Course_Code,
                                                Course_Name = y.FirstOrDefault().Acd_Course.Course_Name,
                                                Class_Name = y.FirstOrDefault().Mstr_Class.Class_Name,
                                                Jml_Peserta = y.Count(),

                                                //Sudah_Dinilai = db.Acd_Student_Khs.Where(z => z.Acd_Student_Krs.Term_Year_Id == Term_Year_Id
                                                //                                                    && z.Acd_Student.Department_Id == Department_Id
                                                //                                                    && z.Acd_Student.Class_Prog_Id == Class_Prog_Id
                                                //                                                    && z.Acd_Student_Krs.Course_Id == y.FirstOrDefault().Course_Id
                                                //                                                    && z.Acd_Student_Krs.Class_Id == y.FirstOrDefault().Class_Id)
                                                //                                    .Count()

                                                //Jml_Peserta = db.Acd_Student_Krs.Where(a => a.Course_Id == x.FirstOrDefault().Course_Id
                                                //                                            && a.Term_Year_Id == Term_Year_Id
                                                //                                            && a.Acd_Course.Department_Id == Department_Id
                                                //                                            && a.Acd_Student.Class_Prog_Id == Class_Prog_Id).Count(),
                                                //Sudah_Dinilai = db.Acd_Student_Krs.Where(b => b.Course_Id == y.FirstOrDefault().Course_Id
                                                //                                            && b.Class_Id == y.FirstOrDefault().Class_Id
                                                //                                            && b.Term_Year_Id == Term_Year_Id
                                                //                                            && b.Acd_Course.Department_Id == Department_Id
                                                //                                            && b.Acd_Student.Class_Prog_Id == Class_Prog_Id
                                                //                                            && khs_sudah_dinilai.Contains(b.Krs_Id)
                                                //                                            ).Count()
                                                //Sudah_Dinilai = db.Acd_Student_Khs.Where(a => khs_sudah_dinilai.Contains(a.Krs_Id)
                                                //                                              && a.Acd_Student_Krs.Class_Id == y.FirstOrDefault().Class_Id
                                                //                                              && a.Acd_Student_Krs.Course_Id == y.FirstOrDefault().Course_Id
                                                //                                         ).Count()
                                            });

                if (!String.IsNullOrEmpty(searchString))
                {
                    acd_Student_Khs = acd_Student_Khs.Where(x => x.Course_Code.Contains(searchString) || x.Course_Name.Contains(searchString));
                }

                //acd_Student_Khs = acd_Student_Khs.OrderBy(x => x.Course_Code);

                //foreach (var item in acd_Student_Khs)
                //{
                //    Vm_Student_Khs vm_Student_Khs = new Vm_Student_Khs();
                //    vm_Student_Khs.Course_Code = item.Course_Code;
                //    vm_Student_Khs.Course_Name = item.Course_Name;
                //    vm_Student_Khs.Class_Name = item.Class_Name;
                //    vm_Student_Khs.Jml_Peserta = item.Jml_Peserta;
                //    vm_Student_Khs.Sudah_Dinilai = item.Sudah_Dinilai;

                //    list_vm_Student_Khs.Add(vm_Student_Khs);
                //}

                return View(acd_Student_Khs.ToPagedList(pageNumber, pageSize));
            }

            
            return View(list_vm_Student_Khs.ToPagedList(pageNumber, pageSize));
        }

        // GET: StudentKhs/EditList?Term_Year_Id=20011&Department_Id=8&Class_Prog_Id=1&Course_Id=1600&Class_Id=1
        public ActionResult EditList(string currentFilter, string searchString, short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? Course_Id, short? Class_Id, int? page, int? rowPerPage)
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
            ViewBag.Term_Year = Term_Year_Id;
            ViewBag.Class_Prog = Class_Prog_Id;
            ViewBag.Course_Id = Course_Id;
            ViewBag.Class_Id = Class_Id;

            var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                                && x.Acd_Student.Department_Id == Department_Id
                                                                && x.Acd_Student.Class_Prog_Id == Class_Prog_Id
                                                                && x.Course_Id == Course_Id
                                                                && x.Class_Id == Class_Id)
                                                    .OrderBy(x => x.Acd_Student.Nim)
                                                    .Select(x => new Vm_Student_Khs_EditList
                                                    {
                                                        Krs_Id = x.Krs_Id,
                                                        Course_Code = x.Acd_Course.Course_Code,
                                                        Course_Name = x.Acd_Course.Course_Name,
                                                        Class_Name = x.Mstr_Class.Class_Name,

                                                        Nim = x.Acd_Student.Nim,
                                                        Full_Name = x.Acd_Student.Full_Name,
                                                        Grade_Letter = x.Acd_Student_Khs.FirstOrDefault().Acd_Grade_Letter.Grade_Letter,
                                                        Weight_Value = x.Acd_Student_Khs.FirstOrDefault().Weight_Value,
                                                        Sks = x.Sks,

                                                        //duo = db.Acd_Course_Curriculum.Where(y => y.Department_Id == Department_Id
                                                        //                                 && y.Class_Prog_Id == Class_Prog_Id
                                                        //                                 && y.Course_Id == Course_Id
                                                        //                                 && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                        //                                                                                             && z.Department_Id == Department_Id
                                                        //                                                                                             && z.Class_Prog_Id == Class_Prog_Id
                                                        //                                                                                             && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                        //                                                                                     .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                        //                              )
                                                        //                         .Select(y => new 
                                                        //                         {
                                                        //                            Is_For_Transcript = y.Is_For_Transcript.ToString(),
                                                        //                            Transcript_Sks = y.Transcript_Sks.ToString()
                                                        //                         }).ToArray()

                                                        Is_For_Transcript = db.Acd_Course_Curriculum.Where(y => y.Department_Id == Department_Id
                                                                                                            && y.Class_Prog_Id == Class_Prog_Id
                                                                                                            && y.Course_Id == Course_Id
                                                                                                            && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                        && z.Department_Id == Department_Id
                                                                                                                                                                        && z.Class_Prog_Id == Class_Prog_Id
                                                                                                                                                                        && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                        )
                                                                                                .Select(y => y.Is_For_Transcript).FirstOrDefault(),

                                                        Transcript_Sks = db.Acd_Course_Curriculum.Where(y => y.Department_Id == Department_Id
                                                                                                            && y.Class_Prog_Id == Class_Prog_Id
                                                                                                            && y.Course_Id == Course_Id
                                                                                                            && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                        && z.Department_Id == Department_Id
                                                                                                                                                                        && z.Class_Prog_Id == Class_Prog_Id
                                                                                                                                                                        && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                        )
                                                                                                .Select(y => y.Transcript_Sks).FirstOrDefault()

                                                    });

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student_Krs = acd_Student_Krs.Where(x => x.Nim.Contains(searchString) || x.Full_Name.Contains(searchString));
            }

            return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
            
        }

        //// GET: StudentKhs/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Student_Khs acd_Student_Khs = db.Acd_Student_Khs.Find(id);
        //    if (acd_Student_Khs == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Student_Khs);
        //}

        //// GET: StudentKhs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Grade_Letter_Id = new SelectList(db.Acd_Grade_Letter, "Grade_Letter_Id", "Grade_Letter");
        //    ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim");
        //    ViewBag.Krs_Id = new SelectList(db.Acd_Student_Krs, "Krs_Id", "Modified_By");
        //    return View();
        //}

        //// POST: StudentKhs/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? Course_Id, short? Class_Id, [Bind(Include = "Khs_Id,Krs_Id,Student_Id,Grade_Letter_Id,Modified_Date,Modified_By,Weight_Value,Is_Required,Is_For_Transkrip,Bnk_Value,Created_Date,Created_By")] Acd_Student_Khs acd_Student_Khs)
        //{
        //    //if (Term_Year_Id == null || Department_Id == null || Class_Prog_Id == null || Course_Id == null || Class_Id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        acd_Student_Khs.Created_Date = DateTime.Now;

        //        db.Acd_Student_Khs.Add(acd_Student_Khs);
        //        db.SaveChanges();
        //        return RedirectToAction("EditList", "StudentKhs", new { Term_Year_Id = Term_Year_Id, Department_Id = Department_Id, Class_Prog_Id = Class_Prog_Id, Course_Id = Course_Id, Class_Id = Class_Id });
        //    }

        //    //ViewBag.Grade_Letter_Id = new SelectList(db.Acd_Grade_Letter, "Grade_Letter_Id", "Grade_Letter", acd_Student_Khs.Grade_Letter_Id);
        //    //ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Student_Khs.Student_Id);
        //    //ViewBag.Krs_Id = new SelectList(db.Acd_Student_Krs, "Krs_Id", "Modified_By", acd_Student_Khs.Krs_Id);
        //    //return View(acd_Student_Khs);
        //    return RedirectToAction("Edit", "StudentKhs", new { Krs_Id = acd_Student_Khs.Krs_Id});
        //}

        // GET: StudentKhs/Edit/5
        public ActionResult Edit(long? Krs_Id)
        {
            if (Krs_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(Krs_Id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }

            short? department_Id = acd_Student_Krs.Acd_Student.Department_Id;
            var listGradeLetter = db.Acd_Grade_Department.Where(x => x.Department_Id == department_Id)
                                                            .Select(x=> new {
                                                                Grade_Letter_Id = x.Acd_Grade_Letter.Grade_Letter_Id,
                                                                Grade_Letter = x.Acd_Grade_Letter.Grade_Letter
                                                            }).ToList();

            short? grade_Letter_Id = null;
            if (acd_Student_Krs.Acd_Student_Khs.FirstOrDefault() != null)
            {
                grade_Letter_Id = acd_Student_Krs.Acd_Student_Khs.FirstOrDefault().Grade_Letter_Id;
            }

            ViewBag.Grade_Letter_Id = new SelectList(listGradeLetter, "Grade_Letter_Id", "Grade_Letter", grade_Letter_Id);
                                                        
            return View(acd_Student_Krs);
        }

        //// POST: StudentKhs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? Course_Id, short? Class_Id, [Bind(Include = "Khs_Id,Krs_Id,Student_Id,Grade_Letter_Id,Modified_Date,Modified_By,Weight_Value,Is_Required,Is_For_Transkrip,Bnk_Value,Created_Date,Created_By")] Acd_Student_Khs acd_Student_Khs)
        //{
        //    //if (Term_Year_Id == null || Department_Id == null || Class_Prog_Id == null || Course_Id == null || Class_Id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        acd_Student_Khs.Modified_Date = DateTime.Now;

        //        db.Entry(acd_Student_Khs).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("EditList", "StudentKhs", new { Term_Year_Id = Term_Year_Id, Department_Id = Department_Id, Class_Prog_Id = Class_Prog_Id, Course_Id = Course_Id, Class_Id = Class_Id });
        //    }
        //    //ViewBag.Grade_Letter_Id = new SelectList(db.Acd_Grade_Letter, "Grade_Letter_Id", "Grade_Letter", acd_Student_Khs.Grade_Letter_Id);
        //    //ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Student_Khs.Student_Id);
        //    //ViewBag.Krs_Id = new SelectList(db.Acd_Student_Krs, "Krs_Id", "Modified_By", acd_Student_Khs.Krs_Id);
        //    //return View(acd_Student_Khs);
        //    return RedirectToAction("Edit", "StudentKhs", new { Krs_Id = acd_Student_Khs.Krs_Id });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(long? Khs_Id, short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? Course_Id, short? Class_Id, [Bind(Include = "Khs_Id,Krs_Id,Student_Id,Grade_Letter_Id,Modified_Date,Modified_By,Weight_Value,Is_Required,Is_For_Transkrip,Bnk_Value,Created_Date,Created_By")] Acd_Student_Khs acd_Student_Khs)
        {
            if (Term_Year_Id == null || Department_Id == null || Class_Prog_Id == null || Course_Id == null || Class_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            switch (Khs_Id)
            {
                    
                case null:
                    if (ModelState.IsValid)
                    {
                        acd_Student_Khs.Created_Date = DateTime.Now;

                        if (acd_Student_Khs.Grade_Letter_Id == null)
                        {
                            acd_Student_Khs.Weight_Value = null;
                        }
                        else
                        {
                            acd_Student_Khs.Weight_Value = db.Acd_Grade_Department.Where(x => x.Grade_Letter_Id == acd_Student_Khs.Grade_Letter_Id && x.Department_Id == Department_Id)
                                                                                    .Select(x => x.Weight_Value).SingleOrDefault();
                        }

                        db.Acd_Student_Khs.Add(acd_Student_Khs);
                        db.SaveChanges();
                        return RedirectToAction("EditList", "StudentKhs", new { Term_Year_Id = Term_Year_Id, Department_Id = Department_Id, Class_Prog_Id = Class_Prog_Id, Course_Id = Course_Id, Class_Id = Class_Id });
                    }
                    return RedirectToAction("Edit", "StudentKhs", new { Krs_Id = acd_Student_Khs.Krs_Id });

                default:
                    if (ModelState.IsValid)
                    {
                        acd_Student_Khs.Modified_Date = DateTime.Now;

                        if (acd_Student_Khs.Grade_Letter_Id == null)
                        {
                            acd_Student_Khs.Weight_Value = null;
                        }
                        else
                        {
                            acd_Student_Khs.Weight_Value = db.Acd_Grade_Department.Where(x => x.Grade_Letter_Id == acd_Student_Khs.Grade_Letter_Id && x.Department_Id == Department_Id)
                                                                                    .Select(x => x.Weight_Value).SingleOrDefault();
                        }
                        
                        db.Acd_Student_Khs.Attach(acd_Student_Khs); // attach in the Unchanged state
                        db.Entry(acd_Student_Khs).Property(r => r.Modified_Date).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Modified_By).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Grade_Letter_Id).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Weight_Value).IsModified = true;

                        //db.Entry(acd_Student_Khs).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("EditList", "StudentKhs", new { Term_Year_Id = Term_Year_Id, 
                                                                                Department_Id = Department_Id, 
                                                                                Class_Prog_Id = Class_Prog_Id, 
                                                                                Course_Id = Course_Id, 
                                                                                Class_Id = Class_Id });
                        //return RedirectToAction("EditList", "StudentKhs", new { Term_Year_Id = acd_Student_Khs.Acd_Student_Krs.Term_Year_Id,
                        //                                                        Department_Id = acd_Student_Khs.Acd_Student_Krs.Acd_Course.Department_Id, 
                        //                                                        Class_Prog_Id = acd_Student_Khs.Acd_Student_Krs.Class_Prog_Id, 
                        //                                                        Course_Id = acd_Student_Khs.Acd_Student_Krs.Course_Id, 
                        //                                                        Class_Id = acd_Student_Khs.Acd_Student_Krs.Class_Id });
                    }
                    return RedirectToAction("Edit", "StudentKhs", new { Krs_Id = acd_Student_Khs.Krs_Id });
            }
        }

        //// GET: StudentKhs/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Student_Khs acd_Student_Khs = db.Acd_Student_Khs.Find(id);
        //    if (acd_Student_Khs == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Student_Khs);
        //}

        //// POST: StudentKhs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Acd_Student_Khs acd_Student_Khs = db.Acd_Student_Khs.Find(id);
        //    db.Acd_Student_Khs.Remove(acd_Student_Khs);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
