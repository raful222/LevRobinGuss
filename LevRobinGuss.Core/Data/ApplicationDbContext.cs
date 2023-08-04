using LevRobinGuss.Core.Entities;
using LevRobinGuss.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LevRobinGuss.Core.Data
{
    /*
     * Add-Migration -OutputDir "Data/Migrations" Initial -Project LevRobinGuss.Core -StartupProject LevRobinGuss
     * Update-Database -Project LevRobinGuss.Core -StartupProject LevRobinGuss
     * Remove-Migration -Force -Project LevRobinGuss.Core -StartupProject LevRobinGuss
     */
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
      ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
      ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var now = DateTime.Now;

            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added)
                {
                    auditableEntity.Entity.CreatedAt = now;
                    auditableEntity.Entity.UpdatedAt = now;
                }

                if (auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                    auditableEntity.Entity.UpdatedAt = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable(name: "Users");

                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable(name: "Roles");

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
        public virtual DbSet<Gambling> Gamblings { get; set; }

    }
}