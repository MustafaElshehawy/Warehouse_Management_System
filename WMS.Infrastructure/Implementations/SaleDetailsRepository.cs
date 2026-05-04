using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class SaleDetailsRepository:GenericRepository<SaleDetails>,ISaleDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public SaleDetailsRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
    }
}
