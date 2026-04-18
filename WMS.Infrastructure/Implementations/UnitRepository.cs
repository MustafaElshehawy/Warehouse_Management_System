using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class UnitRepository:GenericRepository<Unit>,IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Unit unit, string userId)
        {
            var CategoryInDb = _context.Units.FirstOrDefault(x => x.Id == unit.Id);
            if (CategoryInDb != null)
            {
                CategoryInDb.Name = unit.Name;
                CategoryInDb.ModifiedAt = DateTime.Now;
                CategoryInDb.ModifiedBy = userId;
            }
        }
    }
}
