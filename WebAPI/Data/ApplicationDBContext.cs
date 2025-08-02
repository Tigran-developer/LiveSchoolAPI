using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;
using WebAPI.SeedConfiguration;

namespace WebAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");

            builder.Entity<User>(b =>
            {
                b.ToTable("Users", "identity");
                b.Property(u => u.Id).HasMaxLength(450);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                b.Property(u => u.Initials).HasMaxLength(15);
                b.Property(u => u.FirstName).HasMaxLength(50);
                b.Property(u => u.LastName).HasMaxLength(50);
                b.Property(u => u.Phone).HasMaxLength(20);
                b.HasIndex(u => u.Email).IsUnique();
            });

            builder.Entity<Role>(b =>
            {
                b.ToTable("Roles", "identity");
                b.Property(r => r.Id).HasMaxLength(450);
            });

            builder.Entity<Teacher>(b =>
            {
                b.ToTable("Teachers");
                b.HasOne(t => t.User)
                    .WithOne()
                    .HasForeignKey<Teacher>(t => t.UserId);
            });

            builder.Entity<Pupil>(b =>
            {
                b.ToTable("Pupils");
                b.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<Pupil>(p => p.UserId);
            });

            builder.Entity<Admin>()
                .HasMany<Class>()
                .WithOne(c => c.Admin)
                .HasForeignKey(c => c.CreatedBy);

            builder.Entity<ClassPupil>(b =>
            {
                b.ToTable("ClassPupils");

                b.HasOne(cp => cp.Class)
                    .WithMany(c => c.ClassPupils)
                    .HasForeignKey(cp => cp.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(cp => cp.Pupil)
                    .WithMany()
                    .HasForeignKey(cp => cp.PupilId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(cp => new { cp.ClassId, cp.PupilId }).IsUnique();

                b.Property(cp => cp.Status).HasMaxLength(20);
            });

            builder.ApplyConfiguration(new RoleConfiguration());
        }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassPupil> ClassPupils { get; set; }
    }
}
