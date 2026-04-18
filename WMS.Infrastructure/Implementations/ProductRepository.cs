using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context; 
        }
        public void Update(Product obj, string userId)
        {
            var objFromDb = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CostPrice = obj.CostPrice;
                objFromDb.SellingPrice = obj.SellingPrice;
                objFromDb.SmallestUnitId = obj.SmallestUnitId;

                
                objFromDb.ModifiedAt = DateTime.Now;
                
                objFromDb.ModifiedBy = userId;
                
            }
        }
    }
}
