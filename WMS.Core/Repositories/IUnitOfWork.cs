using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        //hero repositories Accessors
        IUserRepository User { get; }
        ICategoryRepository Category { get; }

        IUnitRepository Unit { get; }

        IProductUnitRepository ProductUnit { get; }

        IProductRepository Product { get; }

        IImageRepository Image { get; }

        IBrancheRepository Branche { get; }

        IWarehouseRepository Warehouse { get; }

        IStockRepository Stock { get; }

        IStockMovementRepository StockMovement { get; }


        IPurchaseDetailsRepository PurchaseDetails { get; }
        IPurchaseHeaderRepository PurchaseHeader { get; }

        IPurchaseReturnHeaderRepository PurchaseReturnHeader { get; }
        IPurchaseReturnDetailsRepository PurchaseReturnDetails { get; }

        ISaleHeaderRepository SaleHeader { get; }
        ISaleDetailsRepository SaleDetails { get; }

        ISaleReturnHeaderRepository SaleReturnHeader { get; }
        ISaleReturnDetailsRepository SaleReturnDetails { get; }
        int Complete();
    }
}
