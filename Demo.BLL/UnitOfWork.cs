using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Demo.BLL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _context;
        private Lazy <IDapartmentRepository>dapartmentRepository;
        private Lazy<IEployeeRepository>  employeeRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            dapartmentRepository = new Lazy<IDapartmentRepository>(new DapartmentRepository(_context));
            employeeRepository = new Lazy<IEployeeRepository>(new EmployeeRepository(_context));
        }
        public IDapartmentRepository  DapartmentRepository => dapartmentRepository.Value;
        public IEployeeRepository EmployeeRepository => employeeRepository.Value;

        public async Task<int> Complete()
        {
                return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
