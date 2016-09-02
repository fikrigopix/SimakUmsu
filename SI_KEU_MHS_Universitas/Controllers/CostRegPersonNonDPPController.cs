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

namespace SI_KEU_MHS_Universitas.Controllers
{
    public class CostRegPersonNonDPPController : Controller
    {
        private SIKEUEntities db = new SIKEUEntities();

        // GET: CostRegPersonNonDPP
        public ActionResult Index(string param)
        {
            ViewBag.param = param;

            //long? camaruId = db.Acd_Student.Where(s => s.Nim.Equals(param)).Select(s => s.Register_Id).FirstOrDefault();
            long? camaruId = db.Acd_Student.Where(s => s.Nim.Equals(param)).Select(s => s.Register_Id).FirstOrDefault();

            if (camaruId == null)
            {
                camaruId = db.Reg_Camaru.Where(c => c.Camaru_Code.Equals(param)).Select(s => s.Camaru_Id).FirstOrDefault();
            }

            List<Fnc_Cost_Regular_Personal> fnc_Cost_Regular_Personal = new List<Fnc_Cost_Regular_Personal>();
            if (camaruId == 0)
            {
                ViewBag.message = "NIM/No.Reg yang anda masukan tidak Valid.";
            }
            else
            {
                fnc_Cost_Regular_Personal = db.Fnc_Cost_Regular_Personal.Where(crp => crp.Camaru_Id == camaruId).ToList();
            }
            ViewBag.student = db.Acd_Student.Where(s => s.Register_Id == camaruId).FirstOrDefault();

            return View(fnc_Cost_Regular_Personal);
        }

        // GET: CostRegPersonNonDPP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal = db.Fnc_Cost_Regular_Personal.Find(id);
            if (fnc_Cost_Regular_Personal == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Cost_Regular_Personal);
        }

        // GET: CostRegPersonNonDPP/Create
        public ActionResult Create(long Camaru_Id)
        {
            ViewBag.student = db.Acd_Student.Where(s => s.Register_Id == Camaru_Id).First();

            ViewBag.Bill_Type = db.Fnc_Bill_Type.OrderBy(bt=>bt.Bill_Type_Name).ToList();
            ViewBag.Cost_Item = db.Fnc_Cost_Item.OrderBy(ci=>ci.Cost_Item_Name).ToList();
            ViewBag.Term_Year = db.Mstr_Term_Year.OrderByDescending(ty=>ty.Term_Year_Id).ToList();
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            //ViewBag.Camaru_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code");
            return View();
        }

        // POST: CostRegPersonNonDPP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cost_Regular_Personal_Id,Camaru_Id,Term_Year_Id,Cost_Item_Id,Bill_Type_Id,Amount,Percentage,Due_Date,Description,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal, string SAmount, string UrlReferrer)
        {
            fnc_Cost_Regular_Personal.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Fnc_Cost_Regular_Personal.Add(fnc_Cost_Regular_Personal);
                db.SaveChanges();
                return Redirect(UrlReferrer);
            }

