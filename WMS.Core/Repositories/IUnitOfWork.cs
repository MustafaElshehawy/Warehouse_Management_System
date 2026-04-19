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
        int Complete();
    }
}
