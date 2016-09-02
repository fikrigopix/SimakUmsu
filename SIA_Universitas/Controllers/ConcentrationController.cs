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
    public class ConcentrationController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Concentration
        public ActionResult Index(short? Department_Id, short? currentDept, string currentFilter, string searchString, int? page, int? rowPerPage)
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
            if (Department_Id != null)
            {
                page = 1;
            }
            else
            {
                Department_Id = currentDept;
            }

            ViewBag.currentFilter = searchString;
            ViewBag.CurrentDept = Department_Id;

            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            var mstr_Concentration = db.Mstr_Concentration.Where(a => a.Department_Id == Department_Id).Include(m => m.Mstr_Department);
            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_Concentration = mstr_Concentration.Where(m => m.Concentration_Name.Contains(searchString) || m.Concentration_Code.Contains(searchString));
            }

            mstr_Concentration = mstr_Concentration.OrderBy(m => m.Concentration_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_Concentration.ToPagedList(pageNumber, pageSize));
        }

        // GET: Concentration/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Concentration mstr_Concentration = db.Mstr_Concentration.Find(id);
            if (mstr_Concentration == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Concentration);
        }

        // GET: Concentration/Create
        public ActionResult Create(short id)
        {
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == id).FirstOrDefault();
            return View();
        }

        // POST: Concentration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Concentration_Id,Concentration_Code,Department_Id,Concentration_Name,Concentration_Name_Eng,Concentration_Acronym,Order_Id")] Mstr_Concentration mstr_Concentration)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Concentration.Add(mstr_Concentration);
                db.SaveChanges();
                ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name", mstr_Concentration.Department_Id);
                return RedirectToAction("Index", new { Department_Id = mstr_Concentration.Department_Id });
            }

            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Concentration.Department_Id).FirstOrDefault();
            return View(mstr_Concentration);
        }

        // GET: Concentration/Edit/5
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
            Mstr_Concentration mstr_Concentration = db.Mstr_Concentration.Find(id);
            if (mstr_Concentration == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Concentration.Department_Id).FirstOrDefault();
            return View(mstr_Concentration);
        }

        // POST: Concentration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Concentration_Id,Concentration_Code,Department_Id,Concentration_Name,Concentration_Name_Eng,Concentration_Acronym,Order_Id")] Mstr_Concentration mstr_Concentration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Concentration).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Concentration concentration = db.Mstr_Concentration.Find(mstr_Concentration.Concentration_Id);
                    if (concentration == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Konsentrasi telah ada.";
                    return RedirectToAction("Edit", mstr_Concentration);
                    throw;
                }
                ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name", mstr_Concentration.Department_Id);
                return RedirectToAction("Index", new { Department_Id = mstr_Concentration.Department_Id });
            }
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == mstr_Concentration.Department_Id).FirstOrDefault();
            return View(mstr_Concentration);
        }

        // GET: Concentration/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Concentration mstr_Concentration = db.Mstr_Concentration.Find(id);
        //    if (mstr_Concentration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Concentration);
        //}

        // POST: Concentration/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Concentration mstr_Concentration = db.Mstr_Concentration.Find(id);
            db.Mstr_Concentration.Remove(mstr_Concentration);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index", new { Department_Id = mstr_Concentration.Department_Id });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", mstr_Concentration.Department_Id);
            return RedirectToAction("Index", new { Department_Id = mstr_Concentration.Department_Id });
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
        public JsonResult IsConcentrationCodeExists(string strConcentrationCode)
        {
            strConcentrationCode = Request.QueryString["Concentration_Code"];
            return Json(!db.Mstr_Concentration.Any(c => c.Concentration_Code == strConcentrationCode), JsonRequestBehavior.AllowGet);
        }
    }
}
