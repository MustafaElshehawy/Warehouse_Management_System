using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class WarehouseRepository:GenericRepository<Warehouse>,IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;
        public WarehouseRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(Warehouse warehouse, string userId)
        {
            var warehouseInDb = _context.Warehouses.FirstOrDefault(w => w.Id == warehouse.Id);
            if (warehouseInDb != null)
            {
                warehouseInDb.Name = warehouse.Name;
                warehouseInDb.Address = warehouse.Address;
                warehouseInDb.BranchId = warehouse.BranchId;
                warehouseInDb.ModifiedBy = userId;
                warehouseInDb.CreatedAt = DateTime.UtcNow;

            }
        
        }
    }
}
