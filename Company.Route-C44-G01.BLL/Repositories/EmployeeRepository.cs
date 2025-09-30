using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;

namespace Company.Route_C44_G01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepo
    {
        public EmployeeRepository(CompanyDbContext context) : base(context) // Call the base class constructor
        {

        }

    }
}
