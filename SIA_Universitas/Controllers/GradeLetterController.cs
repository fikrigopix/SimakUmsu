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
    public class GradeLetterController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: GradeLetter
        public ActionResult Index(int? page, int? rowPerPage)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            var acd_Grade_Latter = db.Acd_Grade_Letter.OrderBy(gl => gl.Grade_Letter_Id);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;

            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(acd_Grade_Latter.ToPagedList(pageNumber, pageSize));
        }

        // GET: GradeLetter/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Grade_Letter acd_Grade_Letter = db.Acd_Grade_Letter.Find(id);
            if (acd_Grade_Letter == null)
            {
                return HttpNotFound();
            }
            return View(acd_Grade_Letter);
        }

        // GET: GradeLetter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GradeLetter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Grade_Letter_Id,Grade_Letter,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Grade_Letter acd_Grade_Letter)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Grade_Letter.Add(acd_Grade_Letter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acd_Grade_Letter);
        }

        // GET: GradeLetter/Edit/5
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
            Acd_Grade_Letter acd_Grade_Letter = db.Acd_Grade_Letter.Find(id);
            if (acd_Grade_Letter == null)
            {
                return HttpNotFound();
            }
            return View(acd_Grade_Letter);
        }

        // POST: GradeLetter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Grade_Letter_Id,Grade_Letter,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Grade_Letter acd_Grade_Letter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Grade_Letter).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Acd_Grade_Letter Grade_Letter = db.Acd_Grade_Letter.Find(acd_Grade_Letter.Grade_Letter_Id);
                    if (Grade_Letter == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Nilai Huruf telah ada.";
                    return RedirectToAction("Edit", acd_Grade_Letter);
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(acd_Grade_Letter);
        }

        // GET: GradeLetter/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Grade_Letter acd_Grade_Letter = db.Acd_Grade_Letter.Find(id);
        //    if (acd_Grade_Letter == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Grade_Letter);
        //}

        // POST: GradeLetter/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Grade_Letter acd_Grade_Letter = db.Acd_Grade_Letter.Find(id);
            db.Acd_Grade_Letter.Remove(acd_Grade_Letter);
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
        public JsonResult IsGradeLetterExists(string strGradeLetter)
        {
            strGradeLetter = Request.QueryString["Grade_Letter"];
            return Json(!db.Acd_Grade_Letter.Any(gl => gl.Grade_Letter == strGradeLetter), JsonRequestBehavior.AllowGet);
        }
    }
}
