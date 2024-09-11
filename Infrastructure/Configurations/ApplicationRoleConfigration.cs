using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OasisoftTask.Core.DomainModels;

namespace OasisoftTask.Infrastructure.Configurations
{
    public class ApplicationRoleConfigration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
