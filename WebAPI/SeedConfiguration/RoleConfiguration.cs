using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Entities;

namespace WebAPI.SeedConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role with full access"
                },
                new Role
                {
                    Id = "2",
                    Name = "Teacher",
                    NormalizedName = "TEACHER",
                    Description = "Teacher role with limited access"
                },
                new Role
                {
                    Id = "3",
                    Name = "Pupil",
                    NormalizedName = "PUPIL",
                    Description = "Pupil role with limited access"
                }
            );
        }
    }
}
