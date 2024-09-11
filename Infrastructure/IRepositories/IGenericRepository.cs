using Microsoft.EntityFrameworkCore.Storage;

namespace OasisoftTask.Infrastructure.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task DeleteRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task<IDbContextTransaction> BeginTransactionAsync();
        IDbContextTransaction BeginTransaction();
        Task CommitAsync();
        Task RollBackAsync();
        void Commit();
        void RollBack();
    }
}
