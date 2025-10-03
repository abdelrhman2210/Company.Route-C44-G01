using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepo
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepository(CompanyDbContext context) : base(context) // Call the base class constructor
        {
            _context = context;
        }

        public List<Department> GetByName(string name)
        {
              return _context.Departments.Where(D => D.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
