﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public  IDapartmentRepository DapartmentRepository { get;}
        public IEployeeRepository EmployeeRepository { get; }

       Task<int> Complete();

     
    }
}
