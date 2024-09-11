using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisoftTask.Applications.Dtos.ToDoDtos;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Common;

namespace OasisoftTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> AddAsync(CreateToDo model)
        {
            return Ok(await _toDoService.AddAsync(model));
        }
        [HttpDelete("Delete")]
        public async Task DeleteAsync(int id)
        {
            await _toDoService.DeleteAsync(id);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _toDoService.GetByIdAsync(id));
        }
        [HttpPut("Update")]
        public async Task UpdateAsync([FromBody] CreateToDo item, int id)
        {
            await _toDoService.UpdateAsync(item, id);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _toDoService.GetToDoListAsync());
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize)
        {
            return Ok(await _toDoService.GetAllWithPaginationAsync(new PageRequest(page, pageSize)));
        }
    }
}
