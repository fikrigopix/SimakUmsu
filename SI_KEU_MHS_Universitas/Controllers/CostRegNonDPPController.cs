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
using EntityFramework.Extensions;
using System.Text.RegularExpressions;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CostRegNonDPPController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CostRegNonDPP
        public ActionResult Index(short? Class_Prog_Id, short? Term_Year_Id, short? Department_Id, short? Entry_Period_Type_Id)
        {
            short clasProgId = Class_Prog_Id ?? 0;
            short termYearId = Term_Year_Id ?? 0;
            short departmentId = Department_Id ?? 0;
            short entryPeriodTypeId = Entry_Period_Type_Id ?? 0;

            if (Class_Prog_Id != null && Term_Year_Id != null && Department_Id != null && Entry_Period_Type_Id != null)
            {
                ViewBag.TermYearId = Term_Year_Id;
                ViewBag.DepartmentId = Department_Id;
                ViewBag.ClasProgId = Class_Prog_Id;
                ViewBag.EntryPeriodTypeId = Entry_Period_Type_Id;
            }

            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name", Department_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", Class_Prog_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Name", Entry_Period_Type_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab2" +
                                  " 'SELECT idCi=ci.Cost_Item_Id, [Kode Biaya]=ci.Cost_Item_Code, [Nama Biaya]=ci.Cost_Item_Name," +
                                          " Angkatan=ey.Entry_Year_Id," +
                                          " amount=cast(cr.Amount as Varchar(20)) +''|''+ cast(cr.Cost_Regular_Id as Varchar(20))" +
                                  " FROM Fnc_Cost_Regular cr" +
                                    " INNER JOIN Fnc_Cost_Item ci ON ci.Cost_Item_Id = cr.Cost_Item_Id" +
                                    " INNER JOIN Mstr_Entry_Year ey ON ey.Entry_Year_Id = cr.Entry_Year_Id" +
                                    " INNER JOIN Mstr_Term_Year ty ON ty.Term_Year_Id = cr.Term_Year_Id" +
                                    " INNER JOIN Mstr_Department dp ON dp.Department_Id = cr.Department_Id" +
                                    " INNER JOIN Mstr_Class_Program cp ON cp.Class_Prog_Id = cr.Class_Prog_Id" +
                                    " INNER JOIN Mstr_Entry_Period_Type ept ON ept.Entry_Period_Type_Id = cr.Entry_Period_Type_Id" +
                                  " WHERE ty.Term_Year_Id = " + termYearId + " AND dp.Department_Id = " + departmentId + " AND cp.Class_Prog_Id = " + clasProgId + " AND ept.Entry_Period_Type_Id = " + entryPeriodTypeId + "'," +
                                  " 'idCi smallint, [Kode Biaya] Varchar(100), [Nama Biaya] Varchar(100), Angkatan Varchar(4), amount Varchar(100)'," +
                                  " 'Angkatan'," +
                                  " 'MAX(amount)'," +
                                  " ''," +
                                  " 'DESC' ";
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

        public ActionResult CopyData(short Class_Prog_Id, short Term_Year_Id, short Department_Id, short Entry_Period_Type_Id)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == Entry_Period_Type_Id).First();

            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name");
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name");
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name");
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyData(short Class_Prog_Id, short Term_Year_Id, short Department_Id, short Entry_Period_Type_Id, short ClassProgId, short TermYearId, short DepartmentId, short EntryPeriodTypeId)
        {
            //Sumber Data untuk diCopy
            var dataSource = db.Fnc_Cost_Regular.Where(cr => cr.Class_Prog_Id == Class_Prog_Id &&
                                                          cr.Term_Year_Id == Term_Year_Id &&
                                                          cr.Department_Id == Department_Id &&
                                                          cr.Entry_Period_Type_Id == Entry_Period_Type_Id).ToList();
            if (dataSource.Count() != 0)
            {
                //Delete Data Lama
                db.Fnc_Cost_Regular.Where(cr => cr.Class_Prog_Id == ClassProgId &&
                                                                cr.Term_Year_Id == TermYearId &&
                                                                cr.Department_Id == DepartmentId &&
                                                                cr.Entry_Period_Type_Id == EntryPeriodTypeId).Delete();
                foreach (var item in dataSource)
                {
                    Fnc_Cost_Regular objNew = new Fnc_Cost_Regular();
                    objNew.Term_Year_Id = TermYearId;
                    objNew.Department_Id = DepartmentId;
                    objNew.Class_Prog_Id = ClassProgId;
                    objNew.Entry_Year_Id = item.Entry_Year_Id;
                    objNew.Entry_Period_Type_Id = EntryPeriodTypeId;
                    objNew.Cost_Item_Id = item.Cost_Item_Id;
                    objNew.Amount = item.Amount;

                    //Insert New Data
                    db.Fnc_Cost_Regular.Add(objNew);
                }
                db.SaveChanges();
            }
            else
            {
                TempData["shortMessage"] = "Sumber Data yang Anda pilih untuk diCopy, Kosong!";
                return RedirectToAction("CopyData", new { Class_Prog_Id = ClassProgId, Term_Year_Id = TermYearId, Department_Id = DepartmentId, Entry_Period_Type_Id = EntryPeriodTypeId });
            }

            return RedirectToAction("Index", new { Class_Prog_Id = ClassProgId, Term_Year_Id = TermYearId, Department_Id = DepartmentId, Entry_Period_Type_Id = EntryPeriodTypeId });
        }

        //// GET: CostRegNonDPP/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Cost_Regular fnc_Cost_Regular = db.Fnc_Cost_Regular.Find(id);
        //    if (fnc_Cost_Regular == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Cost_Regular);
        //}

        // GET: CostRegNonDPP/Create

        public ActionResult Create(short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, short? Entry_Period_Type_Id, short? Entry_Year_Id, short? Cost_Item_Id)
        {
            ViewBag.Angkatan = Entry_Year_Id;
            ViewBag.Biaya = Cost_Item_Id;

            if (Entry_Year_Id != null && Cost_Item_Id != null)
            {
                ViewBag.Entry_Years = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == Entry_Year_Id).First();
                ViewBag.Biayas = db.Fnc_Cost_Item.Where(ci => ci.Cost_Item_Id == Cost_Item_Id).First();
            }
            else
            {
                ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
                ViewBag.Biayas = db.Fnc_Cost_Item.ToList();
            }

            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ep => ep.Entry_Period_Type_Id == Entry_Period_Type_Id).First();

            return View();
        }

        // POST: CostRegNonDPP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cost_Regular_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Entry_Period_Type_Id,Cost_Item_Id,Amount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Regular fnc_Cost_Regular, string SAmount)
        {
            fnc_Cost_Regular.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Fnc_Cost_Regular.Add(fnc_Cost_Regular);
                db.SaveChanges();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Cost_Regular.Class_Prog_Id, Term_Year_Id = fnc_Cost_Regular.Term_Year_Id, Department_Id = fnc_Cost_Regular.Department_Id, Entry_Period_Type_Id = fnc_Cost_Regular.Entry_Period_Type_Id});
            }

            ViewBag.Entry_Years = db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id).ToList();
            ViewBag.Biayas = db.Fnc_Cost_Item.ToList();
            ViewBag.Term_Year = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == fnc_Cost_Regular.Term_Year_Id).First();
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == fnc_Cost_Regular.Department_Id).First();
            ViewBag.Class_Prog = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Cost_Regular.Class_Prog_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ep => ep.Entry_Period_Type_Id == fnc_Cost_Regular.Entry_Period_Type_Id).First();
            
            return View(fnc_Cost_Regular);
        }

        // GET: CostRegNonDPP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Regular fnc_Cost_Regular = db.Fnc_Cost_Regular.Find(id);
            if (fnc_Cost_Regular == null)
            {
                return HttpNotFound();
            }

            return View(fnc_Cost_Regular);
        }

        // POST: CostRegNonDPP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cost_Regular_Id,Term_Year_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Entry_Period_Type_Id,Cost_Item_Id,Amount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Regular fnc_Cost_Regular, string SAmount)
        {
            fnc_Cost_Regular.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Entry(fnc_Cost_Regular).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Class_Prog_Id = fnc_Cost_Regular.Class_Prog_Id, Term_Year_Id = fnc_Cost_Regular.Term_Year_Id, Department_Id = fnc_Cost_Regular.Department_Id, Entry_Period_Type_Id = fnc_Cost_Regular.Entry_Period_Type_Id });
            }

            return View(fnc_Cost_Regular);
        }

        // GET: CostRegNonDPP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Regular fnc_Cost_Regular = db.Fnc_Cost_Regular.Find(id);
            if (fnc_Cost_Regular == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Cost_Regular);
        }

        // POST: CostRegNonDPP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fnc_Cost_Regular fnc_Cost_Regular = db.Fnc_Cost_Regular.Find(id);
            Mstr_Class_Program mstr_Class_Program = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Cost_Regular.Class_Prog_Id).First();
            Mstr_Entry_Period_Type mstr_Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Regular.Entry_Period_Type_Id).First();
            db.Fnc_Cost_Regular.Remove(fnc_Cost_Regular);
            db.SaveChanges();
            return RedirectToAction("Index", new { Class_Prog_Id = mstr_Class_Program.Class_Prog_Id, Term_Year_Id = fnc_Cost_Regular.Term_Year_Id, Department_Id = fnc_Cost_Regular.Department_Id, Entry_Period_Type_Id = mstr_Entry_Period_Type.Entry_Period_Type_Id });
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
        public JsonResult IsDataExists(int? intTerm_Year_Id, int? intDepartment_Id, int? intClass_Prog_Id, int? intEntry_Period_Type_Id, int? intEntry_Year_Id, int? intCost_Item_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Department_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Entry_Year_Id"].Equals("") || Request.QueryString["Entry_Period_Type_Id"].Equals("") || Request.QueryString["Cost_Item_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intTerm_Year_Id = Convert.ToInt32(Request.QueryString["Term_Year_Id"]);
                intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);
                intClass_Prog_Id = Convert.ToInt32(Request.QueryString["Class_Prog_Id"]);
                intEntry_Period_Type_Id = Convert.ToInt32(Request.QueryString["Entry_Period_Type_Id"]);
                intEntry_Year_Id = Convert.ToInt32(Request.QueryString["Entry_Year_Id"]);
                intCost_Item_Id = Convert.ToInt32(Request.QueryString["Cost_Item_Id"]);
                var model = db.Fnc_Cost_Regular.Where(cr => (intTerm_Year_Id.HasValue) ?
                    (cr.Term_Year_Id == intTerm_Year_Id && cr.Department_Id == intDepartment_Id &&
                     cr.Class_Prog_Id == intClass_Prog_Id && cr.Entry_Period_Type_Id == intEntry_Period_Type_Id &&
                     cr.Entry_Year_Id == intEntry_Year_Id && cr.Cost_Item_Id == intCost_Item_Id) :
                    (cr.Cost_Item_Id == intCost_Item_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
