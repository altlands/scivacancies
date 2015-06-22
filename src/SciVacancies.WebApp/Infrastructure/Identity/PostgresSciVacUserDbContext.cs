using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Framework.OptionsModel;
using SciVacancies.ApplicationInfrastructure;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    [DbConfigurationType(typeof(PostgresDbConfiguration))]
    public class PostgresSciVacUserDbContext : SciVacUserDbContext
    {
        public PostgresSciVacUserDbContext(IOptions<DbSettings> dbOptions) : base(dbOptions.Options.ReadModelDb)
        {
        }
        public PostgresSciVacUserDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            // Needed to ensure subclasses share the same table
            var user = modelBuilder.Entity<SciVacUser>().ToTable("AspNetUsers","public");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName).IsRequired();

            // CONSIDER: u.Email is Required if set on options?

            // Look into IValidatable...

            // Try this (Configure Join table name HasMany.WithMany.Map(To match 1.0 tables)
            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles", "public");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins", "public");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("AspNetUserClaims", "public");

            var role = modelBuilder.Entity<IdentityRole>()
                .ToTable("AspNetRoles", "public");
            role.Property(r => r.Name).IsRequired();
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }
    }
}
