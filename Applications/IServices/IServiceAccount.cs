using OasisoftTask.Applications.Dtos.Account;

namespace OasisoftTask.Applications.IServices
{
    public interface IServiceAccount
    {
        Task<UserResult> Login(LoginDto model);
        Task<UserResult> Register(RegisterDto model);
    }
}
