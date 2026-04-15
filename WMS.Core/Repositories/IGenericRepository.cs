using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace WMS.Core.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);

        T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
