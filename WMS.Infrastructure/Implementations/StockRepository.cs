using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class StockRepository:GenericRepository<Stock>,IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
