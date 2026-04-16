using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }

        public void Update(Category category,string userId)
        {
            var CategoryInDb = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (CategoryInDb != null)
            {
                CategoryInDb.Name = category.Name;
                CategoryInDb.Description = category.Description;
                CategoryInDb.ModifiedAt = DateTime.Now;
                CategoryInDb.ModifiedBy = userId;
            }
        }
    }
}
