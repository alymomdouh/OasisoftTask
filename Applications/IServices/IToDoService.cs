using OasisoftTask.Applications.Dtos.ToDoDtos;
using OasisoftTask.Common;

namespace OasisoftTask.Applications.IServices
{
    public interface IToDoService
    {
        Task DeleteAsync(int id);
        Task<ListToDo> GetByIdAsync(int id);
        Task<ListToDo> AddAsync(CreateToDo entity);
        Task UpdateAsync(CreateToDo entity, int id);
        Task<List<ListToDo>> GetToDoListAsync();
        Task<PageResult<ListToDo>> GetAllWithPaginationAsync(PageRequest query);
    }
}
