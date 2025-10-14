using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _dbContext;
        public GenericRepository(CompanyDbContext context)
        {
            _dbContext = context;
        }

        public async Task Add(T data)
        {
            await _dbContext.Set<T>().AddAsync(data);
        }

        public void Delete(T data)
        {
            _dbContext.Set<T>().Remove(data);
        }

        public async Task<T?> Get(int id)
        {
            if(typeof(T) == typeof(Employee))
            {
                return await _dbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id) as T;
            }
            return _dbContext.Set<T>().Find(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _dbContext.Employees.Include(e => e.Department).ToListAsync()as IEnumerable<T>;
            }
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByName(string name)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _dbContext.Employees.Include(e => e.Department).Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync() as IEnumerable<T>;
            }
            else
                return await _dbContext.Departments.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync() as IEnumerable<T>;
        }

        public void Update(T data)
        {
            _dbContext.Set<T>().Update(data);
        }
    }
}
