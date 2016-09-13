using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
//using PagedList;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace SIA_Universitas.Controllers
{
    public class Student_Khsv2Controller : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Student_Khsv2
        public ActionResult Index(string currentFilter, string searchString, short? Term_Year_Id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Term_Year = Term_Year_Id;

            //dropdown
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id).ToList();

            //List<Vm_Student_KhsV2> list_vm_Student_KhsV2 = new List<Vm_Student_KhsV2>();
            Vm_Student_KhsV2_List Vm_Student_KhsV2_Lists = new Vm_Student_KhsV2_List();

            if (Term_Year_Id != null && !string.IsNullOrEmpty(searchString))
            {
                var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                                && x.Acd_Student.Nim == searchString
                                                                )
                                                        .OrderBy(x => x.Acd_Course.Course_Code)
                                                        .Select(x => new Vm_Student_KhsV2
                                                        {
                                                            Student_Id = x.Student_Id,
                                                            semester = "Semester " + x.Mstr_Term_Year.Mstr_Term.Term_Name +" Tahun Akademik "+ x.Mstr_Term_Year.Mstr_Entry_Year.Entry_Year_Name,
                                                            Krs_Id = x.Krs_Id,
                                                            Nim = x.Acd_Student.Nim,
                                                            Full_Name = x.Acd_Student.Full_Name,
                                                            Course_Code = x.Acd_Course.Course_Code,
                                                            Course_Name = x.Acd_Course.Course_Name,
                                                            Grade_Letter = x.Acd_Student_Khs.FirstOrDefault().Acd_Grade_Letter.Grade_Letter,
                                                            Weight_Value = x.Acd_Student_Khs.FirstOrDefault().Weight_Value,
                                                            Sks = x.Sks,

                                                            Is_For_Transcript = db.Acd_Course_Curriculum.Where(y => y.Department_Id == x.Acd_Student.Department_Id
                                                                                                                && y.Class_Prog_Id == x.Class_Prog_Id
                                                                                                                && y.Course_Id == x.Course_Id
                                                                                                                && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                            && z.Department_Id == x.Acd_Student.Department_Id
                                                                                                                                                                            && z.Class_Prog_Id == x.Acd_Student.Class_Prog_Id
                                                                                                                                                                            && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                    .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                            )
                                                                                                    .Select(y => y.Is_For_Transcript).FirstOrDefault(),

                                                            Transcript_Sks = db.Acd_Course_Curriculum.Where(y => y.Department_Id == x.Acd_Student.Department_Id
                                                                                                                && y.Class_Prog_Id == x.Class_Prog_Id
                                                                                                                && y.Course_Id == x.Course_Id
                                                                                                                && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                                                                                                                                            && z.Department_Id == x.Acd_Student.Department_Id
                                                                                                                                                                            && z.Class_Prog_Id == x.Acd_Student.Class_Prog_Id
                                                                                                                                                                            && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                                                                                                                                    .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                                                                            )
                                                                                                    .Select(y => y.Transcript_Sks).FirstOrDefault()

                                                        });
                //return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
                Vm_Student_KhsV2_Lists.Vm_Student_KhsV2_Lists = acd_Student_Krs.ToList();
                return View(Vm_Student_KhsV2_Lists);
            }
            
            //return View(list_vm_Student_KhsV2.ToPagedList(pageNumber, pageSize));
            return View(Vm_Student_KhsV2_Lists);
        }

        // GET: Student_Khsv2/Edit/5
        public ActionResult Edit(long? Krs_Id)
        {
            if (Krs_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(Krs_Id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }

            short? department_Id = acd_Student_Krs.Acd_Student.Department_Id;
            var listGradeLetter = db.Acd_Grade_Department.Where(x => x.Department_Id == department_Id)
                                                            .Select(x => new
                                                            {
                                                                Grade_Letter_Id = x.Acd_Grade_Letter.Grade_Letter_Id,
                                                                Grade_Letter = x.Acd_Grade_Letter.Grade_Letter
                                                            }).ToList();

            short? grade_Letter_Id = null;
            if (acd_Student_Krs.Acd_Student_Khs.FirstOrDefault() != null)
            {
                grade_Letter_Id = acd_Student_Krs.Acd_Student_Khs.FirstOrDefault().Grade_Letter_Id;
            }

            ViewBag.Grade_Letter_Id = new SelectList(listGradeLetter, "Grade_Letter_Id", "Grade_Letter", grade_Letter_Id);

            return View(acd_Student_Krs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(long? Khs_Id, short? Term_Year_Id, string Nim, short? Department_Id, [Bind(Include = "Khs_Id,Krs_Id,Student_Id,Grade_Letter_Id,Modified_Date,Modified_By,Weight_Value,Is_Required,Is_For_Transkrip,Bnk_Value,Created_Date,Created_By")] Acd_Student_Khs acd_Student_Khs)
        {
            if (Term_Year_Id == null || String.IsNullOrEmpty(Nim) || Department_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            switch (Khs_Id)
            {

                case null:
                    if (ModelState.IsValid)
                    {
                        acd_Student_Khs.Created_Date = DateTime.Now;

                        if (acd_Student_Khs.Grade_Letter_Id == null)
                        {
                            acd_Student_Khs.Weight_Value = null;
                        }
                        else
                        {
                            acd_Student_Khs.Weight_Value = db.Acd_Grade_Department.Where(x => x.Grade_Letter_Id == acd_Student_Khs.Grade_Letter_Id && x.Department_Id == Department_Id)
                                                                                    .Select(x => x.Weight_Value).SingleOrDefault();
                        }

                        db.Acd_Student_Khs.Add(acd_Student_Khs);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Student_Khsv2", new { Term_Year_Id = Term_Year_Id, searchString = Nim });
                    }
                    return RedirectToAction("Edit", "Student_Khsv2", new { Krs_Id = acd_Student_Khs.Krs_Id });

                default:
                    if (ModelState.IsValid)
                    {
                        acd_Student_Khs.Modified_Date = DateTime.Now;

                        if (acd_Student_Khs.Grade_Letter_Id == null)
                        {
                            acd_Student_Khs.Weight_Value = null;
                        }
                        else
                        {
                            acd_Student_Khs.Weight_Value = db.Acd_Grade_Department.Where(x => x.Grade_Letter_Id == acd_Student_Khs.Grade_Letter_Id && x.Department_Id == Department_Id)
                                                                                    .Select(x => x.Weight_Value).SingleOrDefault();
                        }

                        db.Acd_Student_Khs.Attach(acd_Student_Khs); // attach in the Unchanged state
                        db.Entry(acd_Student_Khs).Property(r => r.Modified_Date).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Modified_By).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Grade_Letter_Id).IsModified = true;
                        db.Entry(acd_Student_Khs).Property(r => r.Weight_Value).IsModified = true;

                        //db.Entry(acd_Student_Khs).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Student_Khsv2", new { Term_Year_Id = Term_Year_Id, searchString = Nim });
                    }
                    return RedirectToAction("Edit", "Student_Khsv2", new { Krs_Id = acd_Student_Khs.Krs_Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Export(Vm_Student_KhsV2_List vm_Student_KhsV2_List, decimal bbtXjmlSksSmst, decimal jmlh_sks_bernilai, decimal IP)
        {
            List<Vm_Cetak_Student_KhsV2> printList = new List<Vm_Cetak_Student_KhsV2>();
            foreach (var item in vm_Student_KhsV2_List.Vm_Student_KhsV2_Lists)
            {
                Vm_Cetak_Student_KhsV2 vm_Cetak_Student_KhsV2 = new Vm_Cetak_Student_KhsV2();
                
                vm_Cetak_Student_KhsV2.Nim = item.Nim;
                vm_Cetak_Student_KhsV2.Full_Name = item.Full_Name;
                vm_Cetak_Student_KhsV2.Course_Name = item.Course_Name;
                vm_Cetak_Student_KhsV2.Course_Code = item.Course_Code;
                vm_Cetak_Student_KhsV2.Grade_Letter = item.Grade_Letter;
                vm_Cetak_Student_KhsV2.Weight_Value = item.Weight_Value.ToString();
                vm_Cetak_Student_KhsV2.Sks = item.Sks.ToString();
                vm_Cetak_Student_KhsV2.Is_For_Transcript = item.Is_For_Transcript.ToString();
                vm_Cetak_Student_KhsV2.Transcript_Sks = item.Transcript_Sks.ToString();
                vm_Cetak_Student_KhsV2.mutu = Math.Round(Convert.ToDecimal(item.Sks * item.Weight_Value),2).ToString();
                vm_Cetak_Student_KhsV2.bbtXjmlSksSmst = Math.Round(bbtXjmlSksSmst,2).ToString();
                vm_Cetak_Student_KhsV2.jmlh_sks_bernilai = Math.Round(jmlh_sks_bernilai,2).ToString();
                vm_Cetak_Student_KhsV2.ipSemester = Math.Round(IP,2).ToString();

                vm_Cetak_Student_KhsV2.tanggal = DateTime.Now.ToString("d MMMM yyyy");

                vm_Cetak_Student_KhsV2.semester = item.semester;
                vm_Cetak_Student_KhsV2.Student_Id = item.Student_Id;

                printList.Add(vm_Cetak_Student_KhsV2);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "rpt_KartuHasilStudi.rpt"));
            rd.SetDataSource(printList);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "KartuHasilStudi_" + printList.FirstOrDefault().semester + "_" + printList.FirstOrDefault().Nim + ".pdf");
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                throw;
            }
        }
    }
}