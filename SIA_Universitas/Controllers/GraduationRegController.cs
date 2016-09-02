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
    public class GraduationRegController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: GraduationReg
        public ActionResult Index(string currentFilter, string searchString, short? Graduation_Period_Id, short? Department_Id, string tampil, int? page, int? rowPerPage)
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
            ViewBag.Graduation_Period = Graduation_Period_Id;
            ViewBag.tampil = tampil;

            //dropdown
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id).ToList();
            ViewBag.Graduation_Period_Id = new SelectList(db.Acd_Graduation_Period.OrderByDescending(x => x.Term_Year_Id), "Graduation_Period_Id", "Period_Name", Graduation_Period_Id).ToList();

            List<Vm_GraduationReg_Standar> list_vm_GraduationReg_Standar = new List<Vm_GraduationReg_Standar>();

            if (tampil != "standar" && tampil != "lengkap" && tampil != "resume" || Graduation_Period_Id == null || Department_Id == null)
            {
                return View(list_vm_GraduationReg_Standar.ToPagedList(pageNumber, pageSize));
            }

            IQueryable<Vm_GraduationReg_Standar> acd_Graduation_Reg = queryIndex(Graduation_Period_Id, Department_Id, tampil);

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Graduation_Reg = acd_Graduation_Reg.Where(s => s.Nim.Contains(searchString) || s.Full_Name.Contains(searchString));
            }

            if (acd_Graduation_Reg != null)
            {
                if (tampil == "resume")
                {
                    ViewBag.rataIpk = Convert.ToDecimal(acd_Graduation_Reg.Average(x => x.Gpa));
                    ViewBag.rataUmur = Convert.ToDecimal(acd_Graduation_Reg.Average(x => x.Age_Year));
                    ViewBag.rataStudy = Convert.ToDecimal(acd_Graduation_Reg.Average(x => x.Total_Smt_Study));

                    acd_Graduation_Reg = acd_Graduation_Reg.OrderByDescending(x => x.Gpa).ThenByDescending(x => x.Nim);
                }
                else
                {
                    acd_Graduation_Reg = acd_Graduation_Reg.OrderBy(x => x.Nim);
                }
            }

            return View(acd_Graduation_Reg.ToPagedList(pageNumber, pageSize));
        }

        private IQueryable<Vm_GraduationReg_Standar> queryIndex(short? Graduation_Period_Id, short? Department_Id, string tampil)
        {
            IQueryable<Vm_GraduationReg_Standar> QviewModel = null;
            switch (tampil)
            {
                case "resume":
                    short? Faculty_Id = db.Mstr_Department.Where(x => x.Department_Id == Department_Id).Select(x => x.Faculty_Id).FirstOrDefault();

                    QviewModel = from a in db.Acd_Graduation_Reg
                                 join b in db.Acd_Student on a.Student_Id equals b.Student_Id

                                 join c in db.Acd_Graduation_Reg_Temp on a.Student_Id equals c.Student_Id into grt
                                 from cx in grt.DefaultIfEmpty()

                                 //join d in db.Mstr_Graduate_Predicate on a.Graduate_Predicate_Id equals d.Graduate_Predicate_Id into mgp
                                 //from dx in mgp.DefaultIfEmpty()

                                 join e in db.Mstr_Register_Status on a.Register_Status_Id equals e.Register_Status_Id into mrs
                                 from ex in mrs.DefaultIfEmpty()

                                 join f in db.Mstr_Department on b.Department_Id equals f.Department_Id into md
                                 from fx in md.DefaultIfEmpty()

                                 join g in db.Mstr_Faculty on fx.Faculty_Id equals g.Faculty_Id into mf
                                 from gx in mf.DefaultIfEmpty()

                                 join h in db.Acd_Graduation_Period on a.Graduation_Periode_Id equals h.Graduation_Period_Id into agp
                                 from hx in agp.DefaultIfEmpty()

                                 join i in db.Acd_Yudisium on a.Student_Id equals i.Student_Id into ay
                                 from ix in ay.DefaultIfEmpty()

                                 join i1 in db.Mstr_Graduate_Predicate on ix.Graduate_Predicate_Id equals i1.Graduate_Predicate_Id into mgp
                                 from i1x in mgp.DefaultIfEmpty()

                                 where a.Graduation_Periode_Id == Graduation_Period_Id
                                    && fx.Faculty_Id == Faculty_Id

                                 select new Vm_GraduationReg_Standar
                                 {
                                     Graduation_Reg_Id = a.Graduation_Reg_Id,
                                     Student_Id = b.Student_Id,
                                     Nim = b.Nim,
                                     Full_Name = b.Full_Name,
                                     Gpa = cx.Gpa,
                                     Predicate_Name = i1x.Predicate_Name,//dx.Predicate_Name,
                                     Total_Smt_Study = ix.Study_Smt_Length,
                                     Age_Year = ix.Age_Year,
                                     Register_Status_Name = ex.Register_Status_Name,
                                     Yudisium_Date = cx.Yudisium_Date,
                                     Faculty_Name = gx.Faculty_Name,
                                     Period_Name = hx.Period_Name
                                 };
                    break;
                case "standar":
                    QviewModel = from a in db.Acd_Graduation_Reg
                                join b in db.Acd_Student on a.Student_Id equals b.Student_Id

                                join b1 in db.Mstr_Gender on b.Gender_Id equals b1.Gender_Id into genderGroup
                                from b1x in genderGroup.DefaultIfEmpty()

                                join c in db.Acd_Graduation_Reg_Temp on a.Student_Id equals c.Student_Id into grt
                                from cx in grt.DefaultIfEmpty()

                                //join d in db.Mstr_Graduate_Predicate on a.Graduate_Predicate_Id equals d.Graduate_Predicate_Id into mgp
                                //from dx in mgp.DefaultIfEmpty()

                                join e in db.Mstr_Register_Status on a.Register_Status_Id equals e.Register_Status_Id into mrs
                                from ex in mrs.DefaultIfEmpty()

                                join f in db.Acd_Yudisium on a.Student_Id equals f.Student_Id into ay
                                from fx in ay.DefaultIfEmpty()

                                join f1 in db.Mstr_Graduate_Predicate on fx.Graduate_Predicate_Id equals f1.Graduate_Predicate_Id into mgp
                                from f1x in mgp.DefaultIfEmpty()

                                join g in db.Acd_Graduation_Final on a.Student_Id equals g.Student_Id into agf
                                from gx in agf.DefaultIfEmpty()

                                join b2 in db.Mstr_Term on b.Entry_Term_Id equals b2.Term_Id into mt
                                from b2x in mt.DefaultIfEmpty()

                                 

                                where a.Graduation_Periode_Id == Graduation_Period_Id
                                    && b.Department_Id == Department_Id
                                select new Vm_GraduationReg_Standar
                                {
                                    Graduation_Reg_Id = a.Graduation_Reg_Id,
                                    Student_Id = b.Student_Id,
                                    Nim = b.Nim,
                                    Full_Name = b.Full_Name,
                                    Birth_Place = b.Birth_Place,
                                    Birth_Date = b.Birth_Date,
                                    Gender_Type = b1x.Gender_Type,
                                    Gpa = cx.Gpa,
                                    Yudisium_Date = fx.Yudisium_Date,
                                    Total_Smt_Vacation = fx.Study_Smt_Off_Length,
                                    Term_Name = b2x.Term_Name,
                                    Total_Smt_Study = fx.Study_Smt_Length,
                                    Age_Year = fx.Age_Year,
                                    Predicate_Name = f1x.Predicate_Name,
                                    Register_Status_Name = ex.Register_Status_Name,
                                    Email = cx.Email,
                                    Phone = cx.Phone,
                                    Parent_Name = cx.Parent_Name,
                                    Address_0 = cx.Address_0,
                                    Thesis_Title = cx.Thesis_Title,
                                    Thesis_Title_Eng = cx.Thesis_Title_Eng,
                                    Sk_Num = fx.Sk_Num,
                                    Transcript_Num = fx.Transcript_Num,
                                    Certificate_Serial_Full = gx.Certificate_Serial_Full,
                                };
                    break;
                case "lengkap":
                    QviewModel = from a in db.Acd_Graduation_Reg
                                 join b in db.Acd_Student on a.Student_Id equals b.Student_Id

                                 join b1 in db.Mstr_Gender on b.Gender_Id equals b1.Gender_Id into genderGroup
                                 from b1x in genderGroup.DefaultIfEmpty()

                                 join c in db.Acd_Graduation_Reg_Temp on a.Student_Id equals c.Student_Id into grt
                                 from cx in grt.DefaultIfEmpty()

                                 //join d in db.Mstr_Graduate_Predicate on a.Graduate_Predicate_Id equals d.Graduate_Predicate_Id into mgp
                                 //from dx in mgp.DefaultIfEmpty()

                                 join e in db.Mstr_Register_Status on a.Register_Status_Id equals e.Register_Status_Id into mrs
                                 from ex in mrs.DefaultIfEmpty()

                                 join f in db.Acd_Yudisium on a.Student_Id equals f.Student_Id into ay
                                 from fx in ay.DefaultIfEmpty()

                                 join f1 in db.Mstr_Graduate_Predicate on fx.Graduate_Predicate_Id equals f1.Graduate_Predicate_Id into mgp
                                 from f1x in mgp.DefaultIfEmpty()

                                 join g in db.Acd_Graduation_Final on a.Student_Id equals g.Student_Id into agf
                                 from gx in agf.DefaultIfEmpty()

                                 join b2 in db.Mstr_Term on b.Entry_Term_Id equals b2.Term_Id into mt
                                 from b2x in mt.DefaultIfEmpty()

                                 //lengkap
                                 //=================================
                                 join h in db.Acd_Thesis on a.Student_Id equals h.Student_Id into at
                                 from hx in at.DefaultIfEmpty()

                                 join h1 in db.Emp_Employee on hx.Supervisor_1 equals h1.Employee_Id into empSup1
                                 from h1x in empSup1.DefaultIfEmpty()

                                 join h2 in db.Emp_Employee on hx.Supervisor_2 equals h2.Employee_Id into empSup2
                                 from h2x in empSup2.DefaultIfEmpty()

                                 join h3 in db.Emp_Employee on hx.Examiner_1 equals h3.Employee_Id into empExam1
                                 from h3x in empExam1.DefaultIfEmpty()

                                 join h4 in db.Emp_Employee on hx.Examiner_2 equals h4.Employee_Id into empExam2
                                 from h4x in empExam2.DefaultIfEmpty()
                                 //=================================

                                 where a.Graduation_Periode_Id == Graduation_Period_Id
                                     && b.Department_Id == Department_Id
                                 select new Vm_GraduationReg_Standar
                                 {
                                     Graduation_Reg_Id = a.Graduation_Reg_Id,
                                     Student_Id = b.Student_Id,
                                     Nim = b.Nim,
                                     Full_Name = b.Full_Name,
                                     Birth_Place = b.Birth_Place,
                                     Birth_Date = b.Birth_Date,
                                     Gender_Type = b1x.Gender_Type,
                                     Gpa = cx.Gpa,
                                     Yudisium_Date = fx.Yudisium_Date,
                                     Total_Smt_Vacation = fx.Study_Smt_Off_Length,
                                     Term_Name = b2x.Term_Name,
                                     Total_Smt_Study = fx.Study_Smt_Length,
                                     Age_Year = fx.Age_Year,
                                     Predicate_Name = f1x.Predicate_Name,
                                     Register_Status_Name = ex.Register_Status_Name,
                                     Email = cx.Email,
                                     Phone = cx.Phone,
                                     Parent_Name = cx.Parent_Name,
                                     Address_0 = cx.Address_0,
                                     Thesis_Title = cx.Thesis_Title,
                                     Thesis_Title_Eng = cx.Thesis_Title_Eng,
                                     Sk_Num = fx.Sk_Num,
                                     Transcript_Num = fx.Transcript_Num,
                                     Certificate_Serial_Full = gx.Certificate_Serial_Full,

                                     //lengkap
                                     //=================================
                                     DosenPemb1 = h1x.Full_Name,
                                     DosenPemb2 = h2x.Full_Name,
                                     DosenPenguji1 = h3x.Full_Name,
                                     DosenPenguji2 = h4x.Full_Name,
                                     Application_Date = hx.Application_Date,
                                     Thesis_Exam_Date = hx.Thesis_Exam_Date,
                                     Grade = hx.Grade
                                     //=================================
                                 };
                    break;
            }

            return QviewModel;
        }

        // GET: GraduationReg/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Graduation_Reg acd_Graduation_Reg = db.Acd_Graduation_Reg.Find(id);
        //    if (acd_Graduation_Reg == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Graduation_Reg);
        //}

        // GET: GraduationReg/Create
        public ActionResult Create(string currentFilter, string searchString, short? Graduation_Period_Id, short? Department_Id, string tampil, int? page, int? rowPerPage)
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
            ViewBag.Graduation_Period = Graduation_Period_Id;
            ViewBag.tampil = tampil;

            ViewBag.nav = db.Mstr_Department.Where(x => x.Department_Id == Department_Id).Select(x => x.Department_Name).Single();
            ViewBag.nav1 = db.Acd_Graduation_Period.Where(x => x.Graduation_Period_Id == Graduation_Period_Id).Select(x => x.Period_Name).Single();

            IQueryable<Vm_GraduationReg_Standar> acd_Graduation_Reg_Temp =  from a in db.Acd_Graduation_Reg_Temp
                                                                            join b in db.Acd_Student on a.Student_Id equals b.Student_Id
                                                                            join c in db.Acd_Graduation_Reg on a.Student_Id equals c.Student_Id into agr
                                                                            from cx in agr.Where(x=> x.Graduation_Periode_Id == a.Graduate_Periode_Id).DefaultIfEmpty() 
                                                                            where 
                                                                                a.Graduate_Periode_Id == Graduation_Period_Id
                                                                                && b.Department_Id == Department_Id
                                                                                && cx.Student_Id == null
                                                                            select new Vm_GraduationReg_Standar
                                                                            {
                                                                                Graduation_Reg_Temp_Id = a.Graduation_Reg_Temp_Id,
                                                                                Student_Id = a.Student_Id,
                                                                                Nim = b.Nim,
                                                                                Full_Name = a.Full_Name
                                                                            };

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Graduation_Reg_Temp = acd_Graduation_Reg_Temp.Where(x => x.Nim.Contains(searchString) || x.Full_Name.Contains(searchString));
            }
            acd_Graduation_Reg_Temp = acd_Graduation_Reg_Temp.OrderBy(x => x.Nim);

            return View(acd_Graduation_Reg_Temp.ToPagedList(pageNumber, pageSize));
        }

        // POST: GraduationReg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<long> checkGraduation_Reg_Temp_IdToAdd, short Graduation_Period_Id, short? Department_Id, string tampil)
        {
            if (ModelState.IsValid)
            {
                if (checkGraduation_Reg_Temp_IdToAdd != null)
                {
                    foreach (var i in checkGraduation_Reg_Temp_IdToAdd)
                    {
                        Acd_Graduation_Reg reg = new Acd_Graduation_Reg();

                        var acd_Graduation_Reg_Temp = db.Acd_Graduation_Reg_Temp.Where(x => x.Graduation_Reg_Temp_Id == i)
                                    .Select( x=> new 
                                    {   
                                     Student_Id = x.Student_Id,
                                     Register_Status_Id = x.Acd_Student.Register_Status_Id
                                    }).FirstOrDefault();

                        reg.Student_Id = acd_Graduation_Reg_Temp.Student_Id;
                        reg.Graduation_Periode_Id = Graduation_Period_Id;
                        reg.Register_Status_Id = acd_Graduation_Reg_Temp.Register_Status_Id;

                        db.Acd_Graduation_Reg.Add(reg);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index", new { Graduation_Period_Id = Graduation_Period_Id, Department_Id = Department_Id, tampil = tampil });
        }

        // GET: GraduationReg/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Graduation_Reg acd_Graduation_Reg = db.Acd_Graduation_Reg.Find(id);
            if (acd_Graduation_Reg == null)
            {
                return HttpNotFound();
            }

            IQueryable<Vm_GraduationReg_Standar> vm_GraduationReg_Standar = from a in db.Acd_Graduation_Reg
                                 join b in db.Acd_Student on a.Student_Id equals b.Student_Id

                                 join b1 in db.Mstr_Gender on b.Gender_Id equals b1.Gender_Id into genderGroup
                                 from b1x in genderGroup.DefaultIfEmpty()

                                 join c in db.Acd_Graduation_Reg_Temp on a.Student_Id equals c.Student_Id into grt
                                 from cx in grt.DefaultIfEmpty()

                                 //join d in db.Mstr_Graduate_Predicate on a.Graduate_Predicate_Id equals d.Graduate_Predicate_Id into mgp
                                 //from dx in mgp.DefaultIfEmpty()

                                 join e in db.Mstr_Register_Status on a.Register_Status_Id equals e.Register_Status_Id into mrs
                                 from ex in mrs.DefaultIfEmpty()

                                 join f in db.Acd_Yudisium on a.Student_Id equals f.Student_Id into ay
                                 from fx in ay.DefaultIfEmpty()

                                 join f1 in db.Mstr_Graduate_Predicate on fx.Graduate_Predicate_Id equals f1.Graduate_Predicate_Id into mgp
                                 from f1x in mgp.DefaultIfEmpty()

                                 //join g in db.Acd_Graduation_Final on a.Student_Id equals g.Student_Id into agf
                                 //from gx in agf.DefaultIfEmpty()

                                 join b2 in db.Mstr_Term on b.Entry_Term_Id equals b2.Term_Id into mt
                                 from b2x in mt.DefaultIfEmpty()

                                 //lengkap
                                 //=================================
                                 join h in db.Acd_Thesis on a.Student_Id equals h.Student_Id into at
                                 from hx in at.DefaultIfEmpty()

                                 //join h1 in db.Emp_Employee on hx.Supervisor_1 equals h1.Employee_Id into empSup1
                                 //from h1x in empSup1.DefaultIfEmpty()

                                 //join h2 in db.Emp_Employee on hx.Supervisor_2 equals h2.Employee_Id into empSup2
                                 //from h2x in empSup2.DefaultIfEmpty()

                                 //join h3 in db.Emp_Employee on hx.Examiner_1 equals h3.Employee_Id into empExam1
                                 //from h3x in empExam1.DefaultIfEmpty()

                                 //join h4 in db.Emp_Employee on hx.Examiner_2 equals h4.Employee_Id into empExam2
                                 //from h4x in empExam2.DefaultIfEmpty()
                                 //=================================

                                 where a.Graduation_Reg_Id == id
                                 select new Vm_GraduationReg_Standar
                                 {
                                    Graduation_Reg_Id = a.Graduation_Reg_Id,
                                    Student_Id = b.Student_Id,
                                    Nim = b.Nim,
                                    Entry_Year_Code = b.Mstr_Entry_Year.Entry_Year_Code,
                                    Term_Name = b2x.Term_Name,
                                    Full_Name = b.Full_Name,
                                    Birth_Place = b.Birth_Place,
                                    Birth_Date = b.Birth_Date,
                                    Gender_Type = b1x.Gender_Type,
                                    Thesis_Exam_Date = hx.Thesis_Exam_Date,
                                    Yudisium_Date = fx.Yudisium_Date,
                                    Gpa = cx.Gpa,
                                    Total_Smt_Vacation = fx.Study_Smt_Off_Length,
                                    Thesis_Title = cx.Thesis_Title,
                                    Address_0 = cx.Address_0,
                                    Phone = cx.Phone,
                                    Email = cx.Email,
                                    Parent_Name = cx.Parent_Name,
                                    Register_Status_Name = ex.Register_Status_Name,
                                    Sk_Num = fx.Sk_Num,
                                    Transcript_Num = fx.Transcript_Num,
                                 };

            return View(vm_GraduationReg_Standar.FirstOrDefault());
        }

        // POST: GraduationReg/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Graduation_Reg_Id,Graduation_Periode_Id,Student_Id,Age_Year,Age_Month,Age_Day,Age_Year_Length,Age_Day_Length,Study_Length_Year,Study_Length_Month,Study_Length_Day,Study_Length_Sum_Year,Study_Length_Sum_Day,Total_Smt_Vacation,Total_Smt_Study,Register_Status_Id,Reg_Date,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Graduation_Reg acd_Graduation_Reg)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(acd_Graduation_Reg).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Graduation_Periode_Id = new SelectList(db.Acd_Graduation_Period, "Graduation_Period_Id", "Period_Name", acd_Graduation_Reg.Graduation_Periode_Id);
        //    ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name", acd_Graduation_Reg.Register_Status_Id);
        //    return View(acd_Graduation_Reg);
        //}

        // GET: GraduationReg/Delete/5
        public ActionResult Delete(long? id, string tampil)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Graduation_Reg acd_Graduation_Reg = db.Acd_Graduation_Reg.Find(id);
            if (acd_Graduation_Reg == null)
            {
                return HttpNotFound();
            }
            ViewBag.tampil = tampil;
            return View(acd_Graduation_Reg);
        }

        // POST: GraduationReg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, string tampil)
        {
            Acd_Graduation_Reg acd_Graduation_Reg = db.Acd_Graduation_Reg.Find(id);
            var Department_Id = acd_Graduation_Reg.Acd_Student.Department_Id;
            var Graduation_Periode_Id = acd_Graduation_Reg.Graduation_Periode_Id;
            db.Acd_Graduation_Reg.Remove(acd_Graduation_Reg);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                Graduation_Period_Id = Graduation_Periode_Id,
                Department_Id = Department_Id,
                tampil = tampil
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
