using Microsoft.AspNetCore.Identity;

namespace OasisoftTask.Core.DomainModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public ApplicationUser() { }
        public ApplicationUser(string email, string userName, string name)
        {
            Email = email;
            UserName = userName;
            Name = name;
        }
    }
}
