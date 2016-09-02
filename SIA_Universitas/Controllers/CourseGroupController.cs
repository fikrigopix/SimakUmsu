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

namespace SIA_Universitas.Controllers
{
    public class CourseGroupController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CourseGroup
        public ActionResult Index()
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            return View(db.Acd_Course_Group.ToList());
        }

        // GET: CourseGroup/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Group acd_Course_Group = db.Acd_Course_Group.Find(id);
            if (acd_Course_Group == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Group);
        }

        // GET: CourseGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Group_Id,Course_Group_Code,Name_Of_Group,Description,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Group acd_Course_Group)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Course_Group.Add(acd_Course_Group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acd_Course_Group);
        }

        // GET: CourseGroup/Edit/5
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
            Acd_Course_Group acd_Course_Group = db.Acd_Course_Group.Find(id);
            if (acd_Course_Group == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Group);
        }

        // POST: CourseGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Group_Id,Course_Group_Code,Name_Of_Group,Description,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course_Group acd_Course_Group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Course_Group).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Acd_Course_Group courseGroup = db.Acd_Course_Group.Find(acd_Course_Group.Course_Group_Id);
                    if (courseGroup == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kode Kelompok MK telah ada.";
                    return RedirectToAction("Edit", acd_Course_Group);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(acd_Course_Group);
        }

        // GET: CourseGroup/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Course_Group acd_Course_Group = db.Acd_Course_Group.Find(id);
        //    if (acd_Course_Group == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Course_Group);
        //}

        // POST: CourseGroup/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Course_Group acd_Course_Group = db.Acd_Course_Group.Find(id);
            db.Acd_Course_Group.Remove(acd_Course_Group);
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
        public JsonResult IsCourseGroupCodeExists(string strCourseGroupCode)
        {
            strCourseGroupCode = Request.QueryString["Course_Group_Code"];
            return Json(!db.Acd_Course_Group.Any(c => c.Course_Group_Code == strCourseGroupCode), JsonRequestBehavior.AllowGet);
        }
    }
}
