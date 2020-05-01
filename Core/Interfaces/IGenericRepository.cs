using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace zMovies.Core.Interfaces
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task Save();

    }
}