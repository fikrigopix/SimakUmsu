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
    public class Course_TypeController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Course_Type
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
            return View(db.Acd_Course_Type.ToList());
        }

        // GET: Course_Type/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course_Type acd_Course_Type = db.Acd_Course_Type.Find(id);
            if (acd_Course_Type == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Type);
        }

        // GET: Course_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Type_Id,Course_Type_Code,Course_Type_Name,Id_Character,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Type acd_Course_Type)
        {
            acd_Course_Type.Created_Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Acd_Course_Type.Add(acd_Course_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acd_Course_Type);
        }

        // GET: Course_Type/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["errorCourse_Type_Code"] != null)
            {
                ViewBag.errorCourse_Type_Code = TempData["errorCourse_Type_Code"].ToString();
            }
            //if (TempData["errorMessage"] != null)
            //{
            //    ViewBag.errorMessage = TempData["errorMessage"].ToString();
            //}
            Acd_Course_Type acd_Course_Type = db.Acd_Course_Type.Find(id);
            if (acd_Course_Type == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course_Type);
        }

        // POST: Course_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Type_Id,Course_Type_Code,Course_Type_Name,Id_Character,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Course_Type acd_Course_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Course_Type).State = EntityState.Modified;

                ////validasi unik
                //if (acd_Course_Type.Course_Type_Code != null)
                //{
                //    //jika Course_Type_Code sudah ada di data sebelumnya
                //    if (db.Acd_Course_Type.Where(x => x.Course_Type_Code == acd_Course_Type.Course_Type_Code && x.Course_Type_Id != acd_Course_Type.Course_Type_Id).Count() > 0)
                //    {
                //        TempData["errorCourse_Type_Code"] = "Kode Tipe Matakuliah telah ada.";
                //        return RedirectToAction("Edit", acd_Course_Type);
                //    } 
                //}

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    
                    Acd_Course_Type Course_Type_Code = db.Acd_Course_Type.Find(acd_Course_Type.Course_Type_Id);
                    if (Course_Type_Code == null)
                    {
                        return HttpNotFound();
                    }

                    TempData["errorCourse_Type_Code"] = "Kode Tipe Matakuliah telah ada.";
                    return RedirectToAction("Edit", acd_Course_Type);

                    //TempData["errorMessage"] = ex.Message.ToString();
                    //return RedirectToAction("Edit", acd_Course_Type);
                }
                
                return RedirectToAction("Index");
            }
            return View(acd_Course_Type);
        }

        // GET: Course_Type/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Course_Type acd_Course_Type = db.Acd_Course_Type.Find(id);
        //    if (acd_Course_Type == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Course_Type);
        //}

        // POST: Course_Type/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Acd_Course_Type acd_Course_Type = db.Acd_Course_Type.Find(id);
            db.Acd_Course_Type.Remove(acd_Course_Type);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index");
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
        public JsonResult IsCourseTypeCodeExists(string strCourseTypeCode)
        {
            strCourseTypeCode = Request.QueryString ["Course_Type_Code"];
            Int16 intCourseTypeCode = Convert.ToInt16(strCourseTypeCode);
            return Json(!db.Acd_Course_Type.Any(c => c.Course_Type_Code == intCourseTypeCode), JsonRequestBehavior.AllowGet);
        }
    }
}
