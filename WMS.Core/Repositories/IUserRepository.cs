using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Core.Repositories
{
    public interface IUserRepository:IGenericRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetAll(string currentUserId);

        ApplicationUser GetFirstOrDefault(string id);
    }
}
