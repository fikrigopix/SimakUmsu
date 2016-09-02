using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
//using PagedList;
using System.Net;


namespace SIA_Universitas.Controllers
{
    public class Student_Khsv2Controller : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Student_Khsv2
        public ActionResult Index(string currentFilter, string searchString, short? Term_Year_Id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Term_Year = Term_Year_Id;

            //dropdown
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id).ToList();

            List<Vm_Student_KhsV2> list_vm_Student_KhsV2 = new List<Vm_Student_KhsV2>();

            if (Term_Year_Id != null && !String.IsNullOrEmpty(searchString))
            {
                var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                                && x.Acd_Student.Nim == searchString
                                                                )
                                                        .OrderBy(x => x.Acd_Course.Course_Code)
                                                        .Select(x => new Vm_Student_KhsV2
                                                        {
                                                            Krs_Id = x.Krs_Id,
                                                            Nim = x.Acd_Student.Nim,
                                                            Full_Name = x.Acd_Student.Full_Name,
                                                            Course_Code = x.Acd_Course.Course_Code,
                                                            Course_Name = x.Acd_Course.Course_Name,
                                                            Grade_Letter = x.Acd_Student_Khs.FirstOrDefault().Acd_Grade_Letter.Grade_Letter,
                                                            Weight_Value = x.Acd_Student_Khs.FirstOrDefault().Weight_Value,
                                                            Sks = x.Sks,

                                                            Is_For_Transcript = db.Acd_Course_Curriculum.Where(y => y.Department_Id == x.Acd_Student.Department_Id
                                                                                                                && y.Class_Prog_Id == x.Class_Prog_Id
                                                                                                                && y.Course_Id == x.Course_Id
                                                                                                                && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                            && z.Department_Id == x.Acd_Student.Department_Id
                                                                                                                                                                            && z.Class_Prog_Id == x.Acd_Student.Class_Prog_Id
                                                                                                                                                                            && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                    .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                            )
                                                                                                    .Select(y => y.Is_For_Transcript).FirstOrDefault(),

                                                            Transcript_Sks = db.Acd_Course_Curriculum.Where(y => y.Department_Id == x.Acd_Student.Department_Id
                                                                                                                && y.Class_Prog_Id == x.Class_Prog_Id
                                                                                                                && y.Course_Id == x.Course_Id
                                                                                                                && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                            && z.Department_Id == x.Acd_Student.Department_Id
                                                                                                                                                                            && z.Class_Prog_Id == x.Acd_Student.Class_Prog_Id
                                                                                                                                                                            && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                    .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                            )
                                                                                                    .Select(y => y.Transcript_Sks).FirstOrDefault()

                                                        });
                //return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
                return View(acd_Student_Krs);
            }
            //return View(list_vm_Student_KhsV2.ToPagedList(pageNumber, pageSize));
            return View(list_vm_Student_KhsV2);
        }

        // GET: Student_Khsv2/Edit/5
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
                                                            .Select(x => new
                                                            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(long? Khs_Id, short? Term_Year_Id, string Nim, short? Department_Id, [Bind(Include = "Khs_Id,Krs_Id,Student_Id,Grade_Letter_Id,Modified_Date,Modified_By,Weight_Value,Is_Required,Is_For_Transkrip,Bnk_Value,Created_Date,Created_By")] Acd_Student_Khs acd_Student_Khs)
        {
            if (Term_Year_Id == null || String.IsNullOrEmpty(Nim) || Department_Id == null)
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
                        return RedirectToAction("Index", "Student_Khsv2", new { Term_Year_Id = Term_Year_Id, searchString = Nim });
                    }
                    return RedirectToAction("Edit", "Student_Khsv2", new { Krs_Id = acd_Student_Khs.Krs_Id });

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
                        return RedirectToAction("Index", "Student_Khsv2", new { Term_Year_Id = Term_Year_Id, searchString = Nim });
                    }
                    return RedirectToAction("Edit", "Student_Khsv2", new { Krs_Id = acd_Student_Khs.Krs_Id });
            }
        }
    }
}