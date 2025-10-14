using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepo DepartmentRepo { get; }
        IEmployeeRepo EmployeeRepo { get; }
        int SaveChanges();
    }
}
