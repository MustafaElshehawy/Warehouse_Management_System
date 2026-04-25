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
        public ICategoryRepository Category { get; private set; }

        public IUnitRepository Unit { get; private set; }

        public IProductUnitRepository ProductUnit { get; private set; }

        public IProductRepository Product { get; private set; }

        public IImageRepository Image { get; private set; }
        public IBrancheRepository Branche { get; private set; }

        public IWarehouseRepository Warehouse { get; private set; }

        public IStockRepository Stock { get; private set; }

        public IStockMovementRepository StockMovement { get; private set; }

        public IPurchaseHeaderRepository PurchaseHeader{ get; private set; }
        public IPurchaseDetailsRepository PurchaseDetails{ get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(context);
            Category=new CategoryRepository(context);
            Unit = new UnitRepository(context);
            ProductUnit=new ProductUnitRepository(context);
            Product = new ProductRepository(context);
            Image = new ImageRepository(context);
            Branche=new BrancheRepository(context);
            Warehouse=new WarehouseRepository(context);
            Stock=new StockRepository(context);
            StockMovement=new StockMovementRepository(context);
            PurchaseHeader=new PurchaseHeaderRepository(context);
            PurchaseDetails=new PurchaseDetailsRepository(context);
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
