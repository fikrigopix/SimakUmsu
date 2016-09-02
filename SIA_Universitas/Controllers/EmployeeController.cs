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
    public class EmployeeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Employee
        public ActionResult Index(string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;

            var emp_Employee = db.Emp_Employee.Include(e => e.Emp_Active_Status).Include(e => e.Emp_Bank).Include(e => e.Emp_Employee_Status).Include(e => e.Emp_Work_Unit).Include(e => e.Mstr_Gender).Include(e => e.Mstr_Religion);
            if (!String.IsNullOrEmpty(searchString))
            {
                emp_Employee = emp_Employee.Where(s => s.Nik.Contains(searchString) || s.Full_Name.Contains(searchString));
            }

            emp_Employee = emp_Employee.OrderBy(x => x.Full_Name);

            return View(emp_Employee.ToPagedList(pageNumber, pageSize));
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Employee emp_Employee = db.Emp_Employee.Find(id);
            if (emp_Employee == null)
            {
                return HttpNotFound();
            }
            return View(emp_Employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            //ViewBag.Active_Status_Id = new SelectList(db.Emp_Active_Status, "Active_Status_Id", "Description");
            //ViewBag.Bank_Id = new SelectList(db.Emp_Bank, "Bank_Id", "Bank_Name");
            ViewBag.Employee_Status_Id = new SelectList(db.Emp_Employee_Status, "Employee_Status_Id", "Description");
            //ViewBag.Work_Unit_Id = new SelectList(db.Emp_Work_Unit, "Work_Unit_Id", "Work_Unit_Code");
            //ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type");
            //ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Employee_Id,Nik,Nip,Name,First_Title,Last_Title,Full_Name,Birth_Place,Birth_Date,Address,Gender_Id,Religion_Id,Identity_Type_Id,Identity_Number,Bank_Id,Rec_Num,Phone_Mobile,Phone_Home,Employee_Status_Id,Blood_Type_Id,Nbm,Nidn,Email_General,Email_Corporate,Role,Active_Status_Id,Npwp,Nik_Salary,Photos,Password,Nik_Finger_Print,Fingerprint_Id,Document_Serdos,Document_Serdos_Ext,Work_Unit_Id,Department_Id,Employee_Role,Forum_Role,Payroll_Role,internal_eksternal,Rfid,Card_Accepted,Created_By,Created_Date,Modified_By,Modified_Date")] Emp_Employee emp_Employee)
        {
            
            if (ModelState.IsValid)
            {
                emp_Employee.Full_Name = emp_Employee.First_Title + " " + emp_Employee.Name + " " + emp_Employee.Last_Title; 
                db.Emp_Employee.Add(emp_Employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ////ViewBag.Active_Status_Id = new SelectList(db.Emp_Active_Status, "Active_Status_Id", "Description", emp_Employee.Active_Status_Id);
            ////ViewBag.Bank_Id = new SelectList(db.Emp_Bank, "Bank_Id", "Bank_Name", emp_Employee.Bank_Id);
            ViewBag.Employee_Status_Id = new SelectList(db.Emp_Employee_Status, "Employee_Status_Id", "Description", emp_Employee.Employee_Status_Id);
            ////ViewBag.Work_Unit_Id = new SelectList(db.Emp_Work_Unit, "Work_Unit_Id", "Work_Unit_Code", emp_Employee.Work_Unit_Id);
            ////ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", emp_Employee.Gender_Id);
            ////ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", emp_Employee.Religion_Id);
            return View(emp_Employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Employee emp_Employee = db.Emp_Employee.Find(id);
            if (emp_Employee == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Active_Status_Id = new SelectList(db.Emp_Active_Status, "Active_Status_Id", "Description", emp_Employee.Active_Status_Id);
            //ViewBag.Bank_Id = new SelectList(db.Emp_Bank, "Bank_Id", "Bank_Name", emp_Employee.Bank_Id);
            ViewBag.Employee_Status_Id = new SelectList(db.Emp_Employee_Status, "Employee_Status_Id", "Description", emp_Employee.Employee_Status_Id);
            //ViewBag.Work_Unit_Id = new SelectList(db.Emp_Work_Unit, "Work_Unit_Id", "Work_Unit_Code", emp_Employee.Work_Unit_Id);
            //ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", emp_Employee.Gender_Id);
            //ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", emp_Employee.Religion_Id);
            return View(emp_Employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Employee_Id,Nik,Nip,Name,First_Title,Last_Title,Full_Name,Birth_Place,Birth_Date,Address,Gender_Id,Religion_Id,Identity_Type_Id,Identity_Number,Bank_Id,Rec_Num,Phone_Mobile,Phone_Home,Employee_Status_Id,Blood_Type_Id,Nbm,Nidn,Email_General,Email_Corporate,Role,Active_Status_Id,Npwp,Nik_Salary,Photos,Password,Nik_Finger_Print,Fingerprint_Id,Document_Serdos,Document_Serdos_Ext,Work_Unit_Id,Department_Id,Employee_Role,Forum_Role,Payroll_Role,internal_eksternal,Rfid,Card_Accepted,Created_By,Created_Date,Modified_By,Modified_Date")] Emp_Employee emp_Employee)
        {
            if (ModelState.IsValid)
            {
                emp_Employee.Full_Name = emp_Employee.First_Title + " " + emp_Employee.Name + " " + emp_Employee.Last_Title; 

                ////db.Emp_Employee.Attach(emp_Employee); // attach in the Unchanged state
                ////db.Entry(emp_Employee).Property(r => r.Nip).IsModified = true;
                ////db.Entry(emp_Employee).Property(r => r.Nik).IsModified = true;
                ////db.Entry(emp_Employee).Property(r => r.Name).IsModified = true;
                ////db.Entry(emp_Employee).Property(r => r.First_Title).IsModified = true;
                ////db.Entry(emp_Employee).Property(r => r.Last_Title).IsModified = true;
                ////db.Entry(emp_Employee).Property(r => r.Employee_Status_Id).IsModified = true;

                db.Entry(emp_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Active_Status_Id = new SelectList(db.Emp_Active_Status, "Active_Status_Id", "Description", emp_Employee.Active_Status_Id);
            //ViewBag.Bank_Id = new SelectList(db.Emp_Bank, "Bank_Id", "Bank_Name", emp_Employee.Bank_Id);
            ViewBag.Employee_Status_Id = new SelectList(db.Emp_Employee_Status, "Employee_Status_Id", "Description", emp_Employee.Employee_Status_Id);
            //ViewBag.Work_Unit_Id = new SelectList(db.Emp_Work_Unit, "Work_Unit_Id", "Work_Unit_Code", emp_Employee.Work_Unit_Id);
            //ViewBag.Gender_Id = new SelectList(db.Mstr_Gender, "Gender_Id", "Gender_Type", emp_Employee.Gender_Id);
            //ViewBag.Religion_Id = new SelectList(db.Mstr_Religion, "Religion_Id", "Religion_Code", emp_Employee.Religion_Id);
            return View(emp_Employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Employee emp_Employee = db.Emp_Employee.Find(id);
            if (emp_Employee == null)
            {
                return HttpNotFound();
            }
            return View(emp_Employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emp_Employee emp_Employee = db.Emp_Employee.Find(id);
            db.Emp_Employee.Remove(emp_Employee);
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
