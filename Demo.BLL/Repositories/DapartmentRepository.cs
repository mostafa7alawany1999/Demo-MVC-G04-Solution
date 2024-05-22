using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Data;

namespace Demo.BLL.Repositories
{
    public class DapartmentRepository : GenericRepository<Department> , IDapartmentRepository
    {
        public DapartmentRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
