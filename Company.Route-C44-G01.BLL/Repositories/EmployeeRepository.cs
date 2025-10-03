using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Route_C44_G01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepo
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) // Call the base class constructor
        {
            _context = context;
        }

        public List<Employee> GetByName(string name)
        {
           return _context.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
