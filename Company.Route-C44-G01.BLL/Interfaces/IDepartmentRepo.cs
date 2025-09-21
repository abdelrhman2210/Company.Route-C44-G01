using Company.Route_C44_G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Interfaces
{
    public interface IDepartmentRepo
    {
        IEnumerable<Department> GetAll();
        Department? Get(int id);

        int Add(Department model);
        int Update(Department model);
        int Delete(Department model);

    }
}
