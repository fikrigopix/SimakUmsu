using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using System.Data.Common;
using System.Dynamic;
using System.ComponentModel;
using EntityFramework.Extensions;
using System.Data.Entity.Infrastructure;
//using System.Text.RegularExpressions;

namespace SIA_Universitas.Controllers
{
    public class CurriculumEntryYearController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: CurriculumEntryYear
        public ActionResult Index(short? Term_Year_Id, short? Department_Id)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            short termYearId = Term_Year_Id ?? 0;
            short departmentId = Department_Id ?? 0;

            if (Term_Year_Id != null && Department_Id != null)
            {
                ViewBag.TermYearId = Term_Year_Id;
                ViewBag.DepartmentId = Department_Id;
            }

            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab2" +
                                  " 'SELECT idEntryYear=cey.Entry_Year_Id, Angkatan = ey.Entry_Year_Name," +
                                            " clasprog = cast(cey.Class_Prog_Id as Varchar(20)) +''|''+ cast(cp.Class_Program_Name as Varchar(20))," +
                                            " kurikulum = cast(cey.Curriculum_Entry_Year_Id as Varchar(20)) +''|''+ cast(c.Curriculum_Name as Varchar(20))" +
                                   " FROM Acd_Curriculum_Entry_Year cey" +
                                        " INNER JOIN Mstr_Term_Year ty ON ty.Term_Year_Id = cey.Term_Year_Id" +
                                        " INNER JOIN Mstr_Department dp ON dp.Department_Id = cey.Department_Id" +
                                        " INNER JOIN Mstr_Class_Program cp ON cp.Class_Prog_Id = cey.Class_Prog_Id" +
                                        " INNER JOIN Mstr_Entry_Year ey ON ey.Entry_Year_Id = cey.Entry_Year_Id" +
                                        " INNER JOIN Mstr_Curriculum c ON c.Curriculum_Id = cey.Curriculum_Id" +
                                   " WHERE ty.Term_Year_Id =  " + termYearId + " AND dp.Department_Id = " + departmentId + "'," +
                                   " 'idEntryYear smallint, Angkatan Varchar(100), clasprog Varchar(100), kurikulum Varchar(100)'," +
                                   " 'clasprog'," +
                                   " 'MAX(kurikulum)'," +
                                   " ''," +
                                   " 'ASC'," +
                                   " 'idEntryYear DESC'";
                using (var reader = cmd.ExecuteReader())
                {
                    var model = this.Read(reader).ToList();
                    context.Connection.Close();
                    return View(model);
                }
            }
        }

        public List<Dictionary<string, object>> Read(DbDataReader reader)
        {
            List<Dictionary<string, object>> expandolist = new List<Dictionary<string, object>>();
            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }
                expandolist.Add(new Dictionary<string, object>(expando));
            }
            return expandolist;
        }

        // GET: CurriculumEntryYear/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year = db.Acd_Curriculum_Entry_Year.Find(id);
            if (acd_Curriculum_Entry_Year == null)
            {
                return HttpNotFound();
            }
            return View(acd_Curriculum_Entry_Year);
        }

        // GET: CurriculumEntryYear/Create
        public ActionResult Create(short? Term_Year_Id, short? Department_Id, short? Entry_Year_Id, short? Class_Prog_Id, string UrlReferrer)
        {
            ViewBag.Angkatan = Entry_Year_Id;
            ViewBag.ClassProg = Class_Prog_Id;

            if (Entry_Year_Id != null && Class_Prog_Id != null)
            {
                ViewBag.EntryYears = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == Entry_Year_Id).First();
                ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            }
            else
            {
                ViewBag.EntryYears = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
                ViewBag.ClassProgs = db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id).ToList();
            }

            ViewBag.Curriculums = db.Mstr_Curriculum_Applied.Where(ca => ca.Department_Id == Department_Id).ToList();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            return View();
        }

        // POST: CurriculumEntryYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curriculum_Entry_Year_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Curriculum_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Curriculum_Entry_Year.Add(acd_Curriculum_Entry_Year);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            //ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Curriculum_Entry_Year.Class_Prog_Id);
            //ViewBag.Curriculum_Id = new SelectList(db.Mstr_Curriculum, "Curriculum_Id", "Curriculum_Code", acd_Curriculum_Entry_Year.Curriculum_Id);
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", acd_Curriculum_Entry_Year.Department_Id);
            //ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year, "Entry_Year_Id", "Entry_Year_Name", acd_Curriculum_Entry_Year.Entry_Year_Id);
            //ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", acd_Curriculum_Entry_Year.Term_Year_Id);
            return View(acd_Curriculum_Entry_Year);
        }

        // GET: CurriculumEntryYear/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year = db.Acd_Curriculum_Entry_Year.Find(id);
            if (acd_Curriculum_Entry_Year == null)
            {
                return HttpNotFound();
            }
            ViewBag.Curriculums = db.Mstr_Curriculum_Applied.Where(ca => ca.Department_Id == acd_Curriculum_Entry_Year.Department_Id).ToList();
            ViewBag.EntryYears = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == acd_Curriculum_Entry_Year.Entry_Year_Id).First();
            ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == acd_Curriculum_Entry_Year.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Curriculum_Entry_Year.Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Curriculum_Entry_Year.Department_Id).First();
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            return View(acd_Curriculum_Entry_Year);
        }

        // POST: CurriculumEntryYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curriculum_Entry_Year_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Curriculum_Id,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Curriculum_Entry_Year).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }
            ViewBag.Curriculums = db.Mstr_Curriculum_Applied.Where(ca => ca.Department_Id ==acd_Curriculum_Entry_Year.Department_Id).ToList();
            ViewBag.EntryYears = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == acd_Curriculum_Entry_Year.Entry_Year_Id).First();
            ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == acd_Curriculum_Entry_Year.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Curriculum_Entry_Year.Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Curriculum_Entry_Year.Department_Id).First();
            ViewBag.UrlReferrer = UrlReferrer;
            return View(acd_Curriculum_Entry_Year);
        }

        // GET: CurriculumEntryYear/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year = db.Acd_Curriculum_Entry_Year.Find(id);
        //    if (acd_Curriculum_Entry_Year == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Curriculum_Entry_Year);
        //}

        // POST: CurriculumEntryYear/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Curriculum_Entry_Year acd_Curriculum_Entry_Year = db.Acd_Curriculum_Entry_Year.Find(id);
            db.Acd_Curriculum_Entry_Year.Remove(acd_Curriculum_Entry_Year);
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
        public JsonResult IsDataExists(int? intTerm_Year_Id, int? intDepartment_Id, int? intClass_Prog_Id, int? intEntry_Year_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Department_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Entry_Year_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intTerm_Year_Id = Convert.ToInt32(Request.QueryString["Term_Year_Id"]);
                intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);
                intClass_Prog_Id = Convert.ToInt32(Request.QueryString["Class_Prog_Id"]);
                intEntry_Year_Id = Convert.ToInt32(Request.QueryString["Entry_Year_Id"]);
                var model = db.Acd_Curriculum_Entry_Year.Where(cey => (intTerm_Year_Id.HasValue) ?
                    (cey.Term_Year_Id == intTerm_Year_Id && cey.Department_Id == intDepartment_Id &&
                     cey.Class_Prog_Id == intClass_Prog_Id && cey.Entry_Year_Id == intEntry_Year_Id) :
                    (cey.Entry_Year_Id == intEntry_Year_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
