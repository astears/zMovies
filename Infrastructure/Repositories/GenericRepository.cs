using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;

namespace zMovies.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DataContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new DataContext();
            table = _context.Set<T>();
        }

        public GenericRepository(DataContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await table.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            await table.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(T entity)
        {
            table.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }       

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}