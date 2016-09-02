using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIA_Universitas.Models;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace SIA_Universitas.Controllers
{
    public class StudentKrsV1Controller : Controller
    {
        private SIAEntities db = new SIAEntities();

        // GET: StudentKrsV1
        public ActionResult Index(string currentFilter, string searchString, short? Term_Year_Id, short? Department_Id, short? Class_Prog_Id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Department = Department_Id;
            ViewBag.Term_Year = Term_Year_Id;
            ViewBag.Class_Prog = Class_Prog_Id;

            //dropdown
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", Term_Year_Id).ToList();
            ViewBag.Department_Id = new SelectList(db.Mstr_Department.OrderBy(d => d.Department_Code), "Department_Id", "Department_Name", Department_Id).ToList();
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Department_Class_Program.Where(dcp => dcp.Department_Id == Department_Id), "Class_Prog_Id", "Mstr_Class_Program.Class_Program_Name", Class_Prog_Id).ToList();

            Term_Year_Id = Term_Year_Id ?? 0;
            Department_Id = Department_Id ?? 0;
            Class_Prog_Id = Class_Prog_Id ?? 0;

            string query = "select b.Course_Id, c.Class_Id, " +
                            " b.Course_Code, b.Course_Name, c.Class_Name, a.Class_Capacity, COUNT(d.Student_Id) as jmlPeserta, a.Offered_Course_id" +
                            " from Acd_Offered_Course a" +
                            " inner join Acd_Course b on b.Course_Id = a.Course_Id" +
                            " inner join Mstr_Class c on c.Class_Id = a.Class_Id" +

                            " left join Acd_Student_Krs d on d.Term_Year_Id = a.Term_Year_Id and d.Class_Prog_Id = a.Class_Prog_Id and d.Course_Id = a.Course_Id and d.Class_Id = a.Class_Id" +
                            " left join Acd_Student e on e.Student_Id = d.Student_Id and e.Department_Id = a.Department_Id" +

                            " where" +
                            " a.Term_Year_Id = {0}" +
                            " and a.Department_Id = {1}" +
                            " and a.Class_Prog_Id = {2}" +
                            " group by b.Course_Id, c.Class_Id, b.Course_Code, b.Course_Name, c.Class_Name, a.Class_Capacity, a.Offered_Course_id" +
                            " order by b.Course_Code, c.Class_Name";

            IEnumerable<Vm_Student_KrsV1> vm_Student_KrsV1 = db.Database.SqlQuery<Vm_Student_KrsV1>(query, Term_Year_Id, Department_Id, Class_Prog_Id);

            if (!string.IsNullOrEmpty(searchString))
            {
                vm_Student_KrsV1 = vm_Student_KrsV1.Where(x => x.Course_Code.Contains(searchString) || x.Course_Name.Contains(searchString));
            }

            return View(vm_Student_KrsV1.ToPagedList(pageNumber, pageSize));

            //IQueryable<Vm_Student_KrsV1> acd_Student_Krs = from a in db.Acd_Offered_Course
            //                                               join b in db.Acd_Course on a.Course_Id equals b.Course_Id
            //                                               join c in db.Mstr_Class on a.Class_Id equals c.Class_Id

            //                                               //join d in db.Acd_Student_Krs on a.Term_Year_Id equals d.Term_Year_Id into ask
            //                                               //from dx in ask.DefaultIfEmpty()

            //                                               from d in db.Acd_Student_Krs.Where(x => x.Term_Year_Id == a.Term_Year_Id && x.Class_Prog_Id == a.Class_Prog_Id && x.Course_Id == a.Course_Id && x.Class_Id == a.Class_Id)
            //                                                                           .DefaultIfEmpty()

            //                                               //join e in db.Acd_Student on dx.Student_Id equals e.Student_Id into ast
            //                                               //from ex in ast.DefaultIfEmpty()

            //                                               from e in db.Acd_Student.Where(x => x.Student_Id == d.Student_Id && x.Department_Id == a.Department_Id)
            //                                                                           .DefaultIfEmpty()

            //                                               where a.Term_Year_Id == Term_Year_Id
            //                                                   && a.Department_Id == Department_Id
            //                                                   && a.Class_Prog_Id == Class_Prog_Id

            //                                               //&& dx.Class_Prog_Id == a.Class_Prog_Id
            //                                               //&& dx.Course_Id == a.Course_Id
            //                                               //&& dx.Class_Id == a.Class_Id

            //                                               //&& ex.Department_Id == a.Department_Id

            //                                               group new { b, c, a, d } by new { b.Course_Code, b.Course_Name, c.Class_Name, a.Class_Capacity } into grp
            //                                               //group new { b, c, a, d } by b.Course_Code into grp
            //                                               orderby grp.FirstOrDefault().b.Course_Code, grp.FirstOrDefault().c.Class_Name
            //                                               select new Vm_Student_KrsV1
            //                                               {
            //                                                   Course_Code = grp.FirstOrDefault().b.Course_Code,
            //                                                   Course_Name = grp.FirstOrDefault().b.Course_Name,
            //                                                   Class_Name = grp.FirstOrDefault().c.Class_Name,
            //                                                   Class_Capacity = grp.FirstOrDefault().a.Class_Capacity,
            //                                                   //Jml_Peserta = grp.Select(x => x.dx.Student_Id).DefaultIfEmpty().Count()
            //                                                   Jml_Peserta = grp.DefaultIfEmpty().Select(x => x.d.Student_Id).Count()
            //                                               };

            //return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
        }

        // GET: StudentKrsV1/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }
            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV1/List
        public ActionResult List(string currentFilter, string searchString, short Term_Year_Id, short Class_Prog_Id, int Course_Id, short Department_Id, short Class_Id, int Offered_Course_id, int? page, int? rowPerPage)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);
            //alert
            if (TempData["massage_success"] != null)
            {
                ViewBag.massage_success = TempData["massage_success"].ToString();
            }

            //for url
            ViewBag.searchString = searchString;
            ViewBag.Term_Year = Term_Year_Id;
            ViewBag.Class_Prog = Class_Prog_Id;
            ViewBag.Course = Course_Id;
            ViewBag.Class = Class_Id;
            ViewBag.Department = Department_Id;
            ViewBag.Offered_Course = Offered_Course_id;

            ViewBag.nav = db.Acd_Offered_Course.Where(x => x.Offered_Course_id == Offered_Course_id)
                                                //.Select(x => new 
                                                //{
                                                //    Term_Year_Name = x.Mstr_Term_Year.Term_Year_Name,
                                                //    Department_Name = x.Mstr_Department.Department_Name,
                                                //    Class_Program_Name = x.Mstr_Class_Program.Class_Program_Name,
                                                //    Course_Name = x.Acd_Course.Course_Name,
                                                //    Class_Name = x.Mstr_Class.Class_Name
                                                //})
                                                .First();

            var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id 
                                                            && x.Class_Prog_Id == Class_Prog_Id 
                                                            && x.Course_Id == Course_Id 
                                                            && x.Class_Id == Class_Id);

            ViewBag.nav_count_data = acd_Student_Krs.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student_Krs = acd_Student_Krs.Where(x => x.Acd_Student.Nim.Contains(searchString) || x.Acd_Student.Full_Name.Contains(searchString));
            }

            acd_Student_Krs = acd_Student_Krs.OrderBy(x => x.Acd_Student.Nim);

            return View(acd_Student_Krs.ToPagedList(pageNumber, pageSize));
        }

        // GET: StudentKrsV1/ListAdd
        public ActionResult ListAdd(string currentFilter, string searchString, short Term_Year_Id, short Class_Prog_Id, short Department_Id, int Course_Id, short Class_Id, int Offered_Course_id, short? Entry_Year_Id, int? page, int? rowPerPage, string alert)
        {
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }
            Session["rowPerPage"] = (Session["rowPerPage"] == null) ? 10 : ((rowPerPage == null || rowPerPage < 1) ? Session["rowPerPage"] : rowPerPage);
            int pageSize = Convert.ToInt32(Session["rowPerPage"]);
            ViewBag.rowPerPage = pageSize;
            int pageNumber = (page == null || page < 1) ? 1 : Convert.ToInt32(page);

            string massage = string.Empty;
            string detailMassage = string.Empty;
            //alert
            if (TempData["massage"] != null)
            {
                ViewBag.massage = TempData["massage"].ToString();
            }
            if (TempData["detailMassage"] != null)
            {
                ViewBag.detailMassage = TempData["detailMassage"];
            }
            if (TempData["detailMassage1"] != null)
            {
                ViewBag.detailMassage1 = TempData["detailMassage1"];
            }
            
            //for url
            ViewBag.searchString = searchString;
            ViewBag.Term_Year = Term_Year_Id;
            ViewBag.Class_Prog = Class_Prog_Id;
            ViewBag.Department = Department_Id;
            ViewBag.Course = Course_Id;
            ViewBag.Class = Class_Id;
            ViewBag.Offered_Course = Offered_Course_id;
            ViewBag.Entry_Year = Entry_Year_Id;

            ViewBag.nav = db.Acd_Offered_Course.Where(x => x.Offered_Course_id == Offered_Course_id)
                                            //.Select(x => new
                                            //{
                                            //    Term_Year_Name = x.Mstr_Term_Year.Term_Year_Name,
                                            //    Department_Name = x.Mstr_Department.Department_Name,
                                            //    Class_Program_Name = x.Mstr_Class_Program.Class_Program_Name,
                                            //    Course_Name = x.Acd_Course.Course_Name,
                                            //    Class_Name = x.Mstr_Class.Class_Name,
                                            //    Class_Capacity = x.Class_Capacity
                                            //})
                                                .First();

            ViewBag.nav_count_data = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                            && x.Class_Prog_Id == Class_Prog_Id
                                                            && x.Course_Id == Course_Id
                                                            && x.Class_Id == Class_Id)
                                                        .Select(x => x.Student_Id)
                                                        .Count();

            ViewBag.nav1 = db.usp_GetCourseCostForKRS(Term_Year_Id, Department_Id, Class_Prog_Id, Entry_Year_Id, Course_Id).FirstOrDefault();
            //var a = nav1.FirstOrDefault(x=> x.)
            //dropdown
            ViewBag.Entry_Year_Id = new SelectList(db.Mstr_Entry_Year
                                                    .OrderByDescending(ey => ey.Entry_Year_Id), "Entry_Year_Id", "Entry_Year_Code", Entry_Year_Id).ToList();

            var mhs_out = db.Acd_Student_Out.Where(x => x.Acd_Student.Department_Id == Department_Id 
                                                    && x.Acd_Student.Class_Prog_Id == Class_Prog_Id 
                                                    && x.Acd_Student.Entry_Year_Id == Entry_Year_Id)
                                            .OrderBy(x => x.Student_Id)
                                            .Select(x => x.Student_Id);

            var mhs_krsInCourse = db.Acd_Student_Krs
                                            .Where(x => x.Term_Year_Id == Term_Year_Id && x.Class_Prog_Id == Class_Prog_Id && x.Course_Id == Course_Id)
                                            .Select(x => x.Student_Id);

            var acd_Student = db.Acd_Student.Where(x => x.Department_Id == Department_Id
                                                    && x.Entry_Year_Id == Entry_Year_Id
                                                    && x.Class_Prog_Id == Class_Prog_Id
                                                    && !mhs_out.Contains(x.Student_Id)
                                                    && !mhs_krsInCourse.Contains(x.Student_Id));

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student = acd_Student.Where(x => x.Nim.Contains(searchString) || x.Full_Name.Contains(searchString));
            }

            acd_Student = acd_Student.OrderBy(x => x.Nim);

            return View(acd_Student.ToPagedList(pageNumber, pageSize));
        }

        // POST: StudentKrsV1/ListAdd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListAdd(IEnumerable<long> checkStudent_IdToAdd, short Department_Id, int Offered_Course_id, short Entry_Year_Id, Acd_Student_Krs acd_Student_Krs)
        {
            bool errorSave = false;

            if (ModelState.IsValid)
            {
                if (checkStudent_IdToAdd == null)
                {
                    TempData["massage"] = "Pilih data terlebih dahulu!";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                if (validKouta(Offered_Course_id, acd_Student_Krs.Term_Year_Id, acd_Student_Krs.Class_Prog_Id, acd_Student_Krs.Course_Id, acd_Student_Krs.Class_Id, checkStudent_IdToAdd) == false)
                {
                    TempData["massage"] = "Kuota kelas tidak cukup!";
                    return Redirect(Request.UrlReferrer.ToString());
                }

                var getCourseCostForKRS = db.usp_GetCourseCostForKRS(acd_Student_Krs.Term_Year_Id, Department_Id, acd_Student_Krs.Class_Prog_Id, Entry_Year_Id, acd_Student_Krs.Course_Id)
                                     .Select(x => new
                                     {
                                         x.amount,
                                         x.applied_sks
                                     }).First();

                try
                {
                    int jmlSkipLoop = 0;
                    List<Vm_notif_error> listIdError = new List<Vm_notif_error>();
                    foreach (var i in checkStudent_IdToAdd)
                    {
                        bool errorSks = false;
                        bool errorSaldo = false;
                        int OsksAllowed = 0;
                        decimal OsksAmbil = 0;
                        long OSisaSaldoSaatIni = 0;
                        if(validSks(out OsksAllowed,out OsksAmbil, i, acd_Student_Krs.Term_Year_Id, getCourseCostForKRS.applied_sks) == false)
                        {
                            errorSks = true;
                        }
                        if (validSaldo(out OSisaSaldoSaatIni, i, acd_Student_Krs.Term_Year_Id, getCourseCostForKRS.amount) == false)
                        {
                            errorSaldo = true;
                        }
                        if (errorSks || errorSaldo)
                        {
                            listIdError.Add(new Vm_notif_error 
                                                { 
                                                    Student_Id = i, 
                                                    errorSks = errorSks,
                                                    errorSaldo = errorSaldo,
                                                    sksAllowed = OsksAllowed,
                                                    sksAmbil = OsksAmbil,
                                                    SisaSaldoSaatIni = OSisaSaldoSaatIni
                                                });
                            jmlSkipLoop = jmlSkipLoop + 1;
                            continue;
                        }
                        
                        acd_Student_Krs.Student_Id = i;
                        acd_Student_Krs.Sks = Convert.ToDecimal(getCourseCostForKRS.applied_sks);
                        acd_Student_Krs.Amount = Convert.ToInt32(getCourseCostForKRS.amount);
                        acd_Student_Krs.Created_Date = DateTime.Now;
                        acd_Student_Krs.Krs_Date = DateTime.Now;
                        acd_Student_Krs.Modified_Date = DateTime.Now;

                        acd_Student_Krs.Cost_Item_Id = 3; //deposit KRS, nanti dikembangkan bisa 9(KKN)

                        db.Acd_Student_Krs.Add(acd_Student_Krs);
                        db.SaveChanges();
                    }

                    if (jmlSkipLoop > 0)
                    {
                        errorSave = true;
                        if (jmlSkipLoop <= 10)
                        {
                            TempData["detailMassage"] = listIdError;
                            //string html = string.Empty;
                            //foreach (var item in listIdError)
                            //{
                            //    html = "<tr><td>" + item.NimNama.Nim + "</td><td>" + item.NimNama.Full_Name + "</td><td>" + item.Keterangan + "</td></tr>";
                            //}
                            //TempData["detailMassage"] = "<table>" + html + "</table>";
                        }
                        else
                        {
                            TempData["detailMassage1"] = jmlSkipLoop + " Mahasisawa gagal diinput";
                        }


                        //TempData["detailMassage"] = listIdError;
                        //TempData["massage"] = jmlSkipLoop + " Mahasisawa gagal diinput";
                        //string[] arrayIdError;
                        //arrayIdError = listIdError.ConvertAll<String>(p => p.Student_Id.ToString()).ToArray<String>();
                    }
                }
                catch (Exception)
                {
                    TempData["massage"] = "Proses gagal. Terjadi kesalahan database!";
                    errorSave = true;
                }
            }

            if (errorSave)
            {
                return RedirectToAction("ListAdd", new
                {
                    Term_Year_Id = acd_Student_Krs.Term_Year_Id,
                    Class_Prog_Id = acd_Student_Krs.Class_Prog_Id,
                    Department_Id = Department_Id,
                    Course_Id = acd_Student_Krs.Course_Id,
                    Class_Id = acd_Student_Krs.Class_Id,
                    Offered_Course_id = Offered_Course_id,
                    Entry_Year_Id = Entry_Year_Id
                });
            }
            else
            {
                TempData["massage_success"] = "Sukses, data berhasil ditambahkan";
                return RedirectToAction("List", new { 
                    Term_Year_Id = acd_Student_Krs.Term_Year_Id, 
                    Class_Prog_Id = acd_Student_Krs.Class_Prog_Id, 
                    Course_Id = acd_Student_Krs.Course_Id,
                    Department_Id = Department_Id,
                    Class_Id = acd_Student_Krs.Class_Id,
                    Offered_Course_id = Offered_Course_id
                });
            }
        }

        private bool validKouta(int Offered_Course_id, short Term_Year_Id, short Class_Prog_Id, int Course_Id, short Class_Id, IEnumerable<long> checkStudent_IdToAdd)
        {
            short? capacity = db.Acd_Offered_Course.Where(x => x.Offered_Course_id == Offered_Course_id)
                                                .Select(x => x.Class_Capacity).First();

            int count_student = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                            && x.Class_Prog_Id == Class_Prog_Id
                                                            && x.Course_Id == Course_Id
                                                            && x.Class_Id == Class_Id)
                                                        .Select(x => x.Student_Id)
                                                        .Count();
            int? sisa_kuota = capacity - count_student;
            if ( sisa_kuota < checkStudent_IdToAdd.Count())
	        {
                return false;
	        }
            else
            {
                return true;
            }
        }

        private bool validSks(out int OsksAllowed,out decimal OsksAmbil, long Student_Id, short Term_Year_Id, decimal? Sks)
        {
            var sksAllowed = db.usp_GetAllowedSKSForKRS(Term_Year_Id, Student_Id).FirstOrDefault();
            var sksAmbil =  db.Acd_Student_Krs.Where(x => x.Student_Id == Student_Id && x.Term_Year_Id == Term_Year_Id).AsEnumerable().Sum(x => x.Sks);
            if ((sksAllowed - sksAmbil) < Sks)
            {
                OsksAllowed =  (sksAllowed == null) ? 0 : Convert.ToInt32(sksAllowed);
                //OsksAmbil = (sksAmbil == null) ? 0 : Convert.ToDecimal(sksAmbil);
                OsksAmbil = Convert.ToDecimal(sksAmbil);

                return false;
            }
            else
	        {
                OsksAllowed = (sksAllowed == null) ? 0 : Convert.ToInt32(sksAllowed);
                //OsksAmbil = (sksAmbil == null) ? 0 : Convert.ToDecimal(sksAmbil);
                OsksAmbil = Convert.ToDecimal(sksAmbil);

                return true;
	        }
        }

        private bool validSaldo(out long OSisaSaldoSaatIni, long Student_Id, short Term_Year_Id, decimal? amount)
        {
            var sisaSaldo = db.usp_Saldo(Student_Id, Term_Year_Id).FirstOrDefault();

            if (sisaSaldo.SisaSaldoSaatIni < amount)
            {
                OSisaSaldoSaatIni = (sisaSaldo.SisaSaldoSaatIni == null) ? 0 : (long)sisaSaldo.SisaSaldoSaatIni; 
                return false;
            }
            else
            {
                OSisaSaldoSaatIni = (sisaSaldo.SisaSaldoSaatIni == null) ? 0 : (long)sisaSaldo.SisaSaldoSaatIni; 
                return true;
            }
        }

        // GET: StudentKrsV1/List
        public ActionResult DetailError(List<Vm_notif_error>listIdError, string currentFilter, string searchString, short Term_Year_Id, short Class_Prog_Id, short Department_Id, int Course_Id, short Class_Id, int Offered_Course_id, short? Entry_Year_Id, int? page, int? rowPerPage, string alert)
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
            ViewBag.Class_Prog = Class_Prog_Id;
            ViewBag.Course = Course_Id;
            ViewBag.Class = Class_Id;
            ViewBag.Department = Department_Id;
            ViewBag.Offered_Course = Offered_Course_id;

            ViewBag.nav = db.Acd_Offered_Course.Where(x => x.Offered_Course_id == Offered_Course_id)
                //.Select(x => new 
                //{
                //    Term_Year_Name = x.Mstr_Term_Year.Term_Year_Name,
                //    Department_Name = x.Mstr_Department.Department_Name,
                //    Class_Program_Name = x.Mstr_Class_Program.Class_Program_Name,
                //    Course_Name = x.Acd_Course.Course_Name,
                //    Class_Name = x.Mstr_Class.Class_Name
                //})
                                                .First();

            var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                            && x.Class_Prog_Id == Class_Prog_Id
                                                            && x.Course_Id == Course_Id
                                                            && x.Class_Id == Class_Id);

            ViewBag.nav_count_data = acd_Student_Krs.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student_Krs = acd_Student_Krs.Where(x => x.Acd_Student.Nim.Contains(searchString) || x.Acd_Student.Full_Name.Contains(searchString));
            }

            acd_Student_Krs = acd_Student_Krs.OrderBy(x => x.Acd_Student.Nim);

            return View(listIdError.ToPagedList(pageNumber, pageSize));
        }

        // POST: StudentKrsV1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Krs_Id,Student_Id,Term_Year_Id,Course_Id,Class_Prog_Id,Class_Id,Sks,Amount,Nb_Taking,Krs_Date,Due_Date,Cost_Item_Id,Is_Approved,Is_Locked,Modified_By,Modified_Date,Created_Date,Order_Id,Created_By")] Acd_Student_Krs acd_Student_Krs)
        {
            if (ModelState.IsValid)
            {
                db.Acd_Student_Krs.Add(acd_Student_Krs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Student_Krs.Course_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Student_Krs.Student_Id);
            ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item, "Cost_Item_Id", "Cost_Item_Code", acd_Student_Krs.Cost_Item_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student_Krs.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student_Krs.Class_Prog_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", acd_Student_Krs.Term_Year_Id);
            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV1/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Student_Krs.Course_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Student_Krs.Student_Id);
            ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item, "Cost_Item_Id", "Cost_Item_Code", acd_Student_Krs.Cost_Item_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student_Krs.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student_Krs.Class_Prog_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", acd_Student_Krs.Term_Year_Id);
            return View(acd_Student_Krs);
        }

        // POST: StudentKrsV1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Krs_Id,Student_Id,Term_Year_Id,Course_Id,Class_Prog_Id,Class_Id,Sks,Amount,Nb_Taking,Krs_Date,Due_Date,Cost_Item_Id,Is_Approved,Is_Locked,Modified_By,Modified_Date,Created_Date,Order_Id,Created_By")] Acd_Student_Krs acd_Student_Krs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acd_Student_Krs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course_Id = new SelectList(db.Acd_Course, "Course_Id", "Course_Code", acd_Student_Krs.Course_Id);
            ViewBag.Student_Id = new SelectList(db.Acd_Student, "Student_Id", "Nim", acd_Student_Krs.Student_Id);
            ViewBag.Cost_Item_Id = new SelectList(db.Fnc_Cost_Item, "Cost_Item_Id", "Cost_Item_Code", acd_Student_Krs.Cost_Item_Id);
            ViewBag.Class_Id = new SelectList(db.Mstr_Class, "Class_Id", "Class_Name", acd_Student_Krs.Class_Id);
            ViewBag.Class_Prog_Id = new SelectList(db.Mstr_Class_Program, "Class_Prog_Id", "Class_Prog_Code", acd_Student_Krs.Class_Prog_Id);
            ViewBag.Term_Year_Id = new SelectList(db.Mstr_Term_Year.OrderByDescending(ty => ty.Term_Year_Id), "Term_Year_Id", "Term_Year_Name", acd_Student_Krs.Term_Year_Id);
            return View(acd_Student_Krs);
        }

        // GET: StudentKrsV1/Delete/5
        public ActionResult Delete(long? id, int Offered_Course_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            if (acd_Student_Krs == null)
            {
                return HttpNotFound();
            }

            //for url
            ViewBag.Offered_Course = Offered_Course_id;

            return View(acd_Student_Krs);
        }

        // POST: StudentKrsV1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, int Offered_Course_id)
        {
            Acd_Student_Krs acd_Student_Krs = db.Acd_Student_Krs.Find(id);
            short? Department_Id = acd_Student_Krs.Acd_Student.Department_Id;
            db.Acd_Student_Krs.Remove(acd_Student_Krs);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("List", new
            {
                Term_Year_Id = acd_Student_Krs.Term_Year_Id,
                Class_Prog_Id = acd_Student_Krs.Class_Prog_Id,
                Course_Id = acd_Student_Krs.Course_Id,
                Class_Id = acd_Student_Krs.Class_Id,
                Department_Id = Department_Id,
                Offered_Course_id = Offered_Course_id
            });
        }

        public ActionResult Export(string searchString, short Term_Year_Id, short Class_Prog_Id, int Course_Id, short Department_Id, short Class_Id, int Offered_Course_id, string tipe)
        {
            var acd_Student_Krs = db.Acd_Student_Krs.Where(x => x.Term_Year_Id == Term_Year_Id
                                                            && x.Class_Prog_Id == Class_Prog_Id
                                                            && x.Course_Id == Course_Id
                                                            && x.Class_Id == Class_Id);

            ViewBag.nav_count_data = acd_Student_Krs.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                acd_Student_Krs = acd_Student_Krs.Where(x => x.Acd_Student.Nim.Contains(searchString) || x.Acd_Student.Full_Name.Contains(searchString));
            }

            acd_Student_Krs = acd_Student_Krs.OrderBy(x => x.Acd_Student.Nim);

            IQueryable<VM_CetakPresensiMahasiswa> printList = acd_Student_Krs.Select(s => new VM_CetakPresensiMahasiswa
            {
                Nim = s.Acd_Student.Nim,
                Full_Name = s.Acd_Student.Full_Name,
                Gender = (s.Acd_Student.Gender_Id == 0) ? "P" : (s.Acd_Student.Gender_Id == 1) ? "L" : "",
                Faculty_Name = "FAKULTAS " + s.Acd_Student.Mstr_Department.Mstr_Faculty.Faculty_Name,
                Department_Name = s.Acd_Student.Mstr_Department.Department_Name,
                Class_Name = s.Mstr_Class.Class_Name + "-" + s.Mstr_Class_Program.Class_Program_Name,
                Semester = s.Mstr_Term_Year.Mstr_Term.Term_Name + " " + s.Mstr_Term_Year.Mstr_Entry_Year.Entry_Year_Name,
                Course_Name = s.Acd_Course.Course_Name,
                Course_Code = s.Acd_Course.Course_Code,
                Dosen_Full_Name = s.Acd_Course.Acd_Offered_Course.FirstOrDefault().Acd_Offered_Course_Lecturer.FirstOrDefault().Emp_Employee.Full_Name, //s.Acd_Course.Acd_Offered_Course.FirstOrDefault().dosen,
                Jadwal = "",
                Judul = "Semester " + s.Mstr_Term_Year.Mstr_Term.Term_Name + " Tahun Akademik " + s.Mstr_Term_Year.Mstr_Entry_Year.Entry_Year_Name
            });

            string Angkatan = printList.FirstOrDefault().Semester.ToString();
            string Class_Program_Name = printList.FirstOrDefault().Class_Name.ToString();
            string kodeMat = printList.FirstOrDefault().Course_Code.ToString();

            ReportDocument rd = new ReportDocument();
            switch (tipe)
            {
                case "PresensiMahasiswa":
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "rpt_PresensiMahasiswa.rpt"));
                    break;
                case "FormNilaiAkhir":
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "rpt_FormNilaiAkhir.rpt"));
                    break;
            }
            rd.SetDataSource(printList);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", tipe +  "_" + Angkatan + "_" + Class_Program_Name + "_" + kodeMat + ".pdf");

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                throw;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
