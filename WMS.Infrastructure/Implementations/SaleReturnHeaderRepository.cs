using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class SaleReturnHeaderRepository:GenericRepository<SaleReturnHeader>,ISaleReturnHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public SaleReturnHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
