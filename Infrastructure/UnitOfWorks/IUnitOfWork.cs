using OasisoftTask.Core.DomainModels;
using OasisoftTask.Infrastructure.IRepositories;

namespace OasisoftTask.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Save(CancellationToken cancellationToken = default);
        Task Rollback();
    }
}
