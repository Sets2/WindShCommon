using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions.Repositories;
using Core.Domain;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class Repository<T>: IRepository<T> where T: class, IBaseEntity
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllPaginateAsync(int take, int skip)
        {
            return await _context.Set<T>()
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task CreateAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync(); 
        }
        public async Task UpdateAsync(T obj)
        {
            //_context.Set<T>().Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var obj = await GetByIdAsync(id);
            if (obj != null)
            {
                _context.Set<T>().Remove(obj);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<T?> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public IQueryable<T> GetAsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

    }
}
