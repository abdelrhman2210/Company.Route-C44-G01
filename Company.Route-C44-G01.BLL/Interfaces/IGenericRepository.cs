using Company.Route_C44_G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Interfaces
{

    public interface IGenericRepository<T> where T : BaseEntity 
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetByName(string name);
        Task<T?> Get(int id);

        Task Add(T data);
        void Update(T data);
        void Delete(T data);
    }
}
