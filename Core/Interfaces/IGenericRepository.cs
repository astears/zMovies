using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace zMovies.Core.Interfaces
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);

    }
}