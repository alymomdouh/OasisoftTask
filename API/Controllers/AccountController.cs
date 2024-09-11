using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisoftTask.Applications.Dtos.Account;
using OasisoftTask.Applications.IServices;

namespace OasisoftTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IServiceAccount _accountService;
        public AccountController(IServiceAccount accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            return Ok(await _accountService.Login(model));
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            return Ok(await _accountService.Register(model));
        }
    }
}
