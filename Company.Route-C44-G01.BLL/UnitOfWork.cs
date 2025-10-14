using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepo DepartmentRepo { get;  }

        public IEmployeeRepo EmployeeRepo { get; }

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;

            EmployeeRepo = new EmployeeRepository(_context);
            DepartmentRepo = new DepartmentRepository(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
