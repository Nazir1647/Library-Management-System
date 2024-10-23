using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Abstractions.Interfaces
{
    public interface IGenericRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T, bool>> Predicate);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> Predicate);
        Task<bool> SaveAsync(T entity);
        Task<bool> SaveRangeAsync(List<T> entity);
        bool Delete(T entity);
        bool DeleteRange(List<T> entity);
        Task<bool> UpdateAsync(T entity);

    }
}
