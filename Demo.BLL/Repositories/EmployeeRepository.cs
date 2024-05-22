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
    public class EmployeeRepository : GenericRepository<Employee> , IEployeeRepository
    {

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task <IEnumerable<Employee>> GetByNAme(string name)
        {
           return await _Context.Employees.Where(E=>E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department).ToListAsync();
        }
    }
}
