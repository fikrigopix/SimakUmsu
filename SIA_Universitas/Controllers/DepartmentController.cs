using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace SIA_Universitas.Controllers
{
    public class DepartmentController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Department
        public ActionResult Index(short? Faculty_Id, short? currentFac, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            if (searchString != null)
            { 
                page = 1; 
            }
            else
            { 
                searchString = currentFilter; 
            }
            if (Faculty_Id != null)
            {
                page = 1;
            }
            else
            {
                Faculty_Id = currentFac;
            }

            ViewBag.currentFilter = searchString;
            ViewBag.currentFac = Faculty_Id;

            ViewBag.Faculty_Id = new SelectList(db.Mstr_Faculty, "Faculty_Id", "Faculty_Name", Faculty_Id);

            var mstr_Department = db.Mstr_Department.Where(d => d.Faculty_Id == Faculty_Id).Include(m => m.Mstr_Faculty);
            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_Department = mstr_Department.Where(m => m.Department_Name.Contains(searchString) || m.Department_Code.Contains(searchString));
            }
            mstr_Department = mstr_Department.OrderBy(m => m.Department_Code);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);//(page ?? 1);

            return View(mstr_Department.ToPagedList(pageNumber, pageSize));
        }

        // GET: Department/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Department mstr_Department = db.Mstr_Department.Find(id);
            if (mstr_Department == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Department);
        }

        // GET: Department/Create
        public ActionResult Create(short id)
        {
            ViewBag.Faculty = db.Mstr_Faculty.Where(f=>f.Faculty_Id == id).FirstOrDefault();
            ViewBag.Education_Prog_Type_Id = new SelectList(db.Mstr_Education_Program_Type, "Education_Prog_Type_Id", "Program_Name");

            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_Id,Department_Code,Faculty_Id,Department_Name,Department_Name_Eng,Department_Acronym,Department_Dikti_Sk_Number,Department_Dikti_Sk_Date,Order_Id,Education_Prog_Type_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Department mstr_Department)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Department.Add(mstr_Department);
                db.SaveChanges();
                return RedirectToAction("Index", new { Faculty_Id = mstr_Department.Faculty_Id});
            }

            ViewBag.Faculty = db.Mstr_Faculty.Where(f => f.Faculty_Id == mstr_Department.Faculty_Id).FirstOrDefault();
            ViewBag.Education_Prog_Type_Id = new SelectList(db.Mstr_Education_Program_Type, "Education_Prog_Type_Id", "Program_Name", mstr_Department.Education_Prog_Type_Id);

            return View(mstr_Department);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Mstr_Department mstr_Department = db.Mstr_Department.Find(id);
            if (mstr_Department == null)
            {
                return HttpNotFound();
            }
            ViewBag.Faculty_Id = new SelectList(db.Mstr_Faculty, "Faculty_Id", "Faculty_Name", mstr_Department.Faculty_Id);
            ViewBag.Education_Prog_Type_Id = new SelectList(db.Mstr_Education_Program_Type, "Education_Prog_Type_Id", "Program_Name", mstr_Department.Education_Prog_Type_Id);
            return View(mstr_Department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_Id,Department_Code,Faculty_Id,Department_Name,Department_Name_Eng,Department_Acronym,Department_Dikti_Sk_Number,Department_Dikti_Sk_Date,Order_Id,Education_Prog_Type_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Department mstr_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Department).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Department department = db.Mstr_Department.Find(mstr_Department.Department_Id);
                    if (department == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Departemen telah ada.";
                    return RedirectToAction("Edit", new { id = mstr_Department.Department_Id });
                    throw;
                }
                return RedirectToAction("Index", new { Faculty_Id = mstr_Department.Faculty_Id });
            }
            ViewBag.Faculty_Id = new SelectList(db.Mstr_Faculty, "Faculty_Id", "Faculty_Name", mstr_Department.Faculty_Id);
            return View(mstr_Department);
        }

        // GET: Department/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Department mstr_Department = db.Mstr_Department.Find(id);
        //    if (mstr_Department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Department);
        //}

        // POST: Department/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Mstr_Department mstr_Department = db.Mstr_Department.Find(id);
            db.Mstr_Department.Remove(mstr_Department);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return Redirect(UrlReferrer);
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
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

        [AllowAnonymous]
        public JsonResult IsDepartmentCodeExists(string strDepartmentCode)
        {
            strDepartmentCode = Request.QueryString["Department_Code"];
            return Json(!db.Mstr_Department.Any(c => c.Department_Code == strDepartmentCode), JsonRequestBehavior.AllowGet);
        }
    }
}
