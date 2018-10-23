using System;

namespace CCSInventory.Models
{
    /// <summary>
    /// Base class for Entity models that require tracking of created by and modification.
    /// </summary>
    public abstract class TrackedModel
    {
        /// <summary>
        /// DateTime of when the object was first added to the DbSet
        /// </summary>
        /// <value></value>
        public DateTime Created { get; set; }
        /// <summary>
        /// DateTime of when the object was last changed in the DbSet
        /// </summary>
        /// <value></value>
        public DateTime Modified { get; set; }
        /// <summary>
        /// String describing the code or user that created the object.
        /// </summary>
        /// <value></value>
        public string CreatedBy { get; set; } = "Unspecified";
        /// <summary>
        /// String describing the code or user that modified the object.
        /// </summary>
        /// <value></value>
        public string ModifiedBy { get; set; } = "Unspecified";


        // For the Seed Data in CCSDbContext for the first user, the Created and Modified
        // fields were set to "Seeded Data", as there is no proper user that created it.
    }
}
