using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Core.Repositories
{
    public interface IBrancheRepository:IGenericRepository<Branche>
    {
        public void Update(Branche brance, string userId);
    }
}
