using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        //hero repositories Accessors
        IUserRepository User { get; }

        int Complete();
    }
}
