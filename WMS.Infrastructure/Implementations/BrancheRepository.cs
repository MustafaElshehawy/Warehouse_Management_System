using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class BrancheRepository:GenericRepository<Branche>,IBrancheRepository
    {
        private readonly ApplicationDbContext _context;
        public BrancheRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Branche branche, string userId)
        { 
            var BrancheInDb=_context.Branches.FirstOrDefault(x=>x.Id == branche.Id);
            if(BrancheInDb!=null)
            {
                BrancheInDb.Name=branche.Name;
                BrancheInDb.Address=branche.Address;
                BrancheInDb.IsActive = branche.IsActive;
                BrancheInDb.ModifiedAt = DateTime.Now;
                BrancheInDb.ModifiedBy = userId;

            }
        }
    }
}
