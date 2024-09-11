using OasisoftTask.Applications.Dtos.LiveToDoDtos;
using OasisoftTask.Common;

namespace OasisoftTask.Applications.IServices
{
    public interface ILiveToDoService
    {
        Task<List<ListLiveToDo>> GetToDoListAsync();
        Task<PageResult<ListLiveToDo>> GetAllWithPaginationAsync(PageRequest query);
    }
}
