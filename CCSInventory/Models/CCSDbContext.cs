using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    public class CCSDbContext : DbContext
    {
        public CCSDbContext(DbContextOptions<CCSDbContext> options) : base(options)
        {

        }

        // Annotations cannot mark certain things like Alternate Keys.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/alternate-keys
            // For Alternate Keys
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserName);

            // Seeding data: default admin user:
            var defaultAdmin = new User
            {
                ID = 1, // An ID must be specified here.
                FirstName = "Weber",
                LastName = "CS",
                UserName = "skram",
                Role = UserRole.ADMIN,
                Note = "Default user for an empty database",
                CreatedBy = "Seeded Data",
                ModifiedBy = "Seeded Data",
                // Seed Data is inserted using Migrations, so SaveChanges() isn't called
                // These fields must be manually set:
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            PasswordChangeResult r = defaultAdmin.ChangePassword("M8/iq+W1");
            if (r != PasswordChangeResult.PASSWORD_OK)
            {
                throw new Exception("Failure setting password for default user");
            }

            // Seeding data:
            modelBuilder.Entity<User>().HasData(defaultAdmin);
        }

        public override int SaveChanges()
        {
            UpdateModifiedTimestamp();
            return base.SaveChanges();
        }

        /// <summary>
        /// This method updates the Created and Modified timestamps for Entities about to be saved.
        /// </summary>
        private void UpdateModifiedTimestamp()
        {
            var entities = ChangeTracker.Entries().Where(
                x => x.Entity is TrackedModel && (x.State == EntityState.Added || x.State == EntityState.Modified)
            );

            foreach (var e in entities)
            {
                if (e.State == EntityState.Added)
                {
                    ((TrackedModel)e.Entity).Created = DateTime.UtcNow;
                }
                ((TrackedModel)e.Entity).Modified = DateTime.UtcNow;
            }
        }

        public DbSet<User> Users { get; set; }
    }
}
