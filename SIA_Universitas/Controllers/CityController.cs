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
    public class CityController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: City
        public ActionResult Index(short? Province_Id, short? currentProv, string currentFilter, string searchString, int? page, int? rowPerPage)
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
            if (Province_Id != null)
            {
                page = 1;
            }
            else
            {
                Province_Id = currentProv;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.currentProv = Province_Id;
            ViewBag.Province_Id = new SelectList(db.Mstr_Province, "Province_Id", "Province_Name", Province_Id);

            var mstr_City = db.Mstr_City.Where(c => c.Province_Id == Province_Id).Include(m => m.Mstr_Province);
            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_City = mstr_City.Where(s => s.City_Name.Contains(searchString) || s.City_Code.Contains(searchString));
            }

            mstr_City = mstr_City.OrderBy(s => s.City_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_City.ToPagedList(pageNumber, pageSize));
        }

        // GET: City/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_City mstr_City = db.Mstr_City.Find(id);
            if (mstr_City == null)
            {
                return HttpNotFound();
            }
            return View(mstr_City);
        }

        // GET: City/Create
        public ActionResult Create(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Province mstr_Province = db.Mstr_Province.Find(id);
            if (mstr_Province == null)
            {
                return HttpNotFound();
            }
            ViewBag.Province_Id = mstr_Province.Province_Id;
            ViewBag.Province_Name = mstr_Province.Province_Name;
            return View();
        }

        // POST: City/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "City_Id,Province_Id,City_Code,City_Name,Order_Id")] Mstr_City mstr_City)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_City.Add(mstr_City);
                db.SaveChanges();
                return RedirectToAction("Index", new { Province_Id = mstr_City.Province_Id});
            }
            //Mstr_Province mstr_Province = db.Mstr_Province.Find(mstr_City.Province_Id);
            //ViewBag.Province_Id = mstr_Province.Province_Id;
            //ViewBag.Province_Name = mstr_Province.Province_Name;
            return View(mstr_City);
        }

        // GET: City/Edit/5
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
            Mstr_City mstr_City = db.Mstr_City.Find(id);
            if (mstr_City == null)
            {
                return HttpNotFound();
            }
            ViewBag.Province_Id = new SelectList(db.Mstr_Province, "Province_Id", "Province_Name", mstr_City.Province_Id);
            return View(mstr_City);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "City_Id,Province_Id,City_Code,City_Name,Order_Id")] Mstr_City mstr_City)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_City).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_City city = db.Mstr_City.Find(mstr_City.City_Id);
                    if (city == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Kota telah ada.";
                    return RedirectToAction("Edit", mstr_City);
                    throw;
                }
                return RedirectToAction("Index", new { Province_Id = mstr_City.Province_Id});
            }
            ViewBag.Province_Id = new SelectList(db.Mstr_Province, "Province_Id", "Province_Name", mstr_City.Province_Id);
            return View(mstr_City);
        }

        // GET: City/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_City mstr_City = db.Mstr_City.Find(id);
        //    if (mstr_City == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_City);
        //}

        // POST: City/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Mstr_City mstr_City = db.Mstr_City.Find(id);
            Mstr_Province mstr_Province = db.Mstr_Province.Find(mstr_City.Province_Id);
            db.Mstr_City.Remove(mstr_City);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index", new { Province_Id = mstr_Province.Province_Id });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Index", new { Province_Id = mstr_Province.Province_Id });
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
        public JsonResult IsCityCodeExists(string strCityCode)
        {
            strCityCode = Request.QueryString["City_Code"];
            return Json(!db.Mstr_City.Any(c => c.City_Code == strCityCode), JsonRequestBehavior.AllowGet);
        }
    }
}
