using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Core.Repositories
{
    public interface IWarehouseRepository:IGenericRepository<Warehouse>
    {
        public void Update(Warehouse warehouse,string userId);
    }
}
