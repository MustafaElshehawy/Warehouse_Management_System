using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.Implementations
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }
        public IEnumerable<ApplicationUser> GetAll(string currentUserId)
        {
            return _context.ApplicationUsers.Where(x => x.Id != currentUserId).ToList();
        }

        public ApplicationUser GetFirstOrDefault(string id)
        {
            return _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
        }
    }
}
