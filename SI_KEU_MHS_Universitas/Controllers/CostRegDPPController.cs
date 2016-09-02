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
using System.Web.Configuration;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CostRegDPPController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CostRegDPP
        public ActionResult Index(short? Entry_Year_Id, short? Entry_Period_Type_Id)
        {
            short entryYearId = Entry_Year_Id ?? 0;
            short entryPeriodTypeId = Entry_Period_Type_Id ?? 0;

            if (Entry_Year_Id != null && Entry_Period_Type_Id != null)
            {
                ViewBag.EntryYearId = Entry_Year_Id;
                ViewBag.EntryPeriodTypeId = Entry_Period_Type_Id;
            }

            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Id", Entry_Year_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Name", Entry_Period_Type_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab2" +
                                  " 'SELECT idProdi=ct.Department_Id, [Kode Prodi]=dp.Department_Code, idClassProg=ct.Class_Prog_Id, [Nama Prodi]=dp.Department_Name, [Program Kelas]=cp.Class_Program_Name," +
                                          " Angsuran=ct.Payment_Order," +
                                          " amount=cast(ct.Amount as Varchar(20)) +''|''+ cast(ct.Cost_Timing_Id as Varchar(20)) +''|''+ ty.Term_Year_Name" +
                                   " FROM Fnc_Cost_Timing ct" +
                                        " INNER JOIN Fnc_Cost_Item ci ON ci.Cost_Item_Id = ct.Cost_Item_Id" +
                                        " INNER JOIN Mstr_Entry_Year ey ON ey.Entry_Year_Id = ct.Entry_Year_Id" +
                                        " INNER JOIN Mstr_Term_Year ty ON ty.Term_Year_Id = ct.Term_Year_Id" +
                                        " INNER JOIN Mstr_Department dp ON dp.Department_Id = ct.Department_Id" +
                                        " INNER JOIN Mstr_Class_Program cp ON cp.Class_Prog_Id = ct.Class_Prog_Id" +
                                        " INNER JOIN Mstr_Entry_Period_Type ept ON ept.Entry_Period_Type_Id = ct.Entry_Period_Type_Id" +
                                   " WHERE ey.Entry_Year_Id = " + entryYearId + " AND ept.Entry_Period_Type_Id = " + entryPeriodTypeId + "'," +
                                  " 'idProdi int, [Kode Prodi]  Varchar(100), idClassProg int, [Nama Prodi] Varchar(100), [Program Kelas] Varchar(100), Angsuran int, amount Varchar(100)'," +
                                  " 'Angsuran'," +
                                  " 'MAX(amount)'," +
                                  " ''," +
                                  " 'ASC'," +
                                  " '[Kode Prodi], [Program Kelas] DESC' ";
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

        public ActionResult CopyData(short Entry_Year_Id, short Entry_Period_Type_Id)
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(cp => cp.Entry_Year_Id == Entry_Year_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == Entry_Period_Type_Id).First();

            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year, "Entry_Year_Id", "Entry_Year_Id");
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyData(short Entry_Year_Id, short Entry_Period_Type_Id, short EntryYearId, short EntryPeriodTypeId)
        {
            //Sumber Data untuk diCopy
            var dataSource = db.Fnc_Cost_Timing.Where(ct => ct.Entry_Year_Id == Entry_Year_Id && ct.Entry_Period_Type_Id == Entry_Period_Type_Id).ToList();
            if (dataSource.Count() != 0)
            {
                //Delete Data Lama
                db.Fnc_Cost_Timing.Where(cr => cr.Entry_Year_Id == EntryYearId && cr.Entry_Period_Type_Id == EntryPeriodTypeId).Delete();
                
                foreach (var item in dataSource)
                {
                    Fnc_Cost_Timing objNew = new Fnc_Cost_Timing();
                    objNew.Department_Id = item.Department_Id;
                    objNew.Class_Prog_Id = item.Class_Prog_Id;
                    objNew.Entry_Year_Id = EntryYearId;
                    objNew.Cost_Item_Id = item.Cost_Item_Id;
                    objNew.Entry_Period_Type_Id = EntryPeriodTypeId;
                    objNew.Payment_Order = item.Payment_Order;
                    objNew.Term_Year_Id = item.Term_Year_Id;
                    objNew.Due_Date = item.Due_Date;
                    objNew.Amount = item.Amount;

                    //Insert New Data
                    db.Fnc_Cost_Timing.Add(objNew);
                }
                db.SaveChanges();
            }
            else
            {
                TempData["shortMessage"] = "Sumber Data yang Anda pilih untuk diCopy, Kosong!";
                return RedirectToAction("CopyData", new { Entry_Year_Id = EntryYearId, Entry_Period_Type_Id = EntryPeriodTypeId });
            }

            return RedirectToAction("Index", new { Entry_Year_Id = EntryYearId, Entry_Period_Type_Id = EntryPeriodTypeId });
        }

        //// GET: CostRegDPP/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Cost_Timing fnc_Cost_Timing = db.Fnc_Cost_Timing.Find(id);
        //    if (fnc_Cost_Timing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Cost_Timing);
        //}

        // GET: CostRegDPP/Create

        public ActionResult Create( short? Entry_Year_Id, short? Entry_Period_Type_Id, short? Payment_Order, short? Department_Id, short? Class_Prog_Id)
        {
            ViewBag.PaymentOrder = Payment_Order;
            ViewBag.DepartmentId = Department_Id;
            ViewBag.ClassProgId = Class_Prog_Id;

            if (Payment_Order != null && Department_Id != null && Class_Prog_Id != null)
            {
                ViewBag.PaymentOrders = Payment_Order;
                ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == Department_Id).First();
                ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == Class_Prog_Id).First();
            }
            else
            {
                int JmlMaxAngsuranDPP = Convert.ToByte(WebConfigurationManager.AppSettings["JmlMaxAngsuranNonDPP"]);
                List<angsuran> payment_order = new List<angsuran>();
                for (short i = 1; i <= JmlMaxAngsuranDPP; i++)
                {
                    payment_order.Add(new angsuran { Stage_Id = i });
                }
                ViewBag.PaymentOrders = payment_order;
                //ViewBag.PaymentOrders = db.Fnc_Cost_Timing.GroupBy(ct => ct.Payment_Order).Select(ct => ct.FirstOrDefault()).OrderBy(ct => ct.Payment_Order).ToList();
                ViewBag.Departments = db.Mstr_Department.OrderBy(d => d.Department_Id).ToList();
                ViewBag.ClassProgs = db.Mstr_Class_Program.OrderBy(cp => cp.Class_Prog_Id).ToList();
            }

            ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == Entry_Year_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == Entry_Period_Type_Id).First();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name");

            return View();
        }

        // POST: CostRegDPP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cost_Timing_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Cost_Item_Id,Entry_Period_Type_Id,Payment_Order,Term_Year_Id,Due_Date,Amount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Timing fnc_Cost_Timing, string SAmount)
        {
            fnc_Cost_Timing.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));
            
            if (ModelState.IsValid)
            {
                db.Fnc_Cost_Timing.Add(fnc_Cost_Timing);
                db.SaveChanges();
                return RedirectToAction("Index", new { Entry_Year_Id = fnc_Cost_Timing.Entry_Year_Id, Entry_Period_Type_Id = fnc_Cost_Timing.Entry_Period_Type_Id });
            }

            int JmlMaxAngsuranDPP = Convert.ToByte(WebConfigurationManager.AppSettings["JmlMaxAngsuranNonDPP"]);
            List<angsuran> payment_order = new List<angsuran>();
            for (short i = 1; i <= JmlMaxAngsuranDPP; i++)
            {
                payment_order.Add(new angsuran { Stage_Id = i });
            }
            ViewBag.PaymentOrders = payment_order;
            ViewBag.Departments = db.Mstr_Department.OrderBy(d => d.Department_Id).ToList();
            ViewBag.ClassProgs = db.Mstr_Class_Program.OrderBy(cp => cp.Class_Prog_Id).ToList();
            ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Cost_Timing.Entry_Year_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Timing.Entry_Period_Type_Id).First();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name");

            return View(fnc_Cost_Timing);
        }

        // GET: CostRegDPP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Timing fnc_Cost_Timing = db.Fnc_Cost_Timing.Find(id);
            if (fnc_Cost_Timing == null)
            {
                return HttpNotFound();
            }
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", fnc_Cost_Timing.Term_Year_Id);

            return View(fnc_Cost_Timing);
        }

        // POST: CostRegDPP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cost_Timing_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Cost_Item_Id,Entry_Period_Type_Id,Payment_Order,Term_Year_Id,Due_Date,Amount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Timing fnc_Cost_Timing, string SAmount)
        {
            fnc_Cost_Timing.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Entry(fnc_Cost_Timing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Entry_Year_Id = fnc_Cost_Timing.Entry_Year_Id, Entry_Period_Type_Id = fnc_Cost_Timing.Entry_Period_Type_Id });
            }

            return View(fnc_Cost_Timing);
        }

        // GET: CostRegDPP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Timing fnc_Cost_Timing = db.Fnc_Cost_Timing.Find(id);
            if (fnc_Cost_Timing == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Cost_Timing);
        }

        // POST: CostRegDPP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fnc_Cost_Timing fnc_Cost_Timing = db.Fnc_Cost_Timing.Find(id);
            Mstr_Entry_Period_Type mstr_Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Timing.Entry_Period_Type_Id).First();
            db.Fnc_Cost_Timing.Remove(fnc_Cost_Timing);
            db.SaveChanges();
            return RedirectToAction("Index", new { Entry_Year_Id = fnc_Cost_Timing.Entry_Year_Id, Entry_Period_Type_Id = mstr_Entry_Period_Type.Entry_Period_Type_Id });
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
        public JsonResult IsDataExists(int? intDepartment_Id, int? intEntry_Period_Type_Id, int? intEntry_Year_Id, int? intPayment_Order, int? intClass_Prog_Id, int? intCost_Item_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Department_Id"].Equals("") || Request.QueryString["Payment_Order"].Equals("") || Request.QueryString["Entry_Year_Id"].Equals("") || Request.QueryString["Entry_Period_Type_Id"].Equals("") || Request.QueryString["Cost_Item_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);
                intClass_Prog_Id = Convert.ToInt32(Request.QueryString["Class_Prog_Id"]);
                intEntry_Period_Type_Id = Convert.ToInt32(Request.QueryString["Entry_Period_Type_Id"]);
                intEntry_Year_Id = Convert.ToInt32(Request.QueryString["Entry_Year_Id"]);
                intPayment_Order = Convert.ToInt32(Request.QueryString["Payment_Order"]);
                intCost_Item_Id = Convert.ToInt32(Request.QueryString["Cost_Item_Id"]);
                var model = db.Fnc_Cost_Timing.Where(ct => (intDepartment_Id.HasValue) ?
                    (ct.Department_Id == intDepartment_Id && ct.Entry_Period_Type_Id == intEntry_Period_Type_Id &&
                     ct.Entry_Year_Id == intEntry_Year_Id && ct.Payment_Order == intPayment_Order &&
                     ct.Cost_Item_Id == intCost_Item_Id && ct.Class_Prog_Id == intClass_Prog_Id) :
                    (ct.Class_Prog_Id == intClass_Prog_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
