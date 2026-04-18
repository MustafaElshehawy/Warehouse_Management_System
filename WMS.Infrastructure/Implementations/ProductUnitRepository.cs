using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class ProductUnitRepository:GenericRepository<ProductUnit>,IProductUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductUnitRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        void IProductUnitRepository.Update(ProductUnit productUnit, string userId)
        {
            var ProductUnitInDb = _context.ProductUnits.FirstOrDefault(x => x.Id == productUnit.Id);
            if (ProductUnitInDb != null)
            {
                ProductUnitInDb.ProductId = productUnit.ProductId;
                ProductUnitInDb.ParentUnit = productUnit.ParentUnit;
                ProductUnitInDb.ChildUnit = productUnit.ChildUnit;
                ProductUnitInDb.UnitFactor = productUnit.UnitFactor;
                ProductUnitInDb.ParentUnitPrice = productUnit.ParentUnitPrice;
                ProductUnitInDb.ModifiedAt = DateTime.Now;
                ProductUnitInDb.ModifiedBy = userId;
            }
        }
    }
}