            ViewBag.Bill_Type_Id = new SelectList(db.Fnc_Bill_Type, "Bill_Type_Id", "Bill_Type_Name", fnc_Cost_Regular_Personal.Bill_Type_Id);
            ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item, "Cost_Item_Id", "Cost_Item_Code", fnc_Cost_Regular_Personal.Cost_Item_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", fnc_Cost_Regular_Personal.Term_Year_Id);
            ViewBag.Camaru_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", fnc_Cost_Regular_Personal.Camaru_Id);
            return View(fnc_Cost_Regular_Personal);
        }

        // GET: CostRegPersonNonDPP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TempData["shortMessage"] != null)
            {
                ViewBag.message = TempData["shortMessage"].ToString();
            }
            Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal = db.Fnc_Cost_Regular_Personal.Find(id);
            if (fnc_Cost_Regular_Personal == null)
            {
                return HttpNotFound();
            }
            ViewBag.student = db.Acd_Student.Where(s => s.Register_Id == fnc_Cost_Regular_Personal.Camaru_Id).First();
            ViewBag.Bill_Type_Id = new SelectList(db.Fnc_Bill_Type, "Bill_Type_Id", "Bill_Type_Name", fnc_Cost_Regular_Personal.Bill_Type_Id);
            ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item.OrderBy(ci => ci.Cost_Item_Name), "Cost_Item_Id", "Cost_Item_Name", fnc_Cost_Regular_Personal.Cost_Item_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", fnc_Cost_Regular_Personal.Term_Year_Id);
            ViewBag.Camaru_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", fnc_Cost_Regular_Personal.Camaru_Id);
            ViewBag.UrlReferrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return View(fnc_Cost_Regular_Personal);
        }

        // POST: CostRegPersonNonDPP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cost_Regular_Personal_Id,Camaru_Id,Term_Year_Id,Cost_Item_Id,Bill_Type_Id,Amount,Percentage,Due_Date,Description,Created_By,Created_Date,Modified_By,Modified_Date")] Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal, string SAmount, string UrlReferrer)
        {
            fnc_Cost_Regular_Personal.Amount = (int)decimal.Parse(Regex.Replace(SAmount, @"[^\d.]", ""));

            if (ModelState.IsValid)
            {
                db.Entry(fnc_Cost_Regular_Personal).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Fnc_Cost_Regular_Personal CostRegularPersonal = db.Fnc_Cost_Regular_Personal.Find(fnc_Cost_Regular_Personal.Cost_Regular_Personal_Id);
                    if (CostRegularPersonal == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Kombinasi Tahun Semester, Item Pembayaran & Jenis Dispensasi telah digunakan.";
                    return RedirectToAction("Edit", fnc_Cost_Regular_Personal);
                    throw;
                }
                return Redirect(UrlReferrer);
            }
            //ViewBag.Bill_Type_Id = new SelectList(db.Fnc_Bill_Type, "Bill_Type_Id", "Bill_Type_Name", fnc_Cost_Regular_Personal.Bill_Type_Id);
            //ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item, "Cost_Item_Id", "Cost_Item_Code", fnc_Cost_Regular_Personal.Cost_Item_Id);
            //ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year, "Term_Year_Id", "Term_Year_Name", fnc_Cost_Regular_Personal.Term_Year_Id);
            //ViewBag.Camaru_Id = new SelectList(db.Reg_Camaru, "Camaru_Id", "Camaru_Code", fnc_Cost_Regular_Personal.Camaru_Id);
            return View(fnc_Cost_Regular_Personal);
        }

        // GET: CostRegPersonNonDPP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal = db.Fnc_Cost_Regular_Personal.Find(id);
            if (fnc_Cost_Regular_Personal == null)
            {
                return HttpNotFound();
            }
            return View(fnc_Cost_Regular_Personal);
        }

        // POST: CostRegPersonNonDPP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fnc_Cost_Regular_Personal fnc_Cost_Regular_Personal = db.Fnc_Cost_Regular_Personal.Find(id);
            db.Fnc_Cost_Regular_Personal.Remove(fnc_Cost_Regular_Personal);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public JsonResult IsDataExists(long? intCamaru_Id, short? intTerm_Year_Id, short? intCost_Item_Id)
        {

            if (Request.QueryString["Camaru_Id"].Equals("") || Request.QueryString["Term_Year_Id"].Equals("") || Request.QueryString["Cost_Item_Id"].Equals(""))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                intCamaru_Id = Convert.ToInt64(Request.QueryString["Camaru_Id"]);
                intTerm_Year_Id = Convert.ToInt16(Request.QueryString["Term_Year_Id"]);
                intCost_Item_Id = Convert.ToInt16(Request.QueryString["Cost_Item_Id"]);
                var model = db.Fnc_Cost_Regular_Personal.Where(crp => (intCost_Item_Id.HasValue) ?
                    (crp.Camaru_Id == intCamaru_Id && crp.Term_Year_Id == intTerm_Year_Id &&
                     crp.Cost_Item_Id == intCost_Item_Id) :
                    (crp.Cost_Item_Id == intCost_Item_Id)
                );
                return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
