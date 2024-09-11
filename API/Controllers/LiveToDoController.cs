using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Common;

namespace OasisoftTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LiveToDoController : ControllerBase
    {
        private readonly ILiveToDoService _liveToDoService;

        public LiveToDoController(ILiveToDoService liveToDoService)
        {
            _liveToDoService = liveToDoService;
        }
        [HttpGet(nameof(GetListLiveToDo))]
        public async Task<ActionResult> GetListLiveToDo()
        {
            return Ok(await _liveToDoService.GetToDoListAsync());
        }
        [HttpGet(nameof(GetPaginationLiveToDo))]
        public async Task<ActionResult> GetPaginationLiveToDo(int page = Constants.Page, int pageSize = Constants.PageSize)
        {
            return Ok(await _liveToDoService.GetAllWithPaginationAsync(new PageRequest(page, pageSize)));
        }
    }
}
