using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SI_KEU_MHS_Universitas.Models;
using EntityFramework.Extensions;
using System.Data.Entity.Infrastructure;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CourseCostTypeController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CourseCostType
        public ActionResult Index(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id)
        {
            short clasProgId = Class_Prog_Id ?? 0;
            short termYearId = Term_Year_Id ?? 0;
            short departmentId = Department_Id ?? 0;

            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }

            if (Class_Prog_Id != null && Term_Year_Id != null && Department_Id != null)
            {
                ViewBag.ClasProgId = Class_Prog_Id;
                ViewBag.TermYearId = Term_Year_Id;
                ViewBag.DepartmentId = Department_Id;
            }

            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", Class_Prog_Id).ToList();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d=>d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            var exceptionList = db.Fnc_Course_Cost_Type.Where(cct => cct.Term_Year_Id == termYearId
                                                                  && cct.Department_Id == departmentId
                                                                  && cct.Class_Prog_Id == clasProgId).Select(cct => cct.Course_Id);

            ViewBag.CourseCostTypeSks = db.Fnc_Course_Cost_Type.Where(cct => cct.Term_Year_Id == termYearId
                                                                       && cct.Department_Id == departmentId
                                                                       && cct.Class_Prog_Id == clasProgId
                                                                       && cct.Is_Sks == true).ToList();
            ViewBag.CourseCostTypePaket = db.Fnc_Course_Cost_Type.Where(cct => cct.Term_Year_Id == termYearId
                                                                       && cct.Department_Id == departmentId
                                                                       && cct.Class_Prog_Id == clasProgId
                                                                       && cct.Is_Sks == false).ToList();
            var acd_Offered_Course = db.Acd_Offered_Course.Where(oc => !exceptionList.Contains(oc.Course_Id)
                                                                    && oc.Term_Year_Id == termYearId
                                                                    && oc.Department_Id == departmentId
                                                                    && oc.Class_Prog_Id == clasProgId)
                                                          .GroupBy(oc => oc.Course_Id).Select(oc => oc.FirstOrDefault()).OrderBy(oc => oc.Acd_Course.Course_Name);

            return View(acd_Offered_Course.ToList());
        }

        //// GET: CourseCostType/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Find(id);
        //    if (fnc_Course_Cost_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Course_Cost_Type);
        //}

        // GET: CourseCostType/Create

        public ActionResult Create(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id)
        {
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Name");
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();
            return View();
        }

        // POST: CourseCostType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Cost_Type_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Course_Id,Is_Sks,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Type fnc_Course_Cost_Type, IEnumerable<int> CourseId, IEnumerable<int> delSKS, IEnumerable<int> delPaket, bool Is_Delete)
        {
            if (Is_Delete == true)
            {
                if (fnc_Course_Cost_Type.Is_Sks == true)
                {
                    try
                    {
                        db.Fnc_Course_Cost_Type.Where(cct => delSKS.Contains(cct.Course_Cost_Type_Id)).Delete();
                    }
                    catch (Exception)
                    {
                        TempData["shortMessage"] = "Data tidak bisa dihapus, dikarenakan Harganya telah disetting..";
                    }
                }
                else
                {
                    try
                    {
                        db.Fnc_Course_Cost_Type.Where(cct => delPaket.Contains(cct.Course_Cost_Type_Id)).Delete();
                    }
                    catch (Exception)
                    {
                        TempData["shortMessage"] = "Data tidak bisa dihapus, dikarenakan Harganya telah disetting..";
                    }
                }
            }
            else
            {
                if (CourseId != null)
                {
                    if (ModelState.IsValid)
                    {
                        foreach (var course in CourseId)
                        {
                            fnc_Course_Cost_Type.Course_Id = course;
                            db.Fnc_Course_Cost_Type.Add(fnc_Course_Cost_Type);
                            db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Type.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Type.Term_Year_Id, Department_Id = fnc_Course_Cost_Type.Department_Id });
        }

        // GET: CourseCostType/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Find(id);
        //    if (fnc_Course_Cost_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", fnc_Course_Cost_Type.Course_Id);
        //    ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", fnc_Course_Cost_Type.Class_Prog_Id);
        //    ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", fnc_Course_Cost_Type.Department_Id);
        //    ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", fnc_Course_Cost_Type.Term_Year_Id);
        //    return View(fnc_Course_Cost_Type);
        //}

        //// POST: CourseCostType/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Course_Cost_Type_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Course_Id,Is_Sks,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Type fnc_Course_Cost_Type)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(fnc_Course_Cost_Type).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", fnc_Course_Cost_Type.Course_Id);
        //    ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", fnc_Course_Cost_Type.Class_Prog_Id);
        //    ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", fnc_Course_Cost_Type.Department_Id);
        //    ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", fnc_Course_Cost_Type.Term_Year_Id);
        //    return View(fnc_Course_Cost_Type);
        //}

        //// GET: CourseCostType/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Find(id);
        //    if (fnc_Course_Cost_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Course_Cost_Type);
        //}

        //// POST: CourseCostType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Find(id);
        //    db.Fnc_Course_Cost_Type.Remove(fnc_Course_Cost_Type);
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
