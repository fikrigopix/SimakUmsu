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
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SIA_Universitas.Controllers
{
    public class StudentPasswordController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: StudentPassword
        public ActionResult Index(string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //alert
            if (TempData["massage_success"] != null)
            {
                ViewBag.massage_success = TempData["massage_success"].ToString();
            }
            if (TempData["massage_warning"] != null)
            {
                ViewBag.massage_warning = TempData["massage_warning"].ToString();
            }
            if (TempData["massage_danger"] != null)
            {
                ViewBag.massage_danger = TempData["massage_danger"].ToString();
            }

            //for url
            ViewBag.searchString = searchString;

            if (String.IsNullOrEmpty(searchString))
            {
                List<Acd_Student> acd_StudentEmpty = new List<Acd_Student>();
                return View(acd_StudentEmpty.ToPagedList(pageNumber, pageSize));
            }

            var acd_Student = db.Acd_Student.Where(s => s.Nim == searchString);

            if (acd_Student.Count() == 0)
            {
                ViewBag.massage_warning = "Data tidak ditemukan";
            }
            acd_Student = acd_Student.OrderBy(x => x.Full_Name);

            return View(acd_Student.ToPagedList(pageNumber, pageSize));
        }

        //// GET: StudentPassword/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Student acd_Student = db.Acd_Student.Find(id);
        //    if (acd_Student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Student);
        //}

        //// GET: StudentPassword/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id");
        //    ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description");
        //    ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code");
        //    ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code");
        //    ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name");
        //    ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code");
        //    ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code");
        //    ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code");
        //    ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code");
        //    ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code");
        //    ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year, "Entry_Year_Id", "Entry_Year_Name");
        //    ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type");
        //    ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code");
        //    ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name");
        //    ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code");
        //    ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name");
        //    ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code");
        //    ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code");
        //    ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code");
        //    ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code");
        //    ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num");
        //    return View();
        //}

        //// POST: StudentPassword/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Student_Id,Nim,Register_Id,Register_Number,Full_Name,First_Title,Last_Title,Gender_Id,Department_Id,Class_Prog_Id,Concentration_Id,Class_Id,Birth_Place,Birth_Date,Birth_Place_Id,Birth_Country_Id,Citizenship_Id,Entry_Period_Id,Entry_Period_Type_Id,Entry_Year_Id,Entry_Term_Id,Register_Status_Id,Religion_Id,Marital_Status_Id,Job_Id,Blood_Id,High_School_Major_Id,Nisn,Nik,Status_Id,Registration_Date,Registration_Officer_Id,Source_Fund_Id,Read_Quran,Transport,Photo_Status,Student_Password,Parent_Password,Hobby_Id,Kebutuhan_Khusus,Kk_Name,Recieve_Kps,Kps_Number,Completion_Date,Out_Date,Phone_Home,Phone_Mobile,Email_Corporate,Email_General,Rfid,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Student acd_Student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Acd_Student.Add(acd_Student);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Student_Id = new SelectList(db.Acd_Gpa_TEMP, "Student_Id", "Student_Id", acd_Student.Student_Id);
        //    ViewBag.Student_Id = new SelectList(db.Acd_Student_FREE_SKS, "Student_Id", "Description", acd_Student.Student_Id);
        //    ViewBag.Blood_Id = new SelectList(db.Mstr_Blood_Type, "Blood_Type_Id", "Blood_Code", acd_Student.Blood_Id);
        //    ViewBag.Citizenship_Id = new SelectList(db.Mstr_Citizenship, "Citizenship_Id", "Citizenship_Code", acd_Student.Citizenship_Id);
        //    ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student.Class_Id);
        //    ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student.Class_Prog_Id);
        //    ViewBag.Birth_Country_Id = new SelectList(db.Mstr_Country, "Country_Id", "Country_Code", acd_Student.Birth_Country_Id);
        //    ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", acd_Student.Department_Id);
        //    ViewBag.Entry_Period_Id = new SelectList(db.Mstr_Entry_Period, "Entry_Period_Id", "Entry_Period_Code", acd_Student.Entry_Period_Id);
        //    ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Code", acd_Student.Entry_Period_Type_Id);
        //    ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year, "Entry_Year_Id", "Entry_Year_Name", acd_Student.Entry_Year_Id);
        //    ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", acd_Student.Gender_Id);
        //    ViewBag.High_School_Major_Id = new SelectList(db.Mstr_High_School_Major, "High_School_Major_Id", "High_School_Major_Code", acd_Student.High_School_Major_Id);
        //    ViewBag.Job_Id = new SelectList(db.Mstr_Job_Category, "Job_Category_Id", "Job_Category_Name", acd_Student.Job_Id);
        //    ViewBag.Marital_Status_Id = new SelectList(db.Mstr_Marital_Status, "Marital_Status_Id", "Marital_Status_Code", acd_Student.Marital_Status_Id);
        //    ViewBag.Register_Status_Id = new SelectList(db.Mstr_Register_Status, "Register_Status_Id", "Register_Status_Name", acd_Student.Register_Status_Id);
        //    ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", acd_Student.Religion_Id);
        //    ViewBag.Status_Id = new SelectList(db.Mstr_Status, "Status_Id", "Status_Code", acd_Student.Status_Id);
        //    ViewBag.Entry_Term_Id = new SelectList(db.Mstr_Term, "Term_Id", "Term_Code", acd_Student.Entry_Term_Id);
        //    ViewBag.Register_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", acd_Student.Register_Id);
        //    ViewBag.Student_Id = new SelectList(db.Acd_Yudisium, "Student_Id", "Sk_Num", acd_Student.Student_Id);
        //    return View(acd_Student);
        //}

        // GET: StudentPassword/Edit/5
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
            return View(acd_Student);
        }

        // POST: StudentPassword/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Acd_Student acd_Student, string parent_pass, string student_pass)
        {

            if (ModelState.IsValid)
            {
                string err = string.Empty;

                //validasi
                string err1 = cekContentPassword(acd_Student.Student_Password);
                if (err1 != string.Empty)
                {
                    ViewBag.massage_warning_student = err1;
                    return View(acd_Student);
                }

                string err2 = cekContentPassword(acd_Student.Parent_Password);
                if (err2 != string.Empty)
                {
                    ViewBag.massage_warning_parent = err2;
                    return View(acd_Student);
                }

                if (string.IsNullOrEmpty(acd_Student.Student_Password))
                {
                    if (string.IsNullOrEmpty(student_pass))
                    {
                        acd_Student.Student_Password = null;
                    }
                    else
                    {
                        acd_Student.Student_Password = student_pass;
                    }
                }
                else
                {
                    acd_Student.Student_Password = MD5Hash(acd_Student.Student_Password);
                }

                if (string.IsNullOrEmpty(acd_Student.Parent_Password))
                {
                    if (string.IsNullOrEmpty(parent_pass))
                    {
                        acd_Student.Parent_Password = null;
                    }
                    else
                    {
                        acd_Student.Parent_Password = parent_pass;
                    }
                }
                else
                {
                    acd_Student.Parent_Password = MD5Hash(acd_Student.Parent_Password);
                }

                try
                {
                    db.Acd_Student.Attach(acd_Student); // attach in the Unchanged state

                    db.Entry(acd_Student).Property(r => r.Student_Password).IsModified = true;
                    db.Entry(acd_Student).Property(r => r.Parent_Password).IsModified = true;

                    //db.Entry(acd_Student).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    err = ex.Message.ToString();
                }
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}

                if (err == string.Empty)
                {
                    TempData["massage_success"] = "Data berhasil diubah";
                }
                else
                {
                    TempData["massage_danger"] = err;
                }
                
                return RedirectToAction("Index", new { searchString = acd_Student.Nim });
            }

            return View(acd_Student);
        }

        private string cekContentPassword(string password)
        {
            string err = string.Empty;
            if (password != null)
            {
                if (password.Length < 5 || password.Length > 15)
                {
                    err = "Panjang password minimal 5 karakter, maksimal 15 karakter";
                    return err;
                }

                for (int i = 0; i < password.Length; i++) //ulangi sepanjang password
                {
                    if (i == 0)
                    {
                        if (password[i] == '0')
                        {
                            err = "Awal password tidak boleh mengandung angka 0";
                            break;
                        }
                    }

                    if (password[i] == '$')
                    {
                        err = "Password tidak boleh mengandung karakter $";
                        break;
                    }

                    if (password[i].ToString() == "'")
                    {
                        err = "Password tidak boleh mengandung petik satu ";
                        break;
                    }

                    if (password[i] == '"')
                    {
                        err = "Password tidak boleh mengandung petik dua ";
                        break;
                    }
                }
            }

            return err;
        }

        private static string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        // GET: StudentPassword/Delete/5
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

        // POST: StudentPassword/Delete/5
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
