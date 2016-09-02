using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SIA_Universitas.Extensions;
using System.Web.Caching;

namespace SIA_Universitas.Models
{
    public class EmployeeRepository
    {
        private SIAEntities db = new SIAEntities();

        public IQueryable<Emp_Employee> Emp_Employees { get; set; }

        public EmployeeRepository()
        {
            Emp_Employees = GenerateEmployees();
        }

        public List<Emp_Employee> GetEmployees(string searchTerm, int pageSize, int pageNum)
        {
            return GetEmployeesQuery(searchTerm).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();
        }

        public int GetEmployeesCount(string searchTerm, int pageSize, int pageNum)
        {
            return GetEmployeesQuery(searchTerm).Count();
        }

        private IQueryable<Emp_Employee> GetEmployeesQuery(string searchTerm)
        {
            var empStatus = db.Emp_Employee_Status.Where(es => es.Description.Contains("Dosen")).Select(es=>es.Employee_Status_Id).ToList();
            searchTerm = searchTerm.ToLower();
            return Emp_Employees.OrderBy(e => e.Name).Where(e => empStatus.Contains(e.Employee_Status_Id ?? 0) && e.Name.Like(searchTerm));
        }

        private IQueryable<Emp_Employee> GenerateEmployees()
        {
            string chaceKey = "employees";
            if (HttpContext.Current.Cache[chaceKey] != null)
            {
                return (IQueryable<Emp_Employee>)HttpContext.Current.Cache[chaceKey];
            }

            var employees = new List<Emp_Employee>();
            int intCount = Convert.ToInt32(db.Emp_Employee.Count());

            List<Emp_Employee> data = db.Emp_Employee.ToList();

            for (int i = 0; i < data.Count(); i++)
            {
                employees.Add
                (
                    new Emp_Employee()
                    {
                        Employee_Id = data[i].Employee_Id,
                        Name = data[i].Name,
                        Full_Name = data[i].Full_Name,
                        Nik = data[i].Nik,
                        Employee_Status_Id = data[i].Employee_Status_Id
                    }
                );
            }

            var result = employees.AsQueryable();

            HttpContext.Current.Cache[chaceKey] = result;

            return result;
        }
    }
}