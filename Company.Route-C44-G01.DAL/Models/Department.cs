using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.DAL.Models
{
    public class Department : BaseEntity
    {
        
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
