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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepo
    {
        public DepartmentRepository(CompanyDbContext context) : base(context) // Call the base class constructor
        {

        }
    }
}
