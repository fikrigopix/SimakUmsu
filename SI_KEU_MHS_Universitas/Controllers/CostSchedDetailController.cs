using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SI_KEU_MHS_Universitas.Models;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Dynamic;
using System.ComponentModel;

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CostSchedDetailController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CostSchedDetail
        public ActionResult Index(int? CostSchedId, string UrlParent)
        {
            if (CostSchedId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }
            var fnc_Cost_Sched_Detail = db.Fnc_Cost_Sched_Detail.Where(f => f.Cost_Sched_Id == CostSchedId).Include(f => f.Fnc_Cost_Item).Include(f => f.Fnc_Cost_Sched);
            ViewBag.Cost_Sched = db.Fnc_Cost_Sched.Find(CostSchedId);
            if (UrlParent != null)
            {
                ViewBag.UrlReferrer = UrlParent;
            }
            else
            {
                ViewBag.UrlReferrer = (TempData["UrlParent"] != null) ? TempData["UrlParent"].ToString() : System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            }

            return View(fnc_Cost_Sched_Detail.ToList());
        }

        // GET: CostSchedDetail/Resume
        public ActionResult Resume(short? Entry_Year_Id, short? Entry_Period_Type_Id, short? Department_Id, short? Class_Prog_Id)
        {
            short clasProgId = Class_Prog_Id ?? 0;
            short entryYearId = Entry_Year_Id ?? 0;
            short departmentId = Department_Id ?? 0;
            short entryPeriodTypeId = Entry_Period_Type_Id ?? 0;

            if (Class_Prog_Id != null && Entry_Year_Id != null && Department_Id != null && Entry_Period_Type_Id != null)
            {
                ViewBag.EntryYearId = Entry_Year_Id;
                ViewBag.DepartmentId = Department_Id;
                ViewBag.ClasProgId = Class_Prog_Id;
                ViewBag.EntryPeriodTypeId = Entry_Period_Type_Id;
            }

            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year.OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Id", Entry_Year_Id);
            ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Name", Department_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Program_Name", Class_Prog_Id);
            ViewBag.Entry_Period_Type_Id = new SelectList(db.Mstr_Entry_Period_Type, "Entry_Period_Type_Id", "Entry_Period_Type_Name", Entry_Period_Type_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab2"+
                                  " 'SELECT [Nama Biaya]=ci.Cost_Item_Name, angsuran=cs.Payment_Order, amount=cast(csd.Cost_Sched_Detail_Id as Varchar(20)) +''|''+ cast(csd.Amount as Varchar(20))" +
                                   " FROM Fnc_Cost_Sched_Detail csd"+
                                        " INNER JOIN Fnc_Cost_Sched cs ON cs.Cost_Sched_Id = csd.Cost_Sched_Id"+
                                        " INNER JOIN Fnc_Cost_Item ci ON ci.Cost_Item_Id = csd.Cost_Item_id"+
                                   " WHERE cs.Entry_Year_Id = " + entryYearId + " AND" +
                                         " cs.Entry_Period_Type_Id = " + entryPeriodTypeId + " AND" +
                                         " cs.Department_Id = " + departmentId + " AND" +
                                         " cs.Class_Prog_Id = " + clasProgId + "'," +
                                  " '[Nama Biaya] Varchar(100), angsuran int, amount Varchar(100)',"+
                                  " 'angsuran',"+
                                  " 'MIN(amount)',"+
                                  " '',"+
                                  " 'ASC',"+
                                  " '[Nama Biaya]' ";
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

        // GET: CostSchedDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail = db.Fnc_Cost_Sched_Detail.Find(id);
            if (fnc_Cost_Sched_Detail == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Cost_Sched_Detail);
        }

        // GET: CostSchedDetail/Create
        public ActionResult Create(int? CostSchedId, string UrlParent)
        {
            ViewBag.UrlParent = UrlParent;
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            //var exceptionCostItem = db.Fnc_Cost_Sched_Detail.Where(csd => csd.Cost_Sched_Id == id).Select(csd => csd.Cost_Item_id).ToList();
            //ViewBag.Cost_Item_id = new SelectList(db.Fnc_Cost_Item.Where(ci => !exceptionCostItem.Contains(ci.Cost_Item_Id)).OrderBy(ci => ci.Cost_Item_Name), "Cost_Item_Id", "Cost_Item_Name", fnc_Cost_Sched_Detail.Cost_Item_id);
            ViewBag.Biayas = db.Fnc_Cost_Item.OrderBy(ci => ci.Cost_Item_Name).ToList();
            ViewBag.Cost_Sched_Id = CostSchedId;
            return View();
        }

        // POST: CostSchedDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cost_Sched_Detail_Id,Cost_Sched_Id,Cost_Item_id,Amount,SAmount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail, string UrlReferrer, string UrlParent)
        {
            //fnc_Cost_Sched_Detail.Amount = (SAmount != "") ? (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", "")) : fnc_Cost_Sched_Detail.Amount;

            if (ModelState.IsValid)
            {
                db.Fnc_Cost_Sched_Detail.Add(fnc_Cost_Sched_Detail);
                db.SaveChanges();
                TempData["UrlParent"] = UrlParent;
                return Redirect(UrlReferrer);
            }

            ViewBag.UrlReferrer = UrlReferrer;
            //var exceptionCostItem = db.Fnc_Cost_Sched_Detail.Where(csd => csd.Cost_Sched_Id == fnc_Cost_Sched_Detail.Cost_Sched_Id).Select(csd => csd.Cost_Item_id).ToList();
            //ViewBag.Cost_Item_id = new SelectList(db.Fnc_Cost_Item.Where(ci => !exceptionCostItem.Contains(ci.Cost_Item_Id)).OrderBy(ci => ci.Cost_Item_Name), "Cost_Item_Id", "Cost_Item_Name", fnc_Cost_Sched_Detail.Cost_Item_id);
            ViewBag.Biayas = db.Fnc_Cost_Item.OrderBy(ci => ci.Cost_Item_Name).ToList();
            ViewBag.Cost_Sched_Id = fnc_Cost_Sched_Detail.Cost_Sched_Id;
            return View(fnc_Cost_Sched_Detail);
        }

        // GET: CostSchedDetail/Edit/5
        public ActionResult Edit(int? id, string UrlParent)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail = db.Fnc_Cost_Sched_Detail.Find(id);
            if (fnc_Cost_Sched_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.UrlParent = UrlParent;
            ViewBag.UrlReferrer = (TempData["UrlReferrer"] != null) ? TempData["UrlReferrer"].ToString() : System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            ViewBag.Biayas = db.Fnc_Cost_Item.OrderBy(ci => ci.Cost_Item_Name).ToList();
            ViewBag.Cost_Sched_Id = fnc_Cost_Sched_Detail.Cost_Sched_Id;
            return View(fnc_Cost_Sched_Detail);
        }

        // POST: CostSchedDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cost_Sched_Detail_Id,Cost_Sched_Id,Cost_Item_id,Amount,SAmount,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail, string UrlReferrer, string UrlParent)
        {
            //fnc_Cost_Sched_Detail.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));
            if (ModelState.IsValid)
            {
                db.Entry(fnc_Cost_Sched_Detail).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    TempData["shortMessage"] = "Biaya telah ada.";
                    TempData["UrlReferrer"] = UrlReferrer;
                    return RedirectToAction("Edit",  new { id = fnc_Cost_Sched_Detail.Cost_Sched_Detail_Id, UrlParent = UrlParent });
                    throw;
                }
                TempData["UrlParent"] = UrlParent;
                return Redirect(UrlReferrer);
            }
            ViewBag.UrlReferrer = UrlReferrer;
            ViewBag.Biayas = db.Fnc_Cost_Item.OrderBy(ci => ci.Cost_Item_Name).ToList();
            ViewBag.Cost_Sched_Id = fnc_Cost_Sched_Detail.Cost_Sched_Id;
            return View(fnc_Cost_Sched_Detail);
        }

        // GET: CostSchedDetail/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail = db.Fnc_Cost_Sched_Detail.Find(id);
        //    if (fnc_Cost_Sched_Detail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fnc_Cost_Sched_Detail);
        //}

        // POST: CostSchedDetail/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string UrlParent)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Fnc_Cost_Sched_Detail fnc_Cost_Sched_Detail = db.Fnc_Cost_Sched_Detail.Find(id);
            db.Fnc_Cost_Sched_Detail.Remove(fnc_Cost_Sched_Detail);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                TempData["UrlParent"] = UrlParent;
                return Redirect(UrlReferrer);
            }
            TempData["UrlParent"] = UrlParent;
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
        public JsonResult IsDataExists(int? intCost_Sched_Id, short? shortCost_Item_id)
        {

            if (Request.QueryString["Cost_Sched_Id"].Equals("") || Request.QueryString["Cost_Item_id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intCost_Sched_Id = Convert.ToInt32(Request.QueryString["Cost_Sched_Id"]);
                shortCost_Item_id = Convert.ToInt16(Request.QueryString["Cost_Item_id"]);
                var model = db.Fnc_Cost_Sched_Detail.Where(csd => (intCost_Sched_Id.HasValue) ?
                    (csd.Cost_Sched_Id == intCost_Sched_Id && csd.Cost_Item_id == shortCost_Item_id) :
                    (csd.Cost_Sched_Id == intCost_Sched_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
