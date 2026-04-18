using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class ImageRepository:GenericRepository<Image>,IImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Image Image)
        {
            var ImageInDb = _context.Images.FirstOrDefault(x => x.Id == Image.Id);
            if (ImageInDb != null)
            {
                ImageInDb.ImageUrl = Image.ImageUrl;
                ImageInDb.ProductId = Image.ProductId;
            }
        }
    }
}
