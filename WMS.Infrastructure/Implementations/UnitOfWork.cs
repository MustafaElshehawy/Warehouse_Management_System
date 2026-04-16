using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(context);
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
