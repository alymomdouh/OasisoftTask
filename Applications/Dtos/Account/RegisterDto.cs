using System.ComponentModel.DataAnnotations;

namespace OasisoftTask.Applications.Dtos.Account
{
    public record RegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
