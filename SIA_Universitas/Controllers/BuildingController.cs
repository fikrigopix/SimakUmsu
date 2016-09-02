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
using System.Data.Entity.Infrastructure;

namespace SIA_Universitas.Controllers
{
    public class BuildingController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Building
        public ActionResult Index(string currentFilter, string SearchString, int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            //if (TempData["shortMessage"] != null)
            //{
            //    ViewBag.message = TempData["shortMessage"].ToString();
            //}
            if (SearchString != null) { page = 1; }
            else { SearchString = currentFilter; }

            ViewBag.currentFilter = SearchString;

            var mstr_Building = db.Mstr_Building.Where(b => b.Building_Id != null);

            if (!String.IsNullOrEmpty(SearchString))
            {
                mstr_Building = mstr_Building.Where(b => b.Building_Name.Contains(SearchString));
            }
            mstr_Building = mstr_Building.OrderBy(b => b.Building_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);
            return View(mstr_Building.ToPagedList(pageNumber,pageSize));
        }

        // GET: Building/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Building mstr_Building = db.Mstr_Building.Find(id);
            if (mstr_Building == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Building);
        }

        // GET: Building/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Building/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Building_Id,Building_Code,Building_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Building mstr_Building)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Building.Add(mstr_Building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mstr_Building);
        }

        // GET: Building/Edit/5
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
            Mstr_Building mstr_Building = db.Mstr_Building.Find(id);
            if (mstr_Building == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Building);
        }

        // POST: Building/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Building_Id,Building_Code,Building_Name,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Building mstr_Building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Building).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Building building = db.Mstr_Building.Find(mstr_Building.Building_Id);
                    if (building == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Gedung telah ada.";
                    return RedirectToAction("Edit", mstr_Building);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(mstr_Building);
        }

        // GET: Building/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Building mstr_Building = db.Mstr_Building.Find(id);
        //    if (mstr_Building == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Building);
        //}

        // POST: Building/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_Building mstr_Building = db.Mstr_Building.Find(id);
            db.Mstr_Building.Remove(mstr_Building);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index");
                throw;
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
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

        [AllowAnonymous]
        public JsonResult IsBuildingCodeExists(string strBuildingCode)
        {
            strBuildingCode = Request.QueryString["Building_Code"];
            return Json(!db.Mstr_Building.Any(b => b.Building_Code == strBuildingCode), JsonRequestBehavior.AllowGet);
        }
    }
}
