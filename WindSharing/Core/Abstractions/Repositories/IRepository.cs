using System.Linq.Expressions;

namespace Core.Abstractions.Repositories
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllPaginateAsync(int take, int skip);
        Task<T?> GetByIdAsync(Guid id);
        Task CreateAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids);
        Task<T?> GetFirstWhere(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAsQueryable();

    }
}
