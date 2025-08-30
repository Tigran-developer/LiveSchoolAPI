using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Entities;

namespace WebAPI.SeedConfiguration
{
    public class ClassStatusConfiguration : IEntityTypeConfiguration<ClassStatus>
    {
        public void Configure(EntityTypeBuilder<ClassStatus> builder)
        {
            builder.HasData(
                new ClassStatus { Id = 1, Name = "DRAFT" },
                new ClassStatus { Id = 2, Name = "PUBLISHED" },
                new ClassStatus { Id = 3, Name = "IN_PROGRESS" },
                new ClassStatus { Id = 4, Name = "COMPLETED" },
                new ClassStatus { Id = 5, Name = "CANCELLED" },
                new ClassStatus { Id = 6, Name = "ARCHIVED" }
            );
        }
    }
}

