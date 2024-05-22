using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private protected AppDbContext _Context;


        public GenericRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _Context.Employees.Include(E=>E.Department).ToListAsync();
            }
            else
            {
                return await _Context.Set<T>().ToListAsync();
            } 

        }
        public async Task<T> Get(int id)
        {
            var result = await _Context.Set<T>().FindAsync(id);

            return result;
        }

        public void Add(T entity)
        {
             _Context.Add(entity);    
        }

        public void Update(T entity)
        {
            _Context.Update(entity);
        }
        public void Delete(T entity)
        {
            _Context.Remove(entity);          
        }

    
    }
}
