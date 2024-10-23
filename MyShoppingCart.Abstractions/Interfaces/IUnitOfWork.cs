using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Abstractions.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        IGenericRepository<T> GenericRepository<T>() where T : class;
    }
}
