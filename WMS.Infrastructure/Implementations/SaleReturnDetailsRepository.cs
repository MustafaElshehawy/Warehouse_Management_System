using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class SaleReturnDetailsRepository:GenericRepository<SaleReturnDetails>,ISaleReturnDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleReturnDetailsRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
