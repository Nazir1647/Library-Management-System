using Microsoft.EntityFrameworkCore;
using LMS.Abstractions.Interfaces;
using LMS.Tables.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Abstractions.Implementations
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        private bool _disposed;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }
        public bool DeleteRange(List<T> entity)
        {
            _dbSet.RemoveRange(entity);
            return true;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> Predicate)
        {
            return await _dbSet.Where(Predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T, bool>> Predicate)
        {
            return await _dbSet.Where(Predicate).AsNoTracking().ToListAsync();
        }

        public async Task<bool> SaveAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public async Task<bool> SaveRangeAsync(List<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }
    }
}
