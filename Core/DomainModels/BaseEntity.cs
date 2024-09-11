using System.ComponentModel.DataAnnotations;

namespace OasisoftTask.Core.DomainModels
{
    public class BaseEntity

    {
        [Required]
        [Key]
        // [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
    }
}
