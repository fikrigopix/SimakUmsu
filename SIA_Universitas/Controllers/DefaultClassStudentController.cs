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
    public class DefaultClassStudentController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: DefaultClassStudent
        public ActionResult Index()
        {
            var acd_Student = db.Acd_Student.Include(a => a.Acd_Gpa_TEMP).Include(a => a.Acd_Student_FREE_SKS).Include(a => a.Mstr_Blood_Type).Include(a => a.Mstr_Citizenship).Include(a => a.Mstr_Class).Include(a => a.Mstr_Class_Program).Include(a => a.Mstr_Country).Include(a => a.Mstr_Department).Include(a => a.Mstr_Entry_Period).Include(a => a.Mstr_Entry_Period_Type).Include(a => a.Mstr_Entry_Year).Include(a => a.Mstr_Gender).Include(a => a.Mstr_High_School_Major).Include(a => a.Mstr_Job_Category).Include(a => a.Mstr_Marital_Status).Include(a => a.Mstr_Register_Status).Include(a => a.Mstr_Religion).Include(a => a.Mstr_Status).Include(a => a.Mstr_Term).Include(a => a.Reg_Camaru).Include(a => a.Acd_Yudisium);
            return View(acd_Student.ToList());
        }

        // GET: DefaultClassStudent/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student acd_Student = db.Acd_Student.Find(id);
            if (acd_Student == null)
            {
                return HttpNotFound();
            }
            return View(acd_Student);
        }

        // GET: DefaultClassStudent/Create
        public ActionResult Create()
        {
            ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id");
            ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description");
            ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code");
            ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code");
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name");
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code");
            ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code");
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code");
            ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code");
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code");
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name");
            ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type");
            ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code");
            ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name");
            ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code");
            ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name");
            ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code");
            ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code");
            ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code");
            ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code");
            ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num");
            return View();
        }

        // POST: DefaultClassStudent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Student_Id,Nim,Register_Id,Register_Number,Full_Name,First_Title,Last_Title,Gender_Id,Department_Id,Class_Prog_Id,Concentration_Id,Class_Id,Birth_Place,Birth_Date,Birth_Place_Id,Birth_Country_Id,Citizenship_Id,Entry_Period_Id,Entry_Period_Type_Id,Entry_Year_Id,Entry_Term_Id,Register_Status_Id,Religion_Id,Marital_Status_Id,Job_Id,Blood_Id,High_School_Major_Id,Nisn,Nik,Status_Id,Registration_Date,Registration_Officer_Id,Source_Fund_Id,Read_Quran,Transport,Photo_Status,Student_Password,Parent_Password,Hobby_Id,Kebutuhan_Khusus,Kk_Name,Recieve_Kps,Kps_Number,Completion_Date,Out_Date,Phone_Home,Phone_Mobile,Email_Corporate,Email_General,Rfid,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Student acd_Student)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Student.Add(acd_Student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id", acd_Student.Student_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description", acd_Student.Student_Id);
            ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code", acd_Student.Blood_Id);
            ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code", acd_Student.Citizenship_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student.Class_Prog_Id);
            ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code", acd_Student.Birth_Country_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code", acd_Student.Department_Id);
            ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code", acd_Student.Entry_Period_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code", acd_Student.Entry_Period_Type_Id);
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", acd_Student.Entry_Year_Id);
            ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", acd_Student.Gender_Id);
            ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code", acd_Student.High_School_Major_Id);
            ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name", acd_Student.Job_Id);
            ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code", acd_Student.Marital_Status_Id);
            ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name", acd_Student.Register_Status_Id);
            ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", acd_Student.Religion_Id);
            ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code", acd_Student.Status_Id);
            ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", acd_Student.Entry_Term_Id);
            ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", acd_Student.Register_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num", acd_Student.Student_Id);
            return View(acd_Student);
        }

        // GET: DefaultClassStudent/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student acd_Student = db.Acd_Student.Find(id);
            if (acd_Student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id", acd_Student.Student_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description", acd_Student.Student_Id);
            ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code", acd_Student.Blood_Id);
            ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code", acd_Student.Citizenship_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student.Class_Prog_Id);
            ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code", acd_Student.Birth_Country_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code", acd_Student.Department_Id);
            ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code", acd_Student.Entry_Period_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code", acd_Student.Entry_Period_Type_Id);
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", acd_Student.Entry_Year_Id);
            ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", acd_Student.Gender_Id);
            ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code", acd_Student.High_School_Major_Id);
            ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name", acd_Student.Job_Id);
            ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code", acd_Student.Marital_Status_Id);
            ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name", acd_Student.Register_Status_Id);
            ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", acd_Student.Religion_Id);
            ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code", acd_Student.Status_Id);
            ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", acd_Student.Entry_Term_Id);
            ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", acd_Student.Register_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num", acd_Student.Student_Id);
            return View(acd_Student);
        }

        // POST: DefaultClassStudent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Student_Id,Nim,Register_Id,Register_Number,Full_Name,First_Title,Last_Title,Gender_Id,Department_Id,Class_Prog_Id,Concentration_Id,Class_Id,Birth_Place,Birth_Date,Birth_Place_Id,Birth_Country_Id,Citizenship_Id,Entry_Period_Id,Entry_Period_Type_Id,Entry_Year_Id,Entry_Term_Id,Register_Status_Id,Religion_Id,Marital_Status_Id,Job_Id,Blood_Id,High_School_Major_Id,Nisn,Nik,Status_Id,Registration_Date,Registration_Officer_Id,Source_Fund_Id,Read_Quran,Transport,Photo_Status,Student_Password,Parent_Password,Hobby_Id,Kebutuhan_Khusus,Kk_Name,Recieve_Kps,Kps_Number,Completion_Date,Out_Date,Phone_Home,Phone_Mobile,Email_Corporate,Email_General,Rfid,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Student acd_Student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id", acd_Student.Student_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description", acd_Student.Student_Id);
            ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code", acd_Student.Blood_Id);
            ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code", acd_Student.Citizenship_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student.Class_Prog_Id);
            ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code", acd_Student.Birth_Country_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Code", acd_Student.Department_Id);
            ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code", acd_Student.Entry_Period_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code", acd_Student.Entry_Period_Type_Id);
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Name", acd_Student.Entry_Year_Id);
            ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", acd_Student.Gender_Id);
            ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code", acd_Student.High_School_Major_Id);
            ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name", acd_Student.Job_Id);
            ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code", acd_Student.Marital_Status_Id);
            ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name", acd_Student.Register_Status_Id);
            ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", acd_Student.Religion_Id);
            ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code", acd_Student.Status_Id);
            ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", acd_Student.Entry_Term_Id);
            ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", acd_Student.Register_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num", acd_Student.Student_Id);
            return View(acd_Student);
        }

        // GET: DefaultClassStudent/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student acd_Student = db.Acd_Student.Find(id);
            if (acd_Student == null)
            {
                return HttpNotFound();
            }
            return View(acd_Student);
        }

        // POST: DefaultClassStudent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Acd_Student acd_Student = db.Acd_Student.Find(id);
            db.Acd_Student.Remove(acd_Student);
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
