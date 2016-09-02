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
using System.Data.Entity.Infrastructure;
using System.Web.Configuration;

namespace SIA_Universitas.Controllers
{
    public class SchedSessionController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: SchedSession
        public ActionResult Index(short? Term_Year_Id, short? Sched_Type_Id)
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
            short schedTypeId = Sched_Type_Id ?? 0;

            if (Term_Year_Id != null && Sched_Type_Id != null)
            {
                ViewBag.TermYearId = Term_Year_Id;
                ViewBag.SchedTypeId = Sched_Type_Id;
            }

            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id);
            ViewBag.Sched_Type_Id = new SelectList(db.Mstr_Sched_Type, "Sched_Type_Id", "Sched_Type_Name", Sched_Type_Id);

            using (var context = new ConDataContext())
            using (var cmd = context.Connection.CreateCommand())
            {
                context.Connection.Open();
                cmd.CommandText = " exec SP_Crosstab2" +
                                  " 'SELECT dayId = d.Day_Id, Hari = d.Day_Name," +
                                          " sesi = ss.Order_Id," +
                                          " jam = cast(ss.Sched_Session_Id as Varchar(20)) +''|''+ ss.Time_Start +'' - ''+ ss.Time_End" +
                                   " FROM Acd_Sched_Session ss" +
                                        " INNER JOIN Mstr_Term_Year ty ON ty.Term_Year_Id = ss.Term_Year_Id" +
                                        " INNER JOIN Mstr_Sched_Type st ON st.Sched_Type_Id = ss.Sched_Type_Id" +
                                        " INNER JOIN Mstr_Day d ON d.Day_Id = ss.Day_Id" +
                                        " LEFT JOIN Mstr_Class_Program cp ON cp.Class_Prog_Id = ss.Class_Prog_Id" +
                                   " WHERE ty.Term_Year_Id =  " + termYearId + " AND st.Sched_Type_Id = " + schedTypeId + "'," +
                                  " 'dayId smallint, Hari Varchar(100), sesi Varchar(100), jam Varchar(100)'," +
                                  " 'sesi'," +
                                  " 'MAX(jam)'," +
                                  " ''," +
                                  " 'ASC'," +
                                  " 'dayId ASC'";
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

        // GET: SchedSession/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Sched_Session acd_Sched_Session = db.Acd_Sched_Session.Find(id);
            if (acd_Sched_Session == null)
            {
                return HttpNotFound();
            }
            return View(acd_Sched_Session);
        }

        // GET: SchedSession/Create
        public ActionResult Create(short? Term_Year_Id, short? Sched_Type_Id, short? Day_Id, short? Order_Id)
        {
            ViewBag.termYear = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == Term_Year_Id).FirstOrDefault();
            ViewBag.schedType = db.Mstr_Sched_Type.Where(st => st.Sched_Type_Id == Sched_Type_Id).FirstOrDefault();
            if (Day_Id != null && Order_Id != null)
            {
                ViewBag.Day = db.Mstr_Day.Where(d => d.Day_Id == Day_Id).FirstOrDefault();
                ViewBag.Order = Order_Id;
            }
            else
            {
                ViewBag.Day_Id = db.Mstr_Day.ToList();
                short JmlMaxSesi = Convert.ToInt16(WebConfigurationManager.AppSettings["JmlMaxSesi"]);
                List<CustomChoose> order_id = new List<CustomChoose>();
                for (short i = 1; i <= JmlMaxSesi; i++)
                {
                    order_id.Add(new CustomChoose { 
                        Id=i,
                        Value=i.ToString()
                    });
                }
                ViewBag.Order_Id = order_id.ToList();
            }
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            var clasProg = new List<short> { 1, 2, 3 }; 
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => clasProg.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name");
            return View();
        }

        // POST: SchedSession/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sched_Session_Id,Sched_Type_Id,Term_Year_Id,Day_Id,Class_Prog_Id,Order_Id,Time_Start,Time_End,Description,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Sched_Session acd_Sched_Session, string UrlReferrer, int? param)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Sched_Session.Add(acd_Sched_Session);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            ViewBag.termYear = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Sched_Session.Term_Year_Id).FirstOrDefault();
            ViewBag.schedType = db.Mstr_Sched_Type.Where(st => st.Sched_Type_Id == acd_Sched_Session.Sched_Type_Id).FirstOrDefault();
            if (param != null)
            {
                ViewBag.Day = db.Mstr_Day.Where(d => d.Day_Id == acd_Sched_Session.Day_Id).FirstOrDefault();
                ViewBag.Order = acd_Sched_Session.Order_Id;
            }
            else
            {
                ViewBag.Day_Id = db.Mstr_Day.ToList();
                short JmlMaxSesi = Convert.ToInt16(WebConfigurationManager.AppSettings["JmlMaxSesi"]);
                List<CustomChoose> order_id = new List<CustomChoose>();
                for (short i = 1; i <= JmlMaxSesi; i++)
                {
                    order_id.Add(new CustomChoose
                    {
                        Id = i,
                        Value = i.ToString()
                    });
                }
                ViewBag.Order_Id = order_id.ToList();
            }
            ViewBag.UrlReferrer = UrlReferrer;

            var clasProg = new List<short> { 1, 2, 3 };
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => clasProg.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name", acd_Sched_Session.Class_Prog_Id);
            return View(acd_Sched_Session);
        }

        // GET: SchedSession/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Sched_Session acd_Sched_Session = db.Acd_Sched_Session.Find(id);
            if (acd_Sched_Session == null)
            {
                return HttpNotFound();
            }
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            var clasProg = new List<short> { 1, 2, 3 };
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => clasProg.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name", acd_Sched_Session.Class_Prog_Id);
            ViewBag.Day = db.Mstr_Day.Where(d => d.Day_Id == acd_Sched_Session.Day_Id).FirstOrDefault();
            ViewBag.termYear = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Sched_Session.Term_Year_Id).FirstOrDefault();
            ViewBag.schedType = db.Mstr_Sched_Type.Where(st => st.Sched_Type_Id == acd_Sched_Session.Sched_Type_Id).FirstOrDefault();
            return View(acd_Sched_Session);
        }

        // POST: SchedSession/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sched_Session_Id,Sched_Type_Id,Term_Year_Id,Day_Id,Class_Prog_Id,Order_Id,Time_Start,Time_End,Description,Created_By,Created_Date,Modified_By,Modified_Date")] Acd_Sched_Session acd_Sched_Session, string UrlReferrer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Sched_Session).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }
            ViewBag.UrlReferrer = UrlReferrer;
            var clasProg = new List<short> { 1, 2, 3 };
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program.Where(cp => clasProg.Contains(cp.Class_Prog_Id)), "Class_Prog_Id", "Class_Program_Name", acd_Sched_Session.Class_Prog_Id);
            ViewBag.Day = db.Mstr_Day.Where(d => d.Day_Id == acd_Sched_Session.Day_Id).FirstOrDefault();
            ViewBag.termYear = db.Mstr_Term_Year.Where(ty => ty.Term_Year_Id == acd_Sched_Session.Term_Year_Id).FirstOrDefault();
            ViewBag.schedType = db.Mstr_Sched_Type.Where(st => st.Sched_Type_Id == acd_Sched_Session.Sched_Type_Id).FirstOrDefault();
            return View(acd_Sched_Session);
        }

        // GET: SchedSession/Delete/5
        //public ActionResult Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Acd_Sched_Session acd_Sched_Session = db.Acd_Sched_Session.Find(id);
        //    if (acd_Sched_Session == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(acd_Sched_Session);
        //}

        // POST: SchedSession/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            string UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Acd_Sched_Session acd_Sched_Session = db.Acd_Sched_Session.Find(id);
            db.Acd_Sched_Session.Remove(acd_Sched_Session);
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
        public JsonResult IsDataExists(short? intSched_Type_Id, short? intTerm_Year_Id, short? intDay_Id, short? intOrder_Id)
        {
            if (Request.QueryString["Sched_Type_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Day_Id"].Equals("") || Request.QueryString["Order_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intSched_Type_Id = Convert.ToInt16(Request.QueryString["Sched_Type_Id"]);
                intTerm_Year_Id = Convert.ToInt16(Request.QueryString["Term_Year_Id"]);
                intDay_Id = Convert.ToInt16(Request.QueryString["Day_Id"]);
                intOrder_Id = Convert.ToInt16(Request.QueryString["Order_Id"]);
                var model = db.Acd_Sched_Session.Where(ss => (intTerm_Year_Id.HasValue) ?
                    (ss.Sched_Type_Id == intSched_Type_Id && ss.Term_Year_Id == intTerm_Year_Id && ss.Day_Id == intDay_Id && ss.Order_Id == intOrder_Id) :
                    (ss.Order_Id == intOrder_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
