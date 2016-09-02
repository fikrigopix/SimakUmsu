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
    public class CourseIdenticController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CourseIdentic
        public ActionResult Index(short? Department_Id)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            var deptId = Department_Id ?? 0;
            ViewBag.deptId = Department_Id;
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            var acd_Course_Identic = db.Acd_Course_Identic.Where(ci => ci.Department_Id == Department_Id);
            return View(acd_Course_Identic.ToList());
        }

        // GET: CourseIdentic/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Identic acd_Course_Identic = db.Acd_Course_Identic.Find(id);
            if (acd_Course_Identic == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Identic);
        }

        // GET: CourseIdentic/Create
        public ActionResult Create(short? Department_Id)
        {
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (Department_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<int> exceptionList = db.Acd_Course_Identic_Detail.Select(cid => cid.Course_Id).ToList();

            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => !exceptionList.Contains(c.Course_Id)), "Course_Id", "NameCode");
            ViewBag.Department_Id = Department_Id;

            return View();
        }

        // POST: CourseIdentic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Identic_Id,Identic_Name,Department_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Identic acd_Course_Identic, int[] Courses, string UrlReferrer)
        {
            Acd_Course_Identic_Detail acd_Course_Identic_Detail = new Acd_Course_Identic_Detail();
            if (ModelState.IsValid)
            {
                db.Acd_Course_Identic.Add(acd_Course_Identic);
                db.SaveChanges();
                if (Courses != null)
                {
                    short Course_Identic_Id = db.Acd_Course_Identic.Where(ci => ci.Identic_Name == acd_Course_Identic.Identic_Name).Select(ci => ci.Course_Identic_Id).First();
                    for (int i = 0; i < Courses.Length; i++)
                    {
                        acd_Course_Identic_Detail.Course_Identic_Id = Course_Identic_Id;
                        acd_Course_Identic_Detail.Course_Id = Courses[i];
                        db.Acd_Course_Identic_Detail.Add(acd_Course_Identic_Detail);
                        db.SaveChanges();
                    }
                }

                return Redirect(UrlReferrer);
            }

            List<int> exceptionList = db.Acd_Course_Identic_Detail.Select(cid => cid.Course_Id).ToList();
            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => !exceptionList.Contains(c.Course_Id)), "Course_Id", "NameCode");
            ViewBag.Department_Id = acd_Course_Identic.Department_Id;

            return View(acd_Course_Identic);
        }

        // GET: CourseIdentic/Edit/5
        public ActionResult Edit(short? id, string UrlReferrer)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Identic acd_Course_Identic = db.Acd_Course_Identic.Find(id);
            if (acd_Course_Identic == null)
            {
                return HttpNotFound();
            }

            ViewBag.acd_Course_Identic_Detail = db.Acd_Course_Identic_Detail.Where(cid => cid.Course_Identic_Id == acd_Course_Identic.Course_Identic_Id);

            List<int> exceptionList = db.Acd_Course_Identic_Detail.Select(cid => cid.Course_Id).ToList();
            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => !exceptionList.Contains(c.Course_Id)), "Course_Id", "NameCode");
            //ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Course_Identic.Department_Id).First();
            return View(acd_Course_Identic);
        }

        // POST: CourseIdentic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Identic_Id,Identic_Name,Department_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Identic acd_Course_Identic, int[] Courses, string UrlReferrer)
        {
            Acd_Course_Identic_Detail acd_Course_Identic_Detail = new Acd_Course_Identic_Detail();
            if (ModelState.IsValid)
            {
                db.Entry(acd_Course_Identic).State = EntityState.Modified;
                db.SaveChanges();
                if (Courses != null)
                {
                    short Course_Identic_Id = db.Acd_Course_Identic.Where(ci => ci.Identic_Name == acd_Course_Identic.Identic_Name).Select(ci => ci.Course_Identic_Id).First();
                    for (int i = 0; i < Courses.Length; i++)
                    {
                        acd_Course_Identic_Detail.Course_Identic_Id = Course_Identic_Id;
                        acd_Course_Identic_Detail.Course_Id = Courses[i];
                        db.Acd_Course_Identic_Detail.Add(acd_Course_Identic_Detail);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Edit", new { id = acd_Course_Identic.Course_Identic_Id, UrlReferrer = UrlReferrer });
            }

            ViewBag.acd_Course_Identic_Detail = db.Acd_Course_Identic_Detail.Where(cid => cid.Course_Identic_Id == acd_Course_Identic.Course_Identic_Id);

            List<int> exceptionList = db.Acd_Course_Identic_Detail.Select(cid => cid.Course_Id).ToList();
            ViewBag.Course_Id = new SelectList(db.Acd_Course.Where(c => !exceptionList.Contains(c.Course_Id)), "Course_Id", "NameCode");

            return View(acd_Course_Identic);
        }

        //// GET: CourseIdentic/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Course_Identic acd_Course_Identic = db.Acd_Course_Identic.Find(id);
        //    if (acd_Course_Identic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Course_Identic);
        //}

        //// POST: CourseIdentic/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Course_Identic acd_Course_Identic = db.Acd_Course_Identic.Find(id);
            db.Acd_Course_Identic.Remove(acd_Course_Identic);
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

        // POST: CourseIdentic/DeleteCourse/5
        //[HttpPost, ActionName("DeleteCourse")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteCourseConfirmed(short id, string UrlReferrer)
        {
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Course_Identic_Detail acd_Course_Identic_Detail = db.Acd_Course_Identic_Detail.Find(id);
            db.Acd_Course_Identic_Detail.Remove(acd_Course_Identic_Detail);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Edit", new { id = acd_Course_Identic_Detail.Course_Identic_Id, UrlReferrer = UrlReferrer });
            }
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Edit", new { id = acd_Course_Identic_Detail.Course_Identic_Id, UrlReferrer = UrlReferrer });
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
        public JsonResult IsDataExists(string strIdenticName)
        {
            strIdenticName = Request.QueryString["Identic_Name"];
            return Json(!db.Acd_Course_Identic.Any(ci => ci.Identic_Name == strIdenticName), JsonRequestBehavior.AllowGet);
        }
    }
}
