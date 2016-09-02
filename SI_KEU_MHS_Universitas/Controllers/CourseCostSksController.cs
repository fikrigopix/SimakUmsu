using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SI_KEU_MHS_Universitas.Models;
using System.Data.Common;
using System.Dynamic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CourseCostSksController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CourseCostSks
        public ActionResult Index(short? Class_Prog_Id, short? Term_Year_Id)
        {
            short clasProgId = Class_Prog_Id ?? 0;
            short termYearId = Term_Year_Id ?? 0;

            if (Class_Prog_Id != null && Term_Year_Id != null)
            {
                ViewBag.ClasProgId = Class_Prog_Id;
                ViewBag.TermYearId = Term_Year_Id;
            }

            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", Class_Prog_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab" +
                                  " 'SELECT id=dp.Department_Id, Kode=dp.Department_Code, Prodi=dp.Department_Name," +
                                          " angkatan=ccs.Entry_Year_Id," +
                                          " amount=cast(ccs.Amount_Per_Sks as varchar(20))+''|''+ cast(ccs.Course_Cost_Sks_Id as varchar(20))" +
                                  " FROM Fnc_Course_Cost_Sks ccs" +
                                  " INNER JOIN Mstr_Department dp ON dp.Department_Id = ccs.Department_Id" +
                                  " WHERE ccs.Term_Year_Id = " + termYearId + " AND ccs.Class_Prog_Id = " + clasProgId + "'," +
                                        " 'angkatan'," +
                                        " 'MAX(amount)[]'," +
                                        " 'id,Kode, Prodi'," +
                                        " 'DESC'," +
                                        " 'kode' ";
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

        //// GET: CourseCostSks/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Course_Cost_Sks fnc_Course_Cost_Sks = db.Fnc_Course_Cost_Sks.Find(id);
        //    if (fnc_Course_Cost_Sks == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Course_Cost_Sks);
        //}

        // GET: CourseCostSks/Create

        public ActionResult Create(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id, short? Entry_Year_Id)
        {
            ViewBag.Prodi = Department_Id;
            ViewBag.Angkatan = Entry_Year_Id;

            if (Department_Id != null && Entry_Year_Id != null)
            {
                ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
                ViewBag.Entry_Years = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == Entry_Year_Id).First();
            }
            else
            {
                ViewBag.Departments = db.Mstr_Department_Class_Program.Where(d=>d.Class_Prog_Id==Class_Prog_Id).OrderBy(d => d.Mstr_Department.Department_Code).ToList();
                ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
            }
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();

            return View();
        }

        // POST: CourseCostSks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Cost_Sks_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Amount_Per_Sks,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Sks fnc_Course_Cost_Sks, string SAmount)
        {
            fnc_Course_Cost_Sks.Amount_Per_Sks = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Fnc_Course_Cost_Sks.Add(fnc_Course_Cost_Sks);
                db.SaveChanges();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Sks.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Sks.Term_Year_Id });
            }

            ViewBag.Departments = db.Mstr_Department_Class_Program.Where(d => d.Class_Prog_Id == fnc_Course_Cost_Sks.Class_Prog_Id).OrderBy(d => d.Mstr_Department.Department_Code).ToList();
            ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Course_Cost_Sks.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Sks.Term_Year_Id).First();
            return View(fnc_Course_Cost_Sks);
        }

        // GET: CourseCostSks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Course_Cost_Sks fnc_Course_Cost_Sks = db.Fnc_Course_Cost_Sks.Find(id);
            if (fnc_Course_Cost_Sks == null)
            {
                return HttpNotFound();
            }
            ViewBag.Term_Year_Name = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Sks.Term_Year_Id).Select(ty => ty.Term_Year_Name).First();
            return View(fnc_Course_Cost_Sks);
        }

        // POST: CourseCostSks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Cost_Sks_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Amount_Per_Sks,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Sks fnc_Course_Cost_Sks, string SAmount)
        {
            fnc_Course_Cost_Sks.Amount_Per_Sks = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Entry(fnc_Course_Cost_Sks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Sks.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Sks.Term_Year_Id });
            }
            ViewBag.Term_Year_Name = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Sks.Term_Year_Id).Select(ty => ty.Term_Year_Name).First();
            return View(fnc_Course_Cost_Sks);
        }

        // GET: CourseCostSks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Course_Cost_Sks fnc_Course_Cost_Sks = db.Fnc_Course_Cost_Sks.Find(id);
            if (fnc_Course_Cost_Sks == null)
            {
                return HttpNotFound();
            }
            ViewBag.Term_Year_Name = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Sks.Term_Year_Id).Select(ty => ty.Term_Year_Name).First();
            return View(fnc_Course_Cost_Sks);
        }

        // POST: CourseCostSks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fnc_Course_Cost_Sks fnc_Course_Cost_Sks = db.Fnc_Course_Cost_Sks.Find(id);
            Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Course_Cost_Sks.Class_Prog_Id).First();
            db.Fnc_Course_Cost_Sks.Remove(fnc_Course_Cost_Sks);
            db.SaveChanges();
            return RedirectToAction("Index", new { Class_Prog_Id = mstr_Class_Program.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Sks.Term_Year_Id });
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
        public JsonResult IsDataExists(int? intDepartment_Id, int? intEntry_Year_Id, int? intTerm_Year_Id, int? intClass_Prog_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Entry_Year_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);
                intEntry_Year_Id = Convert.ToInt32(Request.QueryString["Entry_Year_Id"]);
                intTerm_Year_Id = Convert.ToInt32(Request.QueryString["Term_Year_Id"]);
                intClass_Prog_Id = Convert.ToInt32(Request.QueryString["Class_Prog_Id"]);
                var model = db.Fnc_Course_Cost_Sks.Where(ccs => (intClass_Prog_Id.HasValue) ?
                    (ccs.Class_Prog_Id == intClass_Prog_Id && ccs.Term_Year_Id == intTerm_Year_Id && ccs.Department_Id == intDepartment_Id && ccs.Entry_Year_Id == intEntry_Year_Id) :
                    (ccs.Department_Id == intDepartment_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
