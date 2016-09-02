using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using PagedList;

namespace SIA_Universitas.Controllers
{
    public class Acd_Student_Khsv2Controller : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: Acd_Student_Khsv2
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
            //ViewBag.Department = Department_Id;
            //ViewBag.Term_Year = Term_Year_Id;
            //ViewBag.Class_Prog = Class_Prog_Id;
            //ViewBag.Course_Id = Course_Id;
            //ViewBag.Class_Id = Class_Id;

            var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                                //&& x.Acd_Student.Department_Id == Department_Id
                                                                //&& x.Acd_Student.Class_Prog_Id == Class_Prog_Id
                                                                //&& x.Course_Id == Course_Id
                                                                //&& x.Class_Id == Class_Id
                                                                && x.Acd_Student.Nim.Contains(searchString))
                                                    .OrderBy(x => x.Acd_Student.Nim)
                                                    .Select(x => new Vm_Student_KhsV2
                                                    {
                                                        Course_Id = x.Course_Id,
                                                        Course_Code = x.Acd_Course.Course_Code,
                                                        Course_Name = x.Acd_Course.Course_Name,
                                                        Grade_Letter = x.Acd_Student_Khs.FirstOrDefault().Acd_Grade_Letter.Grade_Letter,
                                                        Weight_Value = x.Acd_Student_Khs.FirstOrDefault().Weight_Value,
                                                        Sks = x.Sks,

                                                        //duo = db.Acd_Course_Curriculum.Where(y => y.Department_Id == Department_Id
                                                        //                                 && y.Class_Prog_Id == Class_Prog_Id
                                                        //                                 && y.Course_Id == Course_Id
                                                        //                                 && y.Curriculum_Id == db.Acd_Curriculum_Entry_Year.Where(z => z.Term_Year_Id == Term_Year_Id
                                                        //                                                                                             && z.Department_Id == Department_Id
                                                        //                                                                                             && z.Class_Prog_Id == Class_Prog_Id
                                                        //                                                                                             && z.Entry_Year_Id == x.Acd_Student.Entry_Year_Id)
                                                        //                                                                                     .Select(z => z.Curriculum_Id).FirstOrDefault()
                                                        //                              )
                                                        //                         .Select(y => new 
                                                        //                         {
                                                        //                            Is_For_Transcript = y.Is_For_Transcript.ToString(),
                                                        //                            Transcript_Sks = y.Transcript_Sks.ToString()
                                                        //                         }).ToArray()

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

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    acd_Student_Krs = acd_Student_Krs.Where(x => x.Nim.Contains(searchString) || x.Full_Name.Contains(searchString));
            //}

            return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
        }
    }
}