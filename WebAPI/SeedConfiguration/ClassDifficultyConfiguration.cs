using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Entities;

namespace WebAPI.SeedConfiguration
{
    public class ClassDifficultyConfiguration : IEntityTypeConfiguration<ClassDifficulty>
    {
        public void Configure(EntityTypeBuilder<ClassDifficulty> builder)
        {
            builder.HasData(
                new ClassDifficulty { Id = 1, Name = "BEGINNER" },
                new ClassDifficulty { Id = 2, Name = "INTERMEDIATE" },
                new ClassDifficulty { Id = 3, Name = "ADVANCED" },
                new ClassDifficulty { Id = 4, Name = "EXPERT" }
            );
        }
    }
}

