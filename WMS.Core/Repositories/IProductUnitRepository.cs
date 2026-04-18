using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Core.Repositories
{
    public  interface IProductUnitRepository:IGenericRepository<ProductUnit>
    {
        public void Update(ProductUnit productUnit, string userId);
    }
}
