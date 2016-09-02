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
    public class CourseCostPackageController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CourseCostPackage
        public ActionResult Index(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id)
        {
            short clasProgId = Class_Prog_Id ?? 0;
            short termYearId = Term_Year_Id ?? 0;
            short departmentId = Department_Id ?? 0;

            if (Class_Prog_Id != null && Term_Year_Id != null && Department_Id != null)
            {
                ViewBag.ClasProgId = Class_Prog_Id;
                ViewBag.TermYearId = Term_Year_Id;
                ViewBag.DepartmentId = Department_Id;
            }

            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", Class_Prog_Id).ToList();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab"+
                                  " 'SELECT idCct=cct.Course_Cost_Type_Id, Kode=c.Course_Code, Matakuliah=c.Course_Name," +
                                          " angkatan=ccp.Entry_Year_Id,"+
                                          " amount=cast(ccp.Amount_Per_Mk as varchar(20))+''|''+ cast(ccp.Course_Cost_Package_Id as varchar(20))"+
                                   " FROM Fnc_Course_Cost_Package ccp INNER JOIN Fnc_Course_Cost_Type cct ON cct.Course_Cost_Type_Id = ccp.Course_Cost_Type_Id"+
                                                                    " INNER JOIN Acd_Course c ON c.Course_Id = cct.Course_Id"+
                                   " WHERE cct.Term_Year_Id = " + termYearId + " AND cct.Department_Id = " + departmentId + " AND cct.Class_Prog_Id = " + clasProgId + "'," +
                                  " 'angkatan',"+
                                  " 'MAX(amount)[]',"+
                                  " 'idCct,Kode, Matakuliah'," +
                                  " 'DESC',"+
                                  " 'Kode' ";
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

        //// GET: CourseCostPackage/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Course_Cost_Package fnc_Course_Cost_Package = db.Fnc_Course_Cost_Package.Find(id);
        //    if (fnc_Course_Cost_Package == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Course_Cost_Package);
        //}

        // GET: CourseCostPackage/Create

        public ActionResult Create(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id, short? Entry_Year_Id, int? Course_Cost_Type_Id)
        {
            ViewBag.CourseCostTypeId = Course_Cost_Type_Id;
            ViewBag.Prodi = Department_Id;
            ViewBag.Angkatan = Entry_Year_Id;

            if (Course_Cost_Type_Id != null && Entry_Year_Id != null)
            {
                ViewBag.CourseCostTypes = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == Course_Cost_Type_Id).First();
                ViewBag.Entry_Years = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == Entry_Year_Id).First();
            }
            else
            {
                ViewBag.CourseCostTypes = db.Fnc_Course_Cost_Type.Where(cct => cct.Term_Year_Id == Term_Year_Id
                                                                            && cct.Department_Id == Department_Id
                                                                            && cct.Class_Prog_Id == Class_Prog_Id
                                                                            && cct.Is_Sks == false).ToList();
                ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
            }
            ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();

            return View();
        }

        // POST: CourseCostPackage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Cost_Package_Id,Course_Cost_Type_Id,Entry_Year_Id,Amount_Per_Mk,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Package fnc_Course_Cost_Package, string SAmount)
        {
            fnc_Course_Cost_Package.Amount_Per_Mk = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Fnc_Course_Cost_Package.Add(fnc_Course_Cost_Package);
                db.SaveChanges();
                Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == fnc_Course_Cost_Package.Course_Cost_Type_Id).First();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Type.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Type.Term_Year_Id, Department_Id = fnc_Course_Cost_Type.Department_Id });
            }

            ViewBag.CourseCostTypes = db.Fnc_Course_Cost_Type.Where(cct => cct.Term_Year_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Term_Year_Id
                                                                        && cct.Department_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Department_Id
                                                                        && cct.Class_Prog_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Class_Prog_Id).ToList();
            ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
            ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Package.Fnc_Course_Cost_Type.Term_Year_Id).First();
            return View(fnc_Course_Cost_Package);
        }

        // GET: CourseCostPackage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Course_Cost_Package fnc_Course_Cost_Package = db.Fnc_Course_Cost_Package.Find(id);
            Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Find(fnc_Course_Cost_Package.Course_Cost_Type_Id);
            if (fnc_Course_Cost_Package == null)
            {
                return HttpNotFound();
            }
            ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == fnc_Course_Cost_Type.Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Course_Cost_Type.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Type.Term_Year_Id).First();
            ViewBag.CourseCostTypes = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == fnc_Course_Cost_Package.Course_Cost_Type_Id).First();
            ViewBag.Entry_Years = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Course_Cost_Package.Entry_Year_Id).First();
            return View(fnc_Course_Cost_Package);
        }

        // POST: CourseCostPackage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Cost_Package_Id,Course_Cost_Type_Id,Entry_Year_Id,Amount_Per_Mk,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Course_Cost_Package fnc_Course_Cost_Package, string SAmount)
        {
            fnc_Course_Cost_Package.Amount_Per_Mk = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));
            Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == fnc_Course_Cost_Package.Course_Cost_Type_Id).First();
            if (ModelState.IsValid)
            {
                db.Entry(fnc_Course_Cost_Package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Type.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Type.Term_Year_Id, Department_Id = fnc_Course_Cost_Type.Department_Id });
            }
            ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == fnc_Course_Cost_Type.Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Course_Cost_Type.Class_Prog_Id).First();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Course_Cost_Type.Term_Year_Id).First();
            ViewBag.CourseCostTypes = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == fnc_Course_Cost_Package.Course_Cost_Type_Id).First();
            ViewBag.Entry_Years = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Course_Cost_Package.Entry_Year_Id).First();
            return View(fnc_Course_Cost_Package);
        }

        // GET: CourseCostPackage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Course_Cost_Package fnc_Course_Cost_Package = db.Fnc_Course_Cost_Package.Find(id);
            if (fnc_Course_Cost_Package == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Course_Cost_Package);
        }

        // POST: CourseCostPackage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fnc_Course_Cost_Package fnc_Course_Cost_Package = db.Fnc_Course_Cost_Package.Find(id);
            Fnc_Course_Cost_Type fnc_Course_Cost_Type = db.Fnc_Course_Cost_Type.Where(cct => cct.Course_Cost_Type_Id == fnc_Course_Cost_Package.Course_Cost_Type_Id).First();
            db.Fnc_Course_Cost_Package.Remove(fnc_Course_Cost_Package);
            db.SaveChanges();
            return RedirectToAction("Index", new { Class_Prog_Id = fnc_Course_Cost_Type.Class_Prog_Id, Term_Year_Id = fnc_Course_Cost_Type.Term_Year_Id, Department_Id = fnc_Course_Cost_Type.Department_Id });
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
        public JsonResult IsDataExists(int? intCourse_Cost_Type_Id, int? intEntry_Year_Id)
        {

            if (Request.QueryString["Entry_Year_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intCourse_Cost_Type_Id = Convert.ToInt32(Request.QueryString["Course_Cost_Type_Id"]);
                intEntry_Year_Id = Convert.ToInt32(Request.QueryString["Entry_Year_Id"]);
                var model = db.Fnc_Course_Cost_Package.Where(ccp => (intCourse_Cost_Type_Id.HasValue) ?
                    (ccp.Course_Cost_Type_Id == intCourse_Cost_Type_Id && ccp.Entry_Year_Id == intEntry_Year_Id) :
                    (ccp.Course_Cost_Type_Id == intCourse_Cost_Type_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
