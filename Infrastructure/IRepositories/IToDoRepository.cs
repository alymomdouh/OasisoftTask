using OasisoftTask.Core.DomainModels;

namespace OasisoftTask.Infrastructure.IRepositories
{
    public interface IToDoRepository : IGenericRepository<ToDo>
    {
        Task<List<ToDo>> GetToDoListAsync();
    }
}