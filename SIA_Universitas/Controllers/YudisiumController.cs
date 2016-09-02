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
using SIA_Universitas.Helpers;

namespace SIA_Universitas.Controllers
{
    public class YudisiumController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Yudisium
        public ActionResult Index(short? Term_Year_Id, short? currentTermYear, short? Department_Id, short? currentDept, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null){ page = 1; }else{ searchString = currentFilter; }
            if (Term_Year_Id != null) { page = 1; } else { Term_Year_Id = currentTermYear; }
            if (Department_Id != null) { page = 1; } else { Department_Id = currentDept; }
            
            ViewBag.CurrentFilter = searchString;
            ViewBag.currentTermYear = Term_Year_Id;
            ViewBag.CurrentDept = Department_Id;
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            var acd_Yudisium = db.Acd_Yudisium.Where(a => a.Acd_Student.Department_Id == Department_Id && a.Term_Year_Id == Term_Year_Id).Include(a => a.Acd_Graduation_Period).Include(a => a.Acd_Student).Include(a => a.Mstr_Graduate_Predicate).Include(a => a.Mstr_Term_Year);
            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Yudisium = acd_Yudisium.Where(s => s.Acd_Student.Nim.Contains(searchString) || s.Acd_Student.Full_Name.Contains(searchString));
            }
            acd_Yudisium = acd_Yudisium.OrderByDescending(s => s.Acd_Student.Nim);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(acd_Yudisium.ToPagedList(pageNumber, pageSize));
        }

        //Function Proses
        public ActionResult Proses(int proses, long Student_Id)
        {
            string method = string.Empty;
            switch (proses)
            {
                case 1:
                    {
                        method = "SuratPermohonanYudisium";
                    }
                    break;
                case 2:
                    {
                        method = "SuratBebasPinjamanLab";
                    }
                    break;
                case 3:
                    {
                        method = "BeritaAcaraYudisium";
                    }
                    break;
                case 4:
                    {
                        method = "PengantarPembayaranWisuda";
                    }
                    break;
                case 5:
                    {
                        method = "SuratKeteranganLulus";
                    }
                    break;
                case 6:
                    {
                        method = "CetakTranskrip";
                    }
                    break;
                case 7:
                    {
                        method = "CetakBuktiPenyerahanTA";
                    }
                    break;
            }

            TempData["StudentId"] = Student_Id;
            return RedirectToAction(method);
        }

        //int proses 1
        public ActionResult SuratPermohonanYudisium()
        {
            long StudentId = Convert.ToInt64(TempData["StudentId"]);
            return View();
        }

        //int proses 2
        public ActionResult SuratBebasPinjamanLab()
        {
            long StudentId = Convert.ToInt64(TempData["StudentId"]);
            return View();
        }

        //int proses 3
        public ActionResult BeritaAcaraYudisium(int? Employee_Id, long? Student_Id, DateTime? tglY, string no, string namaJab, short? GraduatePredicateId, bool? IsGraduated)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            ViewBag.OriUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToString();

            string DeptFunc = (string)Session["DeptFunc"];

            Emp_Employee emp_Employee = new Emp_Employee();
            if (Employee_Id != null)
            {
                emp_Employee = db.Emp_Employee.Find(Employee_Id);
            }

            long StudentId = Convert.ToInt64(TempData["StudentId"] ?? Student_Id);
            Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(StudentId);
            acd_Yudisium.Yudisium_Date = tglY ?? acd_Yudisium.Yudisium_Date;
            acd_Yudisium.Sk_Num = no ?? acd_Yudisium.Sk_Num;
            if (acd_Yudisium.Department_Functionary == null)
            {
                acd_Yudisium.Department_Functionary = namaJab ?? DeptFunc;
            }
            acd_Yudisium.Department_Functionary_Name = emp_Employee.Full_Name ?? acd_Yudisium.Department_Functionary_Name;
            acd_Yudisium.Department_Functionary_Nik = emp_Employee.Nik ?? acd_Yudisium.Department_Functionary_Nik;

            //View Data
            var acd_Student = new Acd_Student();
            acd_Student = db.Acd_Student.Where(s => s.Student_Id == StudentId).FirstOrDefault();
            ViewBag.Nim = acd_Student.Nim;
            ViewBag.Name = acd_Student.Full_Name;
            ViewBag.Department_Id = acd_Student.Department_Id;

            //viewbag for dropdown Is_Graduated
            List<SelectListItem> obj = new List<SelectListItem>();
            obj.Add(new SelectListItem { Text = "Lulus", Value = "true" });
            obj.Add(new SelectListItem { Text = "Tidak lulus", Value = "false" });
            ViewBag.Is_Graduated = new SelectList(obj, "Value", "Text", IsGraduated ?? acd_Yudisium.Is_Graduated);
            //viewbag for dropdown Graduate_Predicate
            ViewBag.Graduate_Predicate_Id = new SelectList(db.Mstr_Graduate_Predicate, "Graduate_Predicate_Id", "Predicate_Name", GraduatePredicateId ?? acd_Yudisium.Graduate_Predicate_Id);

            return View(acd_Yudisium);
        }

        //int proses 4
        public ActionResult PengantarPembayaranWisuda()
        {
            long StudentId = Convert.ToInt64(TempData["StudentId"]);
            return View();
        }

        //int proses 5
        public ActionResult SuratKeteranganLulus(int? Employee_Id, long? Student_Id, DateTime? tglS, string namaJab)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            ViewBag.OriUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToString();

            string facFunc = (string)Session["FacFunc"];

            Emp_Employee emp_Employee = new Emp_Employee();
            if (Employee_Id != null)
            {
                emp_Employee = db.Emp_Employee.Find(Employee_Id);
            }

            long StudentId = Convert.ToInt64(TempData["StudentId"] ?? Student_Id);
            Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(StudentId);
            acd_Yudisium.Sk_Date = tglS ?? acd_Yudisium.Sk_Date;
            if (acd_Yudisium.Faculty_Functionary == null)
            {
                acd_Yudisium.Faculty_Functionary = namaJab ?? facFunc;
            }
            acd_Yudisium.Faculty_Functionary_Name = emp_Employee.Full_Name ?? acd_Yudisium.Faculty_Functionary_Name;
            acd_Yudisium.Faculty_Functionary_Nik = emp_Employee.Nik ?? acd_Yudisium.Faculty_Functionary_Nik;

            //View Data
            var acd_Student = new Acd_Student();
            acd_Student = db.Acd_Student.Where(s => s.Student_Id == StudentId).FirstOrDefault();
            ViewBag.Nim = acd_Student.Nim;
            ViewBag.Name = acd_Student.Full_Name;
            ViewBag.Department_Id = acd_Student.Department_Id;

            return View(acd_Yudisium);
        }

        //int proses 6
        public ActionResult CetakTranskrip()
        {
            long StudentId = Convert.ToInt64(TempData["StudentId"]);
            return View();
        }

        //int proses 7
        public ActionResult CetakBuktiPenyerahanTA()
        {
            long StudentId = Convert.ToInt64(TempData["StudentId"]);
            return View();
        }

        // GET: Yudisium/Details/5
        public ActionResult Details(long? id)
        {

            ViewBag.proses = new string[]{
                "Surat Permohonan Yudisium",//proses 1
                "Surat bebas pinjaman lab",//proses 2
                "Berita acara yudisium",//proses 3
                "Pengantar pembayaran wisuda",//proses 4
                "Surat keterangan lulus",//proses 5
                "Cetak transkrip",//proses 6
                "Cetak bukti penyerahan TA"//proses 7
            };

            //View Data
            var acd_Student = new Acd_Student();
            acd_Student = db.Acd_Student.Where(s => s.Student_Id == id).FirstOrDefault();
            ViewBag.Nim = acd_Student.Nim;
            ViewBag.Name = acd_Student.Full_Name;

            //View Data
            var acd_Thesis = new Acd_Thesis();
            acd_Thesis = db.Acd_Thesis.Where(th => th.Student_Id == id).FirstOrDefault();
            if (acd_Thesis != null)
            {
                ViewBag.judul = acd_Thesis.Thesis_Title;
                ViewBag.judul_Eng = acd_Thesis.Thesis_Title_Eng;
                ViewBag.dosenPemb1 = acd_Thesis.Emp_Employee.Full_Name;
                ViewBag.dosenPemb2 = acd_Thesis.Emp_Employee1.Full_Name;
                ViewBag.tglSeminar = string.Format("{0:dddd, d MMMM yyyy}", acd_Thesis.Seminar_Date);
                ViewBag.tglPendadaran = string.Format("{0:dddd, d MMMM yyyy}", acd_Thesis.Thesis_Exam_Date);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(id);
            if (acd_Yudisium == null)
            {
                return HttpNotFound();
            }
            ViewBag.currentTermYear = acd_Yudisium.Term_Year_Id;
            ViewBag.CurrentDept = acd_Student.Department_Id;

            return View(acd_Yudisium);
        }

        public ActionResult GetEmployees(string searchTerm, int pageSize, int pageNum)
        {
            EmployeeRepository er = new EmployeeRepository();
            List<Emp_Employee> employees = er.GetEmployees(searchTerm, pageSize, pageNum);
            int employeeCount = er.GetEmployeesCount(searchTerm, pageSize, pageNum);

            Select2PagedResult pageEmployees = EmployeesToSelect2Format(employees, employeeCount);

            return new JsonpResult
            {
                Data = pageEmployees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetStudents(short deptId, string searchTerm, int pageSize, int pageNum)
        {
            StudentRepository sr = new StudentRepository();
            List<Acd_Student> students = sr.GetStudents(deptId, searchTerm, pageSize, pageNum);
            int studentCount = sr.GetStudentsCount(deptId, searchTerm, pageSize, pageNum);

            Select2PagedResult pageStudents = StudentsToSelect2Format(students, studentCount);

            return new JsonpResult
            {
                Data = pageStudents,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult EmployeesToSelect2Format(List<Emp_Employee> employees, int totalEmployees)
        {
            Select2PagedResult jsonEmployees = new Select2PagedResult();
            jsonEmployees.Results = new List<Select2Result>();

            foreach (Emp_Employee e in employees)
            {
                jsonEmployees.Results.Add(new Select2Result { id = e.Employee_Id.ToString(), text = e.Full_Name });
            }

            jsonEmployees.Total = totalEmployees;

            return jsonEmployees;
        }

        private Select2PagedResult StudentsToSelect2Format(List<Acd_Student> students, int totalStudents)
        {
            Select2PagedResult jsonStudents = new Select2PagedResult();
            jsonStudents.Results = new List<Select2Result>();

            foreach (Acd_Student s in students)
            {
                jsonStudents.Results.Add(new Select2Result { id = s.Student_Id.ToString(), text = s.Full_Name + " " + s.Nim });
            }

            jsonStudents.Total = totalStudents;

            return jsonStudents;
        }

        // GET: Yudisium/Create
        public ActionResult Create(short idDept, short idTermYear, long Student_Id)
        {
            ViewBag.currentTermYear = idTermYear;
            ViewBag.CurrentDept = idDept;

            //View Data
            var acd_Student = new Acd_Student();
            acd_Student = db.Acd_Student.Where(s => s.Student_Id == Student_Id).FirstOrDefault();
            ViewBag.Nim = acd_Student.Nim;
            ViewBag.Name = acd_Student.Full_Name;

            //View Data
            var acd_Thesis = new Acd_Thesis();
            acd_Thesis = db.Acd_Thesis.Where(th => th.Student_Id == Student_Id).FirstOrDefault();
            if (acd_Thesis != null)
            {
                ViewBag.judul = acd_Thesis.Thesis_Title;
                ViewBag.judul_Eng = acd_Thesis.Thesis_Title_Eng;
                ViewBag.dosenPemb1 = acd_Thesis.Emp_Employee.Full_Name;
                ViewBag.dosenPemb2 = acd_Thesis.Emp_Employee1.Full_Name;
                ViewBag.tglSeminar = string.Format("{0:dddd, d MMMM yyyy}", acd_Thesis.Seminar_Date);
                ViewBag.tglPendadaran = string.Format("{0:dddd, d MMMM yyyy}", acd_Thesis.Thesis_Exam_Date);
            }

            //Input Data
            var acd_yudisium = new Acd_Yudisium();
            var acd_transcript = db.Acd_Transcript.Where(t => t.Student_Id == Student_Id).ToList();

            acd_yudisium.Student_Id = Student_Id;
            acd_yudisium.Term_Year_Id = idTermYear;
            if (acd_transcript.Count() != 0)
            {
                acd_yudisium.Sks_Total = Math.Round(Convert.ToDecimal(acd_transcript.Sum(t => t.Sks)), 0);
                acd_yudisium.Course_Count = Convert.ToByte(acd_transcript.Count());
                acd_yudisium.Bnk = acd_transcript.Sum(t => t.Bnk_Value);
                acd_yudisium.Gpa = Math.Round(Convert.ToDecimal(acd_yudisium.Bnk / acd_yudisium.Sks_Total), 2);
            }
            acd_yudisium.Application_Date = DateTime.Now;


            
            //ViewBag.Graduation_Period_Id = new SelectList(db.Acd_Graduation_Period, "Graduation_Period_Id", "Period_Name");
            //ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim");
            //ViewBag.Graduate_Predicate_Id = new SelectList(db.Mstr_Graduate_Predicate, "Graduate_Predicate_Id", "Predicate_Name");
            //ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name");
            return View(acd_yudisium);
        }

        // POST: Yudisium/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Student_Id,Term_Year_Id,Sks_Total,Course_Count,Bnk,Gpa,Sk_Num,Sk_Date,Yudisium_Date,Graduate_Date,Application_Date,Is_Graduated,Graduate_Predicate_Id,Transcript_Num,Transcript_Date,Department_Functionary,Department_Functionary_Name,Department_Functionary_Nik,Faculty_Functionary,Faculty_Functionary_Name,Faculty_Functionary_Nik,Description,Graduation_Period_Id,Age_Year,Age_Month,Age_Day,Age_Year_Length,Age_Day_Length,Study_Length_Year,Study_Length_Month,Study_Length_Day,Study_Length_Sum_Year,Study_Length_Sum_Day,Study_Smt_Length,Study_Smt_Off_Length,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Yudisium acd_Yudisium)
        {
            acd_Yudisium.Gpa = Math.Round(Convert.ToDecimal(acd_Yudisium.Bnk / acd_Yudisium.Sks_Total), 2);
            var idDept = db.Acd_Student.Where(s => s.Student_Id == acd_Yudisium.Student_Id).Select(s => s.Department_Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Acd_Yudisium.Add(acd_Yudisium);
                db.SaveChanges();
                return RedirectToAction("Index", new { Term_Year_Id = acd_Yudisium.Term_Year_Id, Department_Id = idDept});
            }

            //ViewBag.Graduation_Period_Id = new SelectList(db.Acd_Graduation_Period, "Graduation_Period_Id", "Period_Name", acd_Yudisium.Graduation_Period_Id);
            //ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Yudisium.Student_Id);
            //ViewBag.Graduate_Predicate_Id = new SelectList(db.Mstr_Graduate_Predicate, "Graduate_Predicate_Id", "Predicate_Name", acd_Yudisium.Graduate_Predicate_Id);
            //ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", acd_Yudisium.Term_Year_Id);
            return RedirectToAction("Create", new { idTermYear = acd_Yudisium.Term_Year_Id, idDept = idDept, Student_Id = acd_Yudisium.Student_Id });
        }

        // GET: Yudisium/Edit/5
        //public ActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(id);
        //    if (acd_Yudisium == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Graduation_Period_Id = new SelectList(db.Acd_Graduation_Period, "Graduation_Period_Id", "Period_Name", acd_Yudisium.Graduation_Period_Id);
        //    ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Yudisium.Student_Id);
        //    ViewBag.Graduate_Predicate_Id = new SelectList(db.Mstr_Graduate_Predicate, "Graduate_Predicate_Id", "Predicate_Name", acd_Yudisium.Graduate_Predicate_Id);
        //    ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", acd_Yudisium.Term_Year_Id);
        //    return View(acd_Yudisium);
        //}

        // POST: Yudisium/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Student_Id,Term_Year_Id,Sks_Total,Course_Count,Bnk,Gpa,Sk_Num,Sk_Date,Yudisium_Date,Graduate_Date,Application_Date,Is_Graduated,Graduate_Predicate_Id,Transcript_Num,Transcript_Date,Department_Functionary,Department_Functionary_Name,Department_Functionary_Nik,Faculty_Functionary,Faculty_Functionary_Name,Faculty_Functionary_Nik,Description,Graduation_Period_Id,Age_Year,Age_Month,Age_Day,Age_Year_Length,Age_Day_Length,Study_Length_Year,Study_Length_Month,Study_Length_Day,Study_Length_Sum_Year,Study_Length_Sum_Day,Study_Smt_Length,Study_Smt_Off_Length,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Yudisium acd_Yudisium, string OriUrl)
        {
            if (acd_Yudisium.Study_Length_Year == null)
            {
                var EntryYearStudent = db.Acd_Student.Where(s => s.Student_Id == acd_Yudisium.Student_Id).Select(s => s.Entry_Year_Id).First();
                var EntryTermStudent = db.Acd_Student.Where(s => s.Student_Id == acd_Yudisium.Student_Id).Select(s => s.Entry_Term_Id).First();
                DateTime? EntryDate = null;
                if (EntryTermStudent == 1)
                {
                    string d = "01/09/";
                    EntryDate = Convert.ToDateTime(d + EntryYearStudent);
                }
                if (EntryTermStudent == 2)
                {
                    var year = EntryYearStudent + 1;
                    string d = "01/02/";
                    EntryDate = Convert.ToDateTime(d + year);
                }
                SelisihTanggal st = new SelisihTanggal();
                st.OriTanggal_1 = EntryDate.Value;
                st.OriTanggal_2 = acd_Yudisium.Yudisium_Date.Value;

                //adding data to yudisium table
                acd_Yudisium.Study_Length_Year = st.Tahun;
                acd_Yudisium.Study_Length_Month = st.Bulan;
                acd_Yudisium.Study_Length_Day = st.Hari;
                acd_Yudisium.Study_Length_Sum_Year = Convert.ToDecimal(st.JmlTahun);
                acd_Yudisium.Study_Length_Sum_Day = Convert.ToInt16(st.JmlHari);
            }

            if (ModelState.IsValid)
            {
                string[] sOriUrl = OriUrl.Split('/');

                //var idDept = db.Acd_Student.Where(s => s.Student_Id == acd_Yudisium.Student_Id).Select(s => s.Department_Id).FirstOrDefault();

                System.Web.HttpContext.Current.Session["DeptFunc"] = acd_Yudisium.Department_Functionary;
                System.Web.HttpContext.Current.Session["FacFunc"] = acd_Yudisium.Faculty_Functionary;
                db.Entry(acd_Yudisium).State = EntityState.Modified;
                db.SaveChanges();
                TempData["shortMessage"] = "Data berhasil disimpan";

                return RedirectToAction(sOriUrl[2], sOriUrl[1], new { Student_Id = acd_Yudisium.Student_Id });
            }
            //ViewBag.Graduation_Period_Id = new SelectList(db.Acd_Graduation_Period, "Graduation_Period_Id", "Period_Name", acd_Yudisium.Graduation_Period_Id);
            //ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Yudisium.Student_Id);
            //ViewBag.Graduate_Predicate_Id = new SelectList(db.Mstr_Graduate_Predicate, "Graduate_Predicate_Id", "Predicate_Name", acd_Yudisium.Graduate_Predicate_Id);
            //ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", acd_Yudisium.Term_Year_Id);
            return View(acd_Yudisium);
        }

        // GET: Yudisium/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(id);
            if (acd_Yudisium == null)
            {
                return HttpNotFound();
            }
            ViewBag.currentTermYear = acd_Yudisium.Term_Year_Id;
            ViewBag.CurrentDept = db.Acd_Student.Where(s => s.Student_Id == acd_Yudisium.Student_Id).Select(s => s.Department_Id).FirstOrDefault();

            return View(acd_Yudisium);
        }

        // POST: Yudisium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, short Department_Id)
        {
            Acd_Yudisium acd_Yudisium = db.Acd_Yudisium.Find(id);
            db.Acd_Yudisium.Remove(acd_Yudisium);
            db.SaveChanges();
            return RedirectToAction("Index", new { Term_Year_Id = acd_Yudisium.Term_Year_Id, Department_Id = Department_Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Extra classes to format the results the way the select2 dropdown wants them
        public class Select2PagedResult
        {
            public int Total { get; set; }
            public List<Select2Result> Results { get; set; }
        }

        public class Select2Result
        {
            public string id { get; set; }
            public string text { get; set; }
        }
    }
}
