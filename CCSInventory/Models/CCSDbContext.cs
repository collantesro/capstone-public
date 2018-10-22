using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is the Entity Framework Core DbContext for the app's primary data.
    /// </summary>
    public class CCSDbContext : DbContext
    {
        public CCSDbContext(DbContextOptions<CCSDbContext> options) : base(options)
        {

        }

        // Annotations cannot mark certain things like Alternate Keys.
        /// <summary>
        /// This overridden method is used to define seed data and properties/relationships
        /// that do not have annotations.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/alternate-keys
            // For Alternate Keys.  A UserName must be unique to allow proper login.
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

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling the
        /// base class' SaveChanges() method.  See DbContext.SaveChanges()
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            UpdateModifiedTimestamp();
            return base.SaveChanges();
        }

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling
        /// the base class' SaveChangesAsync() method.  See DbContext.SaveChangesAsync()
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)){
            UpdateModifiedTimestamp();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// This method updates the Created and Modified timestamps for Entities extending from
        /// TrackedModel that are about to be saved.
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

        // DbSets here:
        // EF Core implicitly creates DbSets for models referenced by parent models.
        public DbSet<User> Users { get; set; }
    }
}
