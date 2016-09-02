using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;

namespace SIA_Universitas.Controllers
{
    public class StudentKrsV2Controller : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: StudentKrsV2
        public ActionResult Index(string Student_NIM, short? Term_Year_Id)
        {
            List<Acd_Student_Krs> acd_Student_Krs = new List<Acd_Student_Krs>();

            ViewBag.StudentNIM = Student_NIM;
            ViewBag.TermYear = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).FirstOrDefault();

            short TermYearId = Term_Year_Id ?? 0;

            var Student = db.Acd_Student.Where(s => s.Nim == Student_NIM).FirstOrDefault();
            if (Student != null)
            {
                acd_Student_Krs = db.Acd_Student_Krs.Where(k => k.Student_Id == Student.Student_Id && k.Term_Year_Id == TermYearId).Include(a => a.Acd_Course).Include(a => a.Acd_Student).Include(a => a.Fnc_Cost_Item).Include(a => a.Mstr_Class).Include(a => a.Mstr_Class_Program).Include(a => a.Mstr_Term_Year).ToList();
                var saldo = db.usp_Saldo(Student.Student_Id, TermYearId).FirstOrDefault();

                ViewBag.Student = Student;
                ViewBag.curEntryYear = db.Acd_Curriculum_Entry_Year.Where(k => k.Term_Year_Id == TermYearId && k.Department_Id == Student.Department_Id && k.Entry_Year_Id == Student.Entry_Year_Id).FirstOrDefault();
                ViewBag.saldo = saldo;
                ViewBag.depositbisa = saldo.DepositSmtIni + saldo.SisaDepositLalu;
                ViewBag.sks = db.usp_GetAllowedSKSForKRS(TermYearId, Student.Student_Id).FirstOrDefault().Value;
                ViewBag.kkn = "";
                ViewBag.krskkn = "";
                ViewBag.saldokkn = "";
            }
            else
            {
                string message = null;
                if (Student_NIM == "") { message = "NIM Harus diisi."; }
                ViewBag.message = message ?? "NIM yang anda masukan tidak Valid.";
            }
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);

            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV2/Details/5
        public ActionResult Details(long? id)
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
            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV2/Create
        public ActionResult Create(long Student_Id, short Term_Year_Id, short Class_Prog_Id, int? Course_Id, short? Class_Id, string UrlReferrer)
        {
            var Student = db.Acd_Student.Where(k => k.Student_Id == Student_Id).Single();

            ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(Term_Year_Id, Student.Department_Id, Class_Prog_Id, Student.Entry_Year_Id, Student_Id), "Course_Id", "Course_Name", Course_Id);
            ViewBag.Class_Id = new SelectList(new[] { new { ID = "0", Name = "" } }, "ID", "Name");
            //ViewBag.Term_Year_Id = Term_Year_Id;
            //ViewBag.Student_Id = Student_Id;
            //ViewBag.Class_Prog_Id = Class_Prog_Id;
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            Acd_Student_Krs acd_Student_Krs = new Acd_Student_Krs();
            acd_Student_Krs.Term_Year_Id = Term_Year_Id;
            acd_Student_Krs.Student_Id = Student_Id;
            acd_Student_Krs.Class_Prog_Id = Class_Prog_Id;
            if (Course_Id != null)
            {
                var CourseCostForKRS = db.usp_GetCourseCostForKRS(Term_Year_Id, Student.Department_Id, Class_Prog_Id, Student.Entry_Year_Id, Course_Id).Single();
                acd_Student_Krs.Sks = Convert.ToDecimal(CourseCostForKRS.applied_sks);
                acd_Student_Krs.Amount = Convert.ToInt32(CourseCostForKRS.amount);
                ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == Course_Id && z.Term_Year_Id == Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name");
            }
            if (Class_Id != null)
            {
                var ClassInfo = db.usp_GetClassInfoForKRS(Term_Year_Id, Student.Department_Id, Class_Prog_Id, Course_Id, Class_Id).Single();
                ViewBag.Dosen = "";
                ViewBag.Kapasitas = ClassInfo.Capacity;
                ViewBag.Terdaftar = ClassInfo.Used;
                ViewBag.SisaKuota = ClassInfo.Free;
            }
            return View(acd_Student_Krs);
        }


        // POST: StudentKrsV2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Krs_Id,Student_Id,Term_Year_Id,Course_Id,Class_Prog_Id,Class_Id,Sks,Amount,Nb_Taking,Krs_Date,Due_Date,Cost_Item_Id,Is_Approved,Is_Locked,Modified_By,Modified_Date,Created_Date,Order_Id,Created_By")] Acd_Student_Krs acd_Student_Krs, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                var Student = db.Acd_Student.Find(acd_Student_Krs.Student_Id);
                var ClassInfo = db.usp_GetClassInfoForKRS(acd_Student_Krs.Term_Year_Id, Student.Department_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, acd_Student_Krs.Class_Id).Single();
                var Saldo = db.usp_Saldo(acd_Student_Krs.Student_Id, acd_Student_Krs.Term_Year_Id).Single();
                decimal UsedSKS = db.Acd_Student_Krs.Where(k => k.Student_Id == acd_Student_Krs.Student_Id && k.Term_Year_Id == acd_Student_Krs.Term_Year_Id).Select(p => p.Sks).DefaultIfEmpty().Sum();
                decimal AllowedSKS = db.usp_GetAllowedSKSForKRS(acd_Student_Krs.Term_Year_Id, acd_Student_Krs.Student_Id).Single().Value;
                var CourseCostForKRS = db.usp_GetCourseCostForKRS(acd_Student_Krs.Term_Year_Id, Student.Department_Id, acd_Student_Krs.Class_Prog_Id, Student.Entry_Year_Id, acd_Student_Krs.Course_Id).Single();

                if (ClassInfo.Free <= 0 || (AllowedSKS - UsedSKS) < CourseCostForKRS.applied_sks || Saldo.SisaSaldoSaatIni < CourseCostForKRS.amount)
                {
                    if (ClassInfo.Free <= 0) { ViewBag.NotifMessage = "Kelas Sudah Penuh."; }
                    if ((AllowedSKS - UsedSKS) < CourseCostForKRS.applied_sks) { ViewBag.NotifMessage = "Sisa SKS tidak mencukupi."; }
                    if (Saldo.SisaSaldoSaatIni < CourseCostForKRS.amount) { ViewBag.NotifMessage = "Sisa Saldo tidak mencukupi."; }
                    
                    ViewBag.Term_Year_Id = acd_Student_Krs.Term_Year_Id;
                    ViewBag.Student_Id = acd_Student_Krs.Student_Id;
                    ViewBag.Class_Prog_Id = acd_Student_Krs.Class_Prog_Id;
                    ViewBag.Course_Id = new SelectList(db.usp_GetOfferredCourseForKRSByStudent(acd_Student_Krs.Term_Year_Id, Student.Department_Id, acd_Student_Krs.Class_Prog_Id, Student.Entry_Year_Id, Student.Student_Id), "course_id", "course_name", acd_Student_Krs.Course_Id);
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
                    ViewBag.Dosen = "";
                    ViewBag.Kapasitas = ClassInfo.Capacity;
                    ViewBag.Terdaftar = ClassInfo.Used;
                    ViewBag.SisaKuota = ClassInfo.Free;
                    ViewBag.UrlReferrer = UrlReferrer;
                    
                    return View(acd_Student_Krs);
                }
                acd_Student_Krs.Created_Date = DateTime.Now;
                acd_Student_Krs.Krs_Date = DateTime.Now;
                acd_Student_Krs.Modified_Date = DateTime.Now;

                acd_Student_Krs.Cost_Item_Id = 3; //deposit KRS, nanti dikembangkan bisa 9(KKN)

                db.Acd_Student_Krs.Add(acd_Student_Krs);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV2/Edit/5
        public ActionResult Edit(long? id, short? Class_Id, string UrlReferrer)
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
            var Student = db.Acd_Student.Where(k => k.Student_Id == acd_Student_Krs.Student_Id).Single();

            ViewBag.CourseName = db.Acd_Course.Where(c => c.Course_Id == acd_Student_Krs.Course_Id).Select(c => c.Course_Name).First();
            ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            var ClassInfo = db.usp_GetClassInfoForKRS(acd_Student_Krs.Term_Year_Id, Student.Department_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, Class_Id ?? acd_Student_Krs.Class_Id).Single();
            if (ClassInfo.Free <= 0) { ViewBag.Notif = "Kelas Sudah Penuh."; }
            ViewBag.Dosen = "";
            ViewBag.Kapasitas = ClassInfo.Capacity;
            ViewBag.Terdaftar = ClassInfo.Used;
            ViewBag.SisaKuota = ClassInfo.Free;

            return View(acd_Student_Krs);
        }

        // POST: StudentKrsV2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Krs_Id,Student_Id,Term_Year_Id,Course_Id,Class_Prog_Id,Class_Id,Sks,Amount,Nb_Taking,Krs_Date,Due_Date,Cost_Item_Id,Is_Approved,Is_Locked,Modified_By,Modified_Date,Created_Date,Order_Id,Created_By")] Acd_Student_Krs acd_Student_Krs, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                var Student = db.Acd_Student.Find(acd_Student_Krs.Student_Id);
                var ClassInfo = db.usp_GetClassInfoForKRS(acd_Student_Krs.Term_Year_Id, Student.Department_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, acd_Student_Krs.Class_Id).Single();

                if (ClassInfo.Free <= 0)
                {
                    ViewBag.NotifMessage = "Kelas Sudah Penuh.";

                    ViewBag.CourseName = db.Acd_Course.Where(c => c.Course_Id == acd_Student_Krs.Course_Id).Select(c => c.Course_Name).First();
                    ViewBag.Class_Id = new SelectList(db.Acd_Offered_Course.Where(z => z.Course_Id == acd_Student_Krs.Course_Id && z.Term_Year_Id == acd_Student_Krs.Term_Year_Id), "Class_Id", "Mstr_Class.Class_Name", acd_Student_Krs.Class_Id);

                    ViewBag.Dosen = "";
                    ViewBag.Kapasitas = ClassInfo.Capacity;
                    ViewBag.Terdaftar = ClassInfo.Used;
                    ViewBag.SisaKuota = ClassInfo.Free;
                    ViewBag.UrlReferrer = UrlReferrer;

                    return View(acd_Student_Krs);
                }
                acd_Student_Krs.Modified_Date = DateTime.Now;

                db.Entry(acd_Student_Krs).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV2/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
        //    if (acd_Student_Krs == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Student_Krs);
        //}

        // POST: StudentKrsV2/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            var khscount = db.Acd_Student_Khs.Where(p => p.Krs_Id == acd_Student_Krs.Krs_Id).Count();
            if (khscount > 0)
            {
                TempData["gagalHapus"] = "Data KRS sudah terpakai di KHS.";
                return Redirect(UrlReferrer);
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            db.Acd_Student_Krs.Remove(acd_Student_Krs);
            db.SaveChanges();
            return Redirect(UrlReferrer);
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
