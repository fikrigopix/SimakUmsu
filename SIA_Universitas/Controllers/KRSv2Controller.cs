using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace SIA_Universitas.Controllers
{
    public class KRSv2Controller : Controller
    {
        private SIAEntities db = new SIAEntities();
        //
        // GET: /KRSv2/
        public ActionResult Index(String Student_NIM, short? Term_Year_Id)
        {
            string studentNIM = Student_NIM;
            short termYearId = Term_Year_Id ?? 0;


            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);

            if (studentNIM == "" || termYearId == 0)
            {
                return View();
            }
            else
            {
                var student = db.Acd_Student.Where(k => k.Nim == studentNIM).Single();
                ViewBag.Student_Id = student.Student_Id;
                ViewBag.TermYearSelected = termYearId;

                var model = db.Acd_Student_Krs.Where(k => k.Student_Id == student.Student_Id && k.Term_Year_Id == termYearId);
                var curEntryYear = db.Acd_Curriculum_Entry_Year.Where(k => k.Term_Year_Id == termYearId && k.Department_Id == student.Department_Id && k.Entry_Year_Id == student.Entry_Year_Id).Single();
                var saldo = db.usp_Saldo(student.Student_Id, termYearId).Single();

                @ViewBag.nama = student.Full_Name;
                @ViewBag.prodi = student.Mstr_Department.Department_Name;
                @ViewBag.classprogram = curEntryYear.Mstr_Class_Program.Class_Program_Name;
                @ViewBag.Class_Prog_Id = curEntryYear.Class_Prog_Id;
                @ViewBag.sks = db.usp_GetAllowedSKSForKRS(termYearId, student.Student_Id).Single().Value;
                @ViewBag.deposit = saldo.DepositSmtIni;
                @ViewBag.depositsisalalu = saldo.SisaDepositLalu;
                @ViewBag.depositbisa = saldo.DepositSmtIni + saldo.SisaDepositLalu;
                @ViewBag.depositsaatini = saldo.DipakaiSaatIni;
                @ViewBag.sisasaldo = saldo.SisaSaldoSaatIni;
                @ViewBag.kkn = "";
                @ViewBag.krskkn = "";
                @ViewBag.saldokkn = "";
                @ViewBag.EntryYearID = curEntryYear.Entry_Year_Id;
                return View(model.ToList());
            }

        }
        public ActionResult Insert(short Term_Year_Id, Int64 Student_Id)
        {
            var student = db.Acd_Student.Where(k => k.Student_Id == Student_Id).Single();
            var curEntryYear = db.Acd_Curriculum_Entry_Year.Where(k => k.Term_Year_Id == Term_Year_Id && k.Department_Id == student.Department_Id && k.Entry_Year_Id == student.Entry_Year_Id).Single();
            ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(Term_Year_Id, student.Department_Id, curEntryYear.Class_Prog_Id, student.Entry_Year_Id, Student_Id), "course_id", "course_name");
            ViewBag.Class_Id = new SelectList(new[] { new { ID = "0", Name = "-- Pilih Kelas --" } }, "ID", "Name");
            ViewBag.Term_Year_Id = Term_Year_Id;
            ViewBag.Student_Id = Student_Id;
            ViewBag.Class_Prog_Id = curEntryYear.Class_Prog_Id;
            //ViewBag.Curriculum_Id = db.Acd_Curriculum_Entry_Year.Where(p => p.Department_Id == student.Department_Id && p.Entry_Year_Id == student.Entry_Year_Id && p.Term_Year_Id == Term_Year_Id).SingleOrDefault().Curriculum_Id;
            Acd_Student_Krs acd_Student_Krs = new Acd_Student_Krs();
            acd_Student_Krs.Term_Year_Id = Term_Year_Id;
            acd_Student_Krs.Student_Id = Student_Id;
            acd_Student_Krs.Class_Prog_Id = curEntryYear.Class_Prog_Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(Acd_Student_Krs acd_Student_Krs)
        {
            if (ModelState.IsValid)
            {
                var student = db.Acd_Student.Find(acd_Student_Krs.Student_Id);
                var classInfo = db.usp_GetClassInfoForKRS(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, acd_Student_Krs.Class_Id).Single();
                var saldo = db.usp_Saldo(acd_Student_Krs.Student_Id, acd_Student_Krs.Term_Year_Id).Single();
                decimal usedSKS = db.Acd_Student_Krs.Where(k => k.Student_Id == acd_Student_Krs.Student_Id && k.Term_Year_Id == acd_Student_Krs.Term_Year_Id).Sum(p => p.Sks);
                decimal AllowedSKS = db.usp_GetAllowedSKSForKRS(acd_Student_Krs.Term_Year_Id, acd_Student_Krs.Student_Id).Single().Value;
                var CourseCostForKRS = db.usp_GetCourseCostForKRS(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, acd_Student_Krs.Course_Id).Single();

                if (classInfo.Free <= 0)
                {
                    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                    ViewBag.NotifMessage = "Kelas Sudah Penuh.";
                    return View(acd_Student_Krs);
                }
                if ((AllowedSKS - usedSKS) < CourseCostForKRS.applied_sks)
                {
                    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                    ViewBag.NotifMessage = "Sisa SKS tidak mencukupi.";
                    return View(acd_Student_Krs);
                }
                if (saldo.SisaSaldoSaatIni < CourseCostForKRS.amount)
                {
                    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                    ViewBag.NotifMessage = "Sisa Saldo tidak mencukupi.";
                    return View(acd_Student_Krs);
                }
                db.Acd_Student_Krs.Add(acd_Student_Krs);
                db.SaveChanges();
                return RedirectToAction("Index", new { Student_NIM = student.Nim, Term_Year_Id = acd_Student_Krs.Term_Year_Id });
            }
            return View();
        }
        public ActionResult Edit(Int64 Krs_Id)
        {
            var krs = db.Acd_Student_Krs.Where(p => p.Krs_Id == Krs_Id).Single();
            var student = db.Acd_Student.Where(k => k.Student_Id == krs.Student_Id).Single();
            ViewBag.Course_Id = new SelectList(new[] { new { course_id = krs.Course_Id, course_name = krs.Acd_Course.Course_Name } }, "course_id", "course_name");
            ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == krs.Course_Id && z.Term_Year_Id == krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", krs.Class_Id);
            ViewBag.Krs_Id = krs.Krs_Id;
            ViewBag.Term_Year_Id = krs.Term_Year_Id;
            ViewBag.Student_Id = krs.Student_Id;
            ViewBag.Class_Prog_Id = krs.Class_Prog_Id;
            var CourseCostForKRS = db.usp_GetCourseCostForKRS(krs.Term_Year_Id, student.Department_Id, krs.Class_Prog_Id, student.Entry_Year_Id, krs.Course_Id).Single();
            ViewBag.Applied_Sks = CourseCostForKRS.applied_sks;
            ViewBag.Amount = CourseCostForKRS.amount;
            ViewBag.Dosen = "";
            var classInfo = db.usp_GetClassInfoForKRS(krs.Term_Year_Id, student.Department_Id, krs.Class_Prog_Id, krs.Course_Id, krs.Class_Id).Single();
            ViewBag.Kapasitas = classInfo.Capacity;
            ViewBag.Terdaftar = classInfo.Used;
            ViewBag.SisaKuota = classInfo.Free;
           // ViewBag.Curriculum_Id = db.Acd_Curriculum_Entry_Year.Where(p => p.Department_Id == student.Department_Id && p.Entry_Year_Id == student.Entry_Year_Id && p.Term_Year_Id == krs.Term_Year_Id).SingleOrDefault().Curriculum_Id;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Acd_Student_Krs acd_Student_Krs)
        {
            if (ModelState.IsValid)
            {
                var student = db.Acd_Student.Find(acd_Student_Krs.Student_Id);
                var classInfo = db.usp_GetClassInfoForKRS(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, acd_Student_Krs.Class_Id).Single();
                //var saldo = db.usp_Saldo(acd_Student_Krs.Student_Id, acd_Student_Krs.Term_Year_Id).Single();
                //decimal usedSKS = db.Acd_Student_Krs.Where(k => k.Student_Id == acd_Student_Krs.Student_Id && k.Term_Year_Id == acd_Student_Krs.Term_Year_Id).Sum(p => p.Sks);
                //decimal AllowedSKS = db.usp_GetAllowedSKSForKRS(acd_Student_Krs.Term_Year_Id, acd_Student_Krs.Student_Id).Single().Value;
                //var CourseCostForKRS = db.usp_GetCourseCostForKRS(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, acd_Student_Krs.Course_Id).Single();

                if (classInfo.Free <= 0)
                {
                    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                    ViewBag.NotifMessage = "Kelas Sudah Penuh.";
                    return View(acd_Student_Krs);
                }
                //if ((AllowedSKS - usedSKS) < CourseCostForKRS.applied_sks)
                //{
                //    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                //    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                //    ViewBag.NotifMessage = "Sisa SKS tidak mencukupi.";
                //    return View(acd_Student_Krs);
                //}
                //if (saldo.SisaSaldoSaatIni < CourseCostForKRS.amount)
                //{
                //    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, student.Department_Id, acd_Student_Krs.Class_Prog_Id, student.Entry_Year_Id, student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                //    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                //    ViewBag.NotifMessage = "Sisa Saldo tidak mencukupi.";
                //    return View(acd_Student_Krs);
                //}

                db.Acd_Student_Krs.Attach(acd_Student_Krs); // attach in the Unchanged state
                db.Entry(acd_Student_Krs).Property(r => r.Class_Id).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index", new { Student_NIM = student.Nim, Term_Year_Id = acd_Student_Krs.Term_Year_Id });
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }
            //for url
            return View(acd_Student_Krs);
        }

        // POST: CourseCurriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            Acd_Student student = db.Acd_Student.Find(acd_Student_Krs.Student_Id);
            var khscount=db.Acd_Student_Khs.Where(p=>p.Krs_Id==acd_Student_Krs.Krs_Id).Count();
            if (khscount>0)
            {
                ViewBag.NotifMessage = "Data KRS sudah terpakai di KHS.";
                return View(acd_Student_Krs);
            }
            db.Acd_Student_Krs.Remove(acd_Student_Krs);
            db.SaveChanges();
            return RedirectToAction("Index", "KRSv2", new
            {
                Student_NIM = student.Nim,
                Term_Year_Id = acd_Student_Krs.Term_Year_Id
            });
        }
        public ActionResult KelasCourse(int course_id, short Term_Year_Id, short Class_Prog_Id, long Student_Id)
        {
            var student = db.Acd_Student.Find(Student_Id);
            var CourseCostForKRS = db.usp_GetCourseCostForKRS(Term_Year_Id, student.Department_Id, Class_Prog_Id, student.Entry_Year_Id, course_id).Single();
            ViewBag.Applied_Sks = CourseCostForKRS.applied_sks;
            ViewBag.Amount = CourseCostForKRS.amount;
            ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == course_id && z.Term_Year_Id == Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name");
            return PartialView();
        }
        public ActionResult PropertiKelas(short Class_Id, int course_id, short Term_Year_Id, short Class_Prog_Id, long Student_Id)
        {
            var student = db.Acd_Student.Find(Student_Id);
            var classInfo = db.usp_GetClassInfoForKRS(Term_Year_Id, student.Department_Id, Class_Prog_Id, course_id, Class_Id).Single();
            ViewBag.Dosen = "";
            ViewBag.Kapasitas = classInfo.Capacity;
            ViewBag.Terdaftar = classInfo.Used;
            ViewBag.SisaKuota = classInfo.Free;
            return PartialView();
        }
    }
}