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
    public class RoomController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Room
        public ActionResult Index(short? Building_Id, short? currentBuilding, string currentFilter, string searchString, int? page, int? rowPerPage)
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
            if (Building_Id != null)
            {
                page = 1;
            }
            else
            {
                Building_Id = currentBuilding;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.currentBuilding = Building_Id;
            ViewBag.Building_Id = new SelectList(db.Mstr_Building.OrderBy(m => m.Building_Code), "Building_Id", "Building_Name", Building_Id);

            var mstr_Room = db.Mstr_Room.Where(m => m.Building_Id == Building_Id).Include(m => m.Mstr_Building);
            if (!String.IsNullOrEmpty(searchString))
            {
                mstr_Room = mstr_Room.Where(s => s.Room_Name.Contains(searchString) || s.Description.Contains(searchString));
            }

            mstr_Room = mstr_Room.OrderBy(s => s.Room_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(mstr_Room.ToPagedList(pageNumber, pageSize));
        }

        // GET: Room/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mstr_Room mstr_Room = db.Mstr_Room.Find(id);
            if (mstr_Room == null)
            {
                return HttpNotFound();
            }
            return View(mstr_Room);
        }

        // GET: Room/Create
        public ActionResult Create(short? id)
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
            ViewBag.Building_Id = mstr_Building.Building_Id;
            ViewBag.Building_Name = mstr_Building.Building_Name;
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Room_Id,Room_Code,Building_Id,Room_Name,Description,Capacity,Acronym,Is_Active,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Room mstr_Room)
        {
            if (ModelState.IsValid)
            {
                db.Mstr_Room.Add(mstr_Room);
                db.SaveChanges();
                return RedirectToAction("Index", new { Building_Id = mstr_Room.Building_Id });
            }

            //ViewBag.Building_Id = new SelectList(db.Mstr_Building, "Building_Id", "Building_Code", mstr_Room.Building_Id);
            return View(mstr_Room);
        }

        // GET: Room/Edit/5
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
            Mstr_Room mstr_Room = db.Mstr_Room.Find(id);
            if (mstr_Room == null)
            {
                return HttpNotFound();
            }
            ViewBag.Building_Id = new SelectList(db.Mstr_Building.OrderBy(m => m.Building_Code), "Building_Id", "Building_Name", mstr_Room.Building_Id);
            return View(mstr_Room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Room_Id,Room_Code,Building_Id,Room_Name,Description,Capacity,Acronym,Is_Active,Created_By,Created_Date,Modified_By,Modified_Date")] Mstr_Room mstr_Room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mstr_Room).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Mstr_Room room = db.Mstr_Room.Find(mstr_Room.Room_Id);
                    if (room == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Ruang telah ada.";
                    return RedirectToAction("Edit", mstr_Room);
                    throw;
                }
                return RedirectToAction("Index", new { Building_Id = mstr_Room.Building_Id });
            }
            ViewBag.Building_Id = new SelectList(db.Mstr_Building.OrderBy(m => m.Building_Code), "Building_Id", "Building_Name", mstr_Room.Building_Id);
            return View(mstr_Room);
        }

        // GET: Room/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Mstr_Room mstr_Room = db.Mstr_Room.Find(id);
        //    if (mstr_Room == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mstr_Room);
        //}

        // POST: Room/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id, short BuildingId)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Mstr_Room mstr_Room = db.Mstr_Room.Find(id);
            db.Mstr_Room.Remove(mstr_Room);
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
        public JsonResult IsRoomCodeExists(string strRoomCode)
        {
            strRoomCode = Request.QueryString["Room_Code"];
            return Json(!db.Mstr_Room.Any(c => c.Room_Code == strRoomCode), JsonRequestBehavior.AllowGet);
        }
    }
}
