using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SIA_Universitas.Models;
using System.Data.Entity.Infrastructure;

namespace SIA_Universitas.Controllers
{
    public class DepartmentClassProgController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: DepartmentClassProg
        public ActionResult Index(string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; } else { searchString = currentFilter; }
            ViewBag.CurrentFilter = searchString;
            
            var mstr_Department = db.Mstr_Department.Include(a=>a.Mstr_Faculty);
            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_Department = mstr_Department.Where(d => d.Department_Name.Contains(searchString));
            }
            mstr_Department = mstr_Department.OrderBy(d => d.Department_Code).ThenBy(d => d.Department_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_Department.ToPagedList(pageNumber, pageSize));
        }

        // GET: DepartmentClassProg/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Department_Class_Program mstr_Department_Class_Program = db.Mstr_Department_Class_Program.Find(id);
            if (mstr_Department_Class_Program == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Department_Class_Program);
        }

        // GET: DepartmentClassProg/Create
        public ActionResult Create(short? id, string UrlReferrer)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == id).Single();
            ViewBag.DeptClasProg = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == id);
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            var exceptionList = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == id).Select(dcp => dcp.Class_Prog_Id).ToList();
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => !exceptionList.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name");
            return View();
        }

        // POST: DepartmentClassProg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_Class_Prog_Id,Department_Id,Class_Prog_Id,Class_Progs,Department_Class_Prog_Name,Acronym,Acreditation_Status,Acreditation_Number,Acreditation_date,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Department_Class_Program mstr_Department_Class_Program, string UrlReferrer)
        {

            if (ModelState.IsValid)
            {
                for (short i = 0; i < mstr_Department_Class_Program.Class_Progs.Length; i++)
                {
                    mstr_Department_Class_Program.Class_Prog_Id = mstr_Department_Class_Program.Class_Progs[i];
                    db.Mstr_Department_Class_Program.Add(mstr_Department_Class_Program);
                    db.SaveChanges();
                }
                return RedirectToAction("Create", new { id = mstr_Department_Class_Program.Department_Id, UrlReferrer = UrlReferrer });
            }

            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Department_Class_Program.Department_Id).Single();
            ViewBag.DeptClasProg = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == mstr_Department_Class_Program.Department_Id);
            ViewBag.UrlReferrer = UrlReferrer;

            var exceptionList = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == mstr_Department_Class_Program.Department_Id).Select(dcp => dcp.Class_Prog_Id).ToList();
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => !exceptionList.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name");
            return View(mstr_Department_Class_Program);
        }

        // GET: DepartmentClassProg/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Department_Class_Program mstr_Department_Class_Program = db.Mstr_Department_Class_Program.Find(id);
            if (mstr_Department_Class_Program == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", mstr_Department_Class_Program.Class_Prog_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", mstr_Department_Class_Program.Department_Id);
            return View(mstr_Department_Class_Program);
        }

        // POST: DepartmentClassProg/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_Class_Prog_Id,Department_Id,Class_Prog_Id,Department_Class_Prog_Name,Acronym,Acreditation_Status,Acreditation_Number,Acreditation_date,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Department_Class_Program mstr_Department_Class_Program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Department_Class_Program).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", mstr_Department_Class_Program.Class_Prog_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", mstr_Department_Class_Program.Department_Id);
            return View(mstr_Department_Class_Program);
        }

        // GET: DepartmentClassProg/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Department_Class_Program mstr_Department_Class_Program = db.Mstr_Department_Class_Program.Find(id);
        //    if (mstr_Department_Class_Program == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Department_Class_Program);
        //}

        // POST: DepartmentClassProg/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id, string UrlReferrer)
        {
            Mstr_Department_Class_Program mstr_Department_Class_Program = db.Mstr_Department_Class_Program.Find(id);
            short x = db.Mstr_Department.Where(d => d.Department_Id == mstr_Department_Class_Program.Department_Id).Select(d => d.Department_Id).Single();
            db.Mstr_Department_Class_Program.Remove(mstr_Department_Class_Program);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Create", new { id = x, UrlReferrer = UrlReferrer });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Create", new { id = x, UrlReferrer = UrlReferrer });
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
