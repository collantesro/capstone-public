using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is the Entity Framework Core DbContext for the app's primary data.
    /// </summary>
    public class CCSDbContext : DbContext
    {
        // DbSets (pluralized names of the Entities):
        public DbSet<User> Users { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLineItem> TransactionLineItems { get; set; }
        public DbSet<PantryPackTransaction> PantryPackTransactions { get; set; }
        public DbSet<PantryPackType> PantryPackType { get; set; }

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
            /*** Alternate Keys ***/
            // https://docs.microsoft.com/en-us/ef/core/modeling/alternate-keys
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserName);
            modelBuilder.Entity<Agency>().HasAlternateKey(a => a.Name);
            modelBuilder.Entity<Category>().HasAlternateKey(c => c.Name);
            modelBuilder.Entity<Subcategory>().HasAlternateKey(s => s.Name);
            modelBuilder.Entity<PantryPackType>().HasAlternateKey(t => t.Name);

            /*** Indexes (Explicit) ***/
            modelBuilder.Entity<User>().HasIndex(u => u.UserName);

            /*** Seed Data ***/
            #region seeddata
            DateTime oct24 = DateTime.Parse("2018-10-24T12:03:00-06:00").ToUniversalTime();

            // Add a default user to the table.  Using an anonymous type instead of a User
            // object so the passwordHash and all other data is constant across migrations
            modelBuilder.Entity<User>().HasData(new
            {
                ID = 1,
                FirstName = "Weber",
                LastName = "CS",
                UserName = "skram",
                // This passwordhash is the hash for: M8/iq+W1
                PasswordHash = "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu",
                Role = UserRole.ADMIN,
                Note = "Default user for an empty database",
                Created = DateTime.Parse("2018-10-18T12:30:18.051Z").ToUniversalTime(),
                CreatedBy = "Seeded Data",
                Modified = oct24,
                ModifiedBy = "Seeded Data",
            });

            // Default categories:
            modelBuilder.Entity<Category>().HasData(new Category[]{
                new Category {
                    ID = 1,
                    Name = "Dry Goods",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    ID = 2,
                    Name = "Perishable",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    ID = 3,
                    Name = "Non-Food",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
            });
            // Default subcategories for above categories:
            modelBuilder.Entity<Subcategory>().HasData(new Subcategory[]{
                new Subcategory {
                    ID = 1,
                    CategoryID = 1,
                    Name = "Unsorted (Dry Goods)",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    ID = 2,
                    CategoryID = 2,
                    Name = "Unsorted (Perishable)",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    ID = 3,
                    CategoryID = 3,
                    Name = "Unsorted (Non-Food)",
                    Created = oct24,
                    CreatedBy = "Seeded Data",
                    Modified = oct24,
                    ModifiedBy = "Seeded Data"
                },
            });

            modelBuilder.Entity<PantryPackType>().HasData(new PantryPackType
            {
                ID = 1,
                Name = "Generic",
                Created = oct24,
                CreatedBy = "Seeded Data",
                Modified = oct24,
                ModifiedBy = "Seeded Data"
            });
            #endregion
        }

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling the
        /// base class' SaveChanges() method.  See DbContext.SaveChanges()
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        /// <returns></returns>
        public int SaveChanges(string username = null)
        {
            UpdateModifiedTimestamp(username);
            return base.SaveChanges();
        }

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling
        /// the base class' SaveChangesAsync() method.  See DbContext.SaveChangesAsync()
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(string username = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateModifiedTimestamp(username);
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// This method updates the Created and Modified timestamps for Entities extending from
        /// TrackedModel that are about to be saved.
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        private void UpdateModifiedTimestamp(string username = null)
        {
            var entities = ChangeTracker.Entries().Where(
                x => x.Entity is TrackedModel && (x.State == EntityState.Added || x.State == EntityState.Modified)
            );

            username = string.IsNullOrWhiteSpace(username) ? "Unspecified" : username;

            foreach (var e in entities)
            {
                if (e.State == EntityState.Added)
                {
                    ((TrackedModel)e.Entity).Created = DateTime.UtcNow;
                    ((TrackedModel)e.Entity).CreatedBy = username;
                }
                ((TrackedModel)e.Entity).Modified = DateTime.UtcNow;
                ((TrackedModel)e.Entity).ModifiedBy = username;

            }
        }
    }
}
