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

            // Seeding data
            // Add a default user to the table.  Using anonymous type instead of a User
            // object so the passwordHash and all other data is constant across migrations
            DateTime createdModified = DateTime.Parse("2018-10-18T12:30:18.051Z").ToUniversalTime();
            modelBuilder.Entity<User>().HasData(new
            {
                ID = 1L, // Since ID is type long in User, specify integer constant as type Long with "L" suffix
                FirstName = "Weber",
                LastName = "CS",
                UserName = "skram",
                // This passwordhash is the hash for: M8/iq+W1
                PasswordHash = "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu",
                Role = UserRole.ADMIN,
                Note = "Default user for an empty database",
                CreatedBy = "Seeded Data",
                ModifiedBy = "Seeded Data",
                Created = createdModified,
                Modified = createdModified,
            });
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
