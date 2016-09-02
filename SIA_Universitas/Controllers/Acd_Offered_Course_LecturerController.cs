using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;

namespace SIA_Universitas.Controllers
{
    public class Acd_Offered_Course_LecturerController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Acd_Offered_Course_Lecturer
        public ActionResult Index()
        {
            var acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Include(a => a.Acd_Offered_Course).Include(a => a.Emp_Employee);
            return View(acd_Offered_Course_Lecturer.ToList());
        }

        // GET: Acd_Offered_Course_Lecturer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Find(id);
            if (acd_Offered_Course_Lecturer == null)
            {
                return HttpNotFound();
            }
            return View(acd_Offered_Course_Lecturer);
        }

        // GET: Acd_Offered_Course_Lecturer/Create
        public ActionResult Create()
        {
            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By");
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik");
            return View();
        }

        // POST: Acd_Offered_Course_Lecturer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Acd_Offered_Course_Lecturer1,Offered_Course_id,Employee_Id,Sks_Weight,Order_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Offered_Course_Lecturer.Add(acd_Offered_Course_Lecturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Lecturer.Offered_Course_id);
            ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_Offered_Course_Lecturer.Employee_Id);
            return View(acd_Offered_Course_Lecturer);
        }

        // GET: Acd_Offered_Course_Lecturer/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Find(id);
        //    if (acd_Offered_Course_Lecturer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Offered_Course_id = new SelectList(db.Acd_Offered_Course, "Offered_Course_id", "Created_By", acd_Offered_Course_Lecturer.Offered_Course_id);
        //    ViewBag.Employee_Id = new SelectList(db.Emp_Employee, "Employee_Id", "Nik", acd_Offered_Course_Lecturer.Employee_Id);
        //    return View(acd_Offered_Course_Lecturer);
        //}

        // POST: Acd_Offered_Course_Lecturer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Acd_Offered_Course_Lecturer1,Offered_Course_id,Employee_Id,Sks_Weight,Order_Id,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer, string UrlReferrer, string p)
        {
            Acd_Offered_Course_Lecturer cause = db.Acd_Offered_Course_Lecturer.Find(acd_Offered_Course_Lecturer.Acd_Offered_Course_Lecturer1);
            short max = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == cause.Offered_Course_id).Max(ocl => ocl.Order_Id);
            Acd_Offered_Course_Lecturer victim = new Acd_Offered_Course_Lecturer();
            if (p == "up")
            {
                if (cause.Order_Id != 1)
                {
                    victim = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == cause.Offered_Course_id && ocl.Order_Id < cause.Order_Id).OrderByDescending(ocl => ocl.Order_Id).First();
                    cause.Order_Id = Convert.ToInt16(victim.Order_Id);
                    victim.Order_Id = Convert.ToInt16(victim.Order_Id + 1);
                }
                //else
                //{
                //    return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
                //}
            }
            if (p == "down")
            {
                if (cause.Order_Id != max)
                {
                    victim = db.Acd_Offered_Course_Lecturer.Where(ocl => ocl.Offered_Course_id == cause.Offered_Course_id && ocl.Order_Id > cause.Order_Id).OrderBy(ocl => ocl.Order_Id).First();
                    cause.Order_Id = Convert.ToInt16(victim.Order_Id);
                    victim.Order_Id = Convert.ToInt16(victim.Order_Id - 1);
                }
                //else
                //{
                //    return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
                //}
            }

            //For Deceive Validation Model
            List<int> list = new List<int>();
            list.Add(1);
            victim.Employees = list.ToArray();
            cause.Employees = list.ToArray();

            //Save Edit
            db.Entry(victim).State = EntityState.Modified;
            db.Entry(cause).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("EditDosen", "OfferedCourse", new { id = cause.Offered_Course_id, UrlReferrer = UrlReferrer });
            //return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        // GET: Acd_Offered_Course_Lecturer/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Find(id);
        //    if (acd_Offered_Course_Lecturer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Offered_Course_Lecturer);
        //}

        // POST: Acd_Offered_Course_Lecturer/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string UrlReferrer)
        {
            Acd_Offered_Course_Lecturer acd_Offered_Course_Lecturer = db.Acd_Offered_Course_Lecturer.Find(id);
            int x = db.Acd_Offered_Course.Where(oc => oc.Offered_Course_id == acd_Offered_Course_Lecturer.Offered_Course_id).Select(oc=>oc.Offered_Course_id).First();
            db.Acd_Offered_Course_Lecturer.Remove(acd_Offered_Course_Lecturer);
            db.SaveChanges();
            return RedirectToAction("EditDosen", "OfferedCourse", new { id = x, UrlReferrer = UrlReferrer });
            //return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
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
