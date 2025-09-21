using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepo
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepository()
        {
            _context = new CompanyDbContext();
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }
        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }
        public int Add(Department model)
        {
            _context.Departments.Add(model);
            return _context.SaveChanges();

        }
        public int Update(Department model)
        {
            _context.Departments.Update(model);
            return _context.SaveChanges();
        }
        public int Delete(Department model)
        {
            _context.Departments.Remove(model);
            return _context.SaveChanges();
        }        
    }
}
