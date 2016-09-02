using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using System.Data.Entity.Infrastructure;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using EntityFramework.Extensions;

namespace SIA_Universitas.Controllers
{
    public class CourseController : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Course
        public ActionResult Index(short? Department_Id, short? currentDept, string currentFilter, string searchString, int? page, int? rowPerPage)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (Department_Id != null)
            {
                page = 1;
            }
            else
            {
                Department_Id = currentDept;
            }

            if (TempData["gagalHapus"] != null)
            {
                ViewBag.gagalHapus = TempData["gagalHapus"].ToString();
            }
            if (TempData["berhasilHapus"] != null)
            {
                ViewBag.berhasilHapus = TempData["berhasilHapus"].ToString();
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentDept = Department_Id;

            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id);

            var acd_Course = db.Acd_Course.Where(a => a.Department_Id == Department_Id).Include(a => a.Acd_Course_Type).Include(a => a.Mstr_Department);
            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Course = acd_Course.Where(s => s.Course_Name.Contains(searchString) || s.Course_Name_Eng.Contains(searchString) || s.Course_Code.Contains(searchString));
            }
            acd_Course = acd_Course.OrderBy(s => s.Course_Name);

            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : (rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage;
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            return View(acd_Course.ToPagedList(pageNumber, pageSize));
        }

        // GET: Course/Report/department_id
        public ActionResult Report(short DeptId, string rpt)
        {
            var table = new DataTable();
            List<Acd_Course> acd_Course = db.Acd_Course.Where(a => a.Department_Id == DeptId).Include(a => a.Acd_Course_Type).Include(a => a.Mstr_Department).ToList();
            int jmlData = acd_Course.Count();

            table.Columns.Add("No", typeof(int));
            table.Columns.Add("Course_Code", typeof(string));
            table.Columns.Add("Course_Name", typeof(string));
            table.Columns.Add("Course_Name_Eng", typeof(string));
            table.Columns.Add("Course_Type_Name", typeof(string));
            table.Columns.Add("Department_Acronym", typeof(string));

            int no = 1;
            foreach (var item in acd_Course)
            {
                table.Rows.Add(no, item.Course_Code, item.Course_Name, item.Course_Name_Eng, item.Acd_Course_Type.Course_Type_Name, item.Mstr_Department.Department_Acronym);
                no++;
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("/Report/rptMatakuliah.rpt");
            rptH.Load();
            rptH.SetDataSource(table);
            if (rpt == "xls")
            {
                var grid = new GridView();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Matakuliah.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.DataSource = table;
                grid.DataBind();
                grid.RenderControl(htw);

                var prodi = db.Mstr_Department.Where(d => d.Department_Id == DeptId).FirstOrDefault();
                DateTime tglCetak = DateTime.Now;
                string headerTable = @"<table><tr><td align=center colspan=6><b>Daftar Matakuliah</b></td></tr><tr><td align=center colspan=6><b>Program Studi " + prodi.Department_Name + "</b></td></tr><tr><td colspan=3>Jumlah Data: <b>" + jmlData + "</b></td><td colspan=3 align=right>Tanggal Cetak: <b>" + tglCetak.ToString("dddd, dd MMMM yyyy") + "</b></td></tr></table>";
                Response.Output.Write(headerTable);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", new { Department_Id = DeptId });
            }
            else if (rpt == "pdf")
            {
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
            else
            {
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                return File(stream, "application/doc", "Matakuliah.doc");
            }
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Course acd_Course = db.Acd_Course.Find(id);
            if (acd_Course == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course);
        }

        // GET: Course/Create
        public ActionResult Create(short id)
        {

            ViewBag.Course_Type_Id = new SelectList(db.Acd_Course_Type, "Course_Type_Id", "Course_Type_Name");
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == id).FirstOrDefault();

            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Course_Id,Department_Id,Course_Type_Id,Course_Code,Course_Name,Course_Name_Eng,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course acd_Course)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Course.Add(acd_Course);
                db.SaveChanges();

                ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", acd_Course.Department_Id);
                
                return RedirectToAction("Index", new { Department_Id = acd_Course.Department_Id });
            }

            ViewBag.Course_Type_Id = new SelectList(db.Acd_Course_Type, "Course_Type_Id", "Course_Type_Name", acd_Course.Course_Type_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Course.Department_Id).FirstOrDefault();
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department, "Department_Id", "Department_Code", acd_Course.Department_Id);
            return View(acd_Course);
        }

        // GET: Course/Edit/5
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
            Acd_Course acd_Course = db.Acd_Course.Find(id);
            if (acd_Course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Type_Id = new SelectList(db.Acd_Course_Type, "Course_Type_Id", "Course_Type_Name", acd_Course.Course_Type_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Course.Department_Id).FirstOrDefault();
            return View(acd_Course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Course_Id,Department_Id,Course_Type_Id,Course_Code,Course_Name,Course_Name_Eng,Created_By,Created_Date,Modified_By,Modified_Date,Order_Id")] Acd_Course acd_Course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Course).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Acd_Course course = db.Acd_Course.Find(acd_Course.Course_Id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["shortMessage"] = "Data ganda, data matakuliah yang akan di tambahkan telah ada.";
                    return RedirectToAction("Edit", acd_Course);
                    throw;
                }
                ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", acd_Course.Department_Id);
                return RedirectToAction("Index", new { Department_Id = acd_Course.Department_Id });
            }
            ViewBag.Course_Type_Id = new SelectList(db.Acd_Course_Type, "Course_Type_Id", "Course_Type_Name", acd_Course.Course_Type_Id);
            ViewBag.Department = db.Mstr_Department.Where(d => d.Department_Id == acd_Course.Department_Id).FirstOrDefault();
            return View(acd_Course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Acd_Course acd_Course = db.Acd_Course.Find(id);
            if (acd_Course == null)
            {
                return HttpNotFound();
            }
            return View(acd_Course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Acd_Course acd_Course = db.Acd_Course.Find(id);
            try
            {
                db.Acd_Course.Where(c => c.Course_Id.Equals(id)).Delete();
            }
            catch (Exception)
            {
                TempData["gagalHapus"] = "Gagal Hapus, Data sudah digunakan";
                return RedirectToAction("Index", new { Department_Id = acd_Course.Department_Id });
            }
            //ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", acd_Course.Department_Id);
            TempData["berhasilHapus"] = "Berhasil Hapus Data.";
            return RedirectToAction("Index", new { Department_Id = acd_Course.Department_Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[AllowAnonymous]
        //public JsonResult IsCourseCodeExists(string strCourseCode)
        //{
        //    strCourseCode = Request.QueryString["Course_Code"];
        //    return Json(!db.Acd_Course.Any(c => c.Course_Code == strCourseCode), JsonRequestBehavior.AllowGet);
        //}

        [AllowAnonymous]
        public JsonResult IsCourse_CodeExists(int? intDepartment_Id, string strCourse_Code)
        {
            strCourse_Code = Request.QueryString["Course_Code"];
            intDepartment_Id = Convert.ToInt32(Request.QueryString["Department_Id"]);

            var model = db.Acd_Course.Where(acd=>(intDepartment_Id.HasValue)?
                (acd.Department_Id == intDepartment_Id && acd.Course_Code == strCourse_Code) :
                (acd.Course_Code == strCourse_Code )
            );

            return Json(model.Count() == 0, JsonRequestBehavior.AllowGet);
        }
    }
}
