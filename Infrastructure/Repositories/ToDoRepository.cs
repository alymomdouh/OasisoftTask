using Microsoft.EntityFrameworkCore;
using OasisoftTask.Core.DomainModels;
using OasisoftTask.Infrastructure.IRepositories;

namespace OasisoftTask.Infrastructure.Repositories
{
    public class ToDoRepository : GenericRepository<ToDo>, IToDoRepository
    {
        private readonly DbSet<ToDo> _dbSetItem;
        public ToDoRepository(ApplicationDbContext dBContext) : base(dBContext)
        {
            _dbSetItem = dBContext.Set<ToDo>();
        }
        public async Task<List<ToDo>> GetToDoListAsync()
        {
            return await _dbSetItem
                              //.Include(x => x.ApplicationUserObj)
                              .ToListAsync();
        }
    }
}
