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
using System.Web.Configuration;
using System.Data.Entity.Infrastructure;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CostSchedController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CostSched
        public ActionResult Index(short? Entry_Year_Id, short? Entry_Period_Type_Id)
        {
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
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
                                          " date=cast(ct.Cost_Sched_Id as Varchar(20)) +''|''+ cast(CONVERT(date,ct.Start_Date) as Varchar(20)) +''|''+ cast(CONVERT(date,ct.End_Date) as Varchar(20)) +''|''+ ty.Term_Year_Name" +
                                   " FROM Fnc_Cost_Sched ct" +
                                        " INNER JOIN Mstr_Entry_Year ey ON ey.Entry_Year_Id = ct.Entry_Year_Id" +
                                        " INNER JOIN Mstr_Term_Year ty ON ty.Term_Year_Id = ct.Term_Year_Id" +
                                        " INNER JOIN Mstr_Department dp ON dp.Department_Id = ct.Department_Id" +
                                        " INNER JOIN Mstr_Class_Program cp ON cp.Class_Prog_Id = ct.Class_Prog_Id" +
                                        " INNER JOIN Mstr_Entry_Period_Type ept ON ept.Entry_Period_Type_Id = ct.Entry_Period_Type_Id" +
                                   " WHERE ey.Entry_Year_Id =  " + entryYearId + " AND ept.Entry_Period_Type_Id =  " + entryPeriodTypeId + "'," +
                                  " 'idProdi int, [Kode Prodi]  Varchar(100), idClassProg int, [Nama Prodi] Varchar(100), [Program Kelas] Varchar(100), Angsuran int, date Varchar(100)'," +
                                  " 'Angsuran'," +
                                  " 'MAX(date)'," +
                                  " ''," +
                                  " 'ASC'," +
                                  " '[Kode Prodi], [Program Kelas] DESC'";
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

        // GET: CostSched/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Cost_Sched fnc_Cost_Sched = db.Fnc_Cost_Sched.Find(id);
        //    if (fnc_Cost_Sched == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Cost_Sched);
        //}

        // GET: CostSched/Create
        public ActionResult Create(short? Entry_Year_Id, short? Entry_Period_Type_Id, short? Payment_Order, short? Department_Id, short? Class_Prog_Id, string UrlReferrer)
        {
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
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
                int JmlMaxAngsuranSPP = Convert.ToByte(WebConfigurationManager.AppSettings["JmlMaxAngsuranSPP"]);
                List<angsuran> payment_order = new List<angsuran>();
                for (short i = 0; i <= JmlMaxAngsuranSPP; i++)
                {
                    payment_order.Add(new angsuran
                    {
                        Stage_Id = i,
                        PaymentOrder = (i == 0) ? "Lunas" : i.ToString()
                    });
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

        // POST: CostSched/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cost_Sched_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Entry_Period_Type_Id,Payment_Order,Term_Year_Id,Start_Date,End_Date,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Sched fnc_Cost_Sched, string UrlReferrer)
        {
            ViewBag.UrlReferrer = UrlReferrer;
            if (ModelState.IsValid)
            {
                db.Fnc_Cost_Sched.Add(fnc_Cost_Sched);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }
            int JmlMaxAngsuranSPP = Convert.ToByte(WebConfigurationManager.AppSettings["JmlMaxAngsuranSPP"]);
            List<angsuran> payment_order = new List<angsuran>();
            for (short i = 0; i <= JmlMaxAngsuranSPP; i++)
            {
                payment_order.Add(new angsuran
                {
                    Stage_Id = i,
                    PaymentOrder = (i == 0) ? "Lunas" : i.ToString()
                });
            }

            ViewBag.PaymentOrders = payment_order;
            ViewBag.Departments = db.Mstr_Department.OrderBy(d => d.Department_Id).ToList();
            ViewBag.ClassProgs = db.Mstr_Class_Program.OrderBy(cp => cp.Class_Prog_Id).ToList();
            ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Cost_Sched.Entry_Year_Id).First();
            ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Sched.Entry_Period_Type_Id).First();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name");

            return View(fnc_Cost_Sched);
        }

        // GET: CostSched/Edit/5
        public ActionResult Edit(int? id, string UrlReferrer)
        {
            ViewBag.UrlReferrer = UrlReferrer ?? System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Sched fnc_Cost_Sched = db.Fnc_Cost_Sched.Find(id);
            if (fnc_Cost_Sched == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PaymentOrders = fnc_Cost_Sched.Payment_Order;
            //ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == fnc_Cost_Sched.Department_Id).First();
            //ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Cost_Sched.Class_Prog_Id).First();
            //ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Cost_Sched.Entry_Year_Id).First();
            //ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Sched.Entry_Period_Type_Id).First();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", fnc_Cost_Sched.Term_Year_Id);

            return View(fnc_Cost_Sched);
        }

        // POST: CostSched/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cost_Sched_Id,Department_Id,Class_Prog_Id,Entry_Year_Id,Entry_Period_Type_Id,Payment_Order,Term_Year_Id,Start_Date,End_Date,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Sched fnc_Cost_Sched, string UrlReferrer)
        {
            ViewBag.UrlReferrer = UrlReferrer;
            if (ModelState.IsValid)
            {
                db.Entry(fnc_Cost_Sched).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                    return RedirectToAction("Edit");
                }
                return Redirect(UrlReferrer);
            }
            //ViewBag.PaymentOrders = fnc_Cost_Sched.Payment_Order;
            //ViewBag.Departments = db.Mstr_Department.Where(d => d.Department_Id == fnc_Cost_Sched.Department_Id).First();
            //ViewBag.ClassProgs = db.Mstr_Class_Program.Where(cp => cp.Class_Prog_Id == fnc_Cost_Sched.Class_Prog_Id).First();
            //ViewBag.Entry_Year = db.Mstr_Entry_Year.Where(ey => ey.Entry_Year_Id == fnc_Cost_Sched.Entry_Year_Id).First();
            //ViewBag.Entry_Period_Type = db.Mstr_Entry_Period_Type.Where(ept => ept.Entry_Period_Type_Id == fnc_Cost_Sched.Entry_Period_Type_Id).First();
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", fnc_Cost_Sched.Term_Year_Id);

            return View(fnc_Cost_Sched);
        }

        // GET: CostSched/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Cost_Sched fnc_Cost_Sched = db.Fnc_Cost_Sched.Find(id);
        //    if (fnc_Cost_Sched == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Cost_Sched);
        //}

        // POST: CostSched/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Fnc_Cost_Sched fnc_Cost_Sched = db.Fnc_Cost_Sched.Find(id);
            if (fnc_Cost_Sched == null)
            {
                return HttpNotFound();
            }
            db.Fnc_Cost_Sched.Remove(fnc_Cost_Sched);
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
        public JsonResult IsDataExists(int? intDepartment_Id, int? intEntry_Period_Type_Id, int? intEntry_Year_Id, int? intPayment_Order, int? intClass_Prog_Id)
        {

            if (Request.QueryString["Class_Prog_Id"].Equals("") || Request.QueryString["Department_Id"].Equals("") || Request.QueryString["Payment_Order"].Equals("") || Request.QueryString["Entry_Year_Id"].Equals("") || Request.QueryString["Entry_Period_Type_Id"].Equals(""))
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
                var model = db.Fnc_Cost_Sched.Where(ct => (intDepartment_Id.HasValue) ?
                    (ct.Department_Id == intDepartment_Id && ct.Entry_Period_Type_Id == intEntry_Period_Type_Id &&
                     ct.Entry_Year_Id == intEntry_Year_Id && ct.Payment_Order == intPayment_Order &&
                     ct.Class_Prog_Id == intClass_Prog_Id) : (ct.Class_Prog_Id == intClass_Prog_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
