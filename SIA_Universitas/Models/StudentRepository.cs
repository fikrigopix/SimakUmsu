using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SIA_Universitas.Extensions;
using System.Web.Caching;

namespace SIA_Universitas.Models
{
    public class StudentRepository
    {
        private SIAEntities db = new SIAEntities();

        public IQueryable<Acd_Student> Acd_Students { get; set; }

        public StudentRepository()
        {
            Acd_Students = GenerateStudents();
        }

        public List<Acd_Student> GetStudents(short deptId, string searchTerm, int pageSize, int pageNum)
        {
            return GetStudentsQuery(deptId, searchTerm).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();
        }

        public int GetStudentsCount(short deptId, string searchTerm, int pageSize, int pageNum)
        {
            return GetStudentsQuery(deptId, searchTerm).Count();
        }

        private IQueryable<Acd_Student> GetStudentsQuery(short deptId, string searchTerm)
        {
            var studentYudisium = db.Acd_Yudisium.Select(y => y.Student_Id).ToList();
            searchTerm = searchTerm.ToLower();
            return Acd_Students.OrderByDescending(s => s.Student_Id).Where(s => !studentYudisium.Contains(s.Student_Id) &&
                                                                                s.Department_Id == deptId &&
                                                                                s.Full_Name.Like(searchTerm));
        }

        private IQueryable<Acd_Student> GenerateStudents()
        {
            string chaceKey = "students";
            if (HttpContext.Current.Cache[chaceKey] != null)
            {
                return (IQueryable<Acd_Student>)HttpContext.Current.Cache[chaceKey];
            }

            var students = new List<Acd_Student>();
            int intCount = Convert.ToInt32(db.Acd_Student.Count());

            List<Acd_Student> data = db.Acd_Student.ToList();

            for (int i = 0; i < data.Count; i++)
            {
                students.Add
                (
                    new Acd_Student()
                    {
                        Student_Id = data[i].Student_Id,
                        Full_Name = data[i].Full_Name,
                        Nim = data[i].Nim,
                        Department_Id = data[i].Department_Id
                    }
                );
            }

            var result = students.AsQueryable();

            HttpContext.Current.Cache[chaceKey] = result;

            return result;
        }
    }
}