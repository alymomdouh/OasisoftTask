using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OasisoftTask.Applications.Dtos.ToDoDtos;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Common;
using OasisoftTask.Core.DomainModels;
using OasisoftTask.Infrastructure.UnitOfWorks;

namespace OasisoftTask.Applications.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ListToDo> AddAsync(CreateToDo item)
        {
            var entity = _mapper.Map<ToDo>(item);
            await _unitOfWork.Repository<ToDo>().AddAsync(entity);
            await _unitOfWork.Save();
            return _mapper.Map<ListToDo>(entity);
        }
        public async Task UpdateAsync(CreateToDo item, int id)
        {
            var entity = await _unitOfWork.Repository<ToDo>().GetByIdAsync(id);
            if (entity != null)
            {
                entity.UpdateData(item.Title, item.UserId, item.Completed);
                await _unitOfWork.Repository<ToDo>().UpdateAsync(entity);
                await _unitOfWork.Save();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ToDo>().GetByIdAsync(id);
            if (entity != null)
            {
                await _unitOfWork.Repository<ToDo>().DeleteAsync(entity);
                await _unitOfWork.Save();
            }
        }

        public async Task<ListToDo?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ToDo>().GetByIdAsync(id);
            if (entity != null)
            {
                return _mapper.Map<ListToDo>(entity);
            }
            return default;
        }

        public async Task<List<ListToDo>> GetToDoListAsync()
        {
            var entities = await _unitOfWork.Repository<ToDo>().GetTableNoTracking()
                  .ProjectTo<ListToDo>(_mapper.ConfigurationProvider)
                  .ToListAsync();
            return entities;
        }
        public async Task<PageResult<ListToDo>> GetAllWithPaginationAsync(PageRequest query)
        {
            return await _unitOfWork.Repository<ToDo>().GetTableNoTracking()
                   .OrderBy(x => x.Id)
                   .ProjectTo<ListToDo>(_mapper.ConfigurationProvider)
                   .ToPaginatedListAsync(query.Page, query.PageSize);
        }
    }
}
