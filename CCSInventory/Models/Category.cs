using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class represents a broad category for donations or outgoing.  Examples: Dry Goods, Perishable, Non-Food
    /// </summary>
    public class Category : TrackedModel
    {
        public int CategoryID { get; set; }
        
        [Required]
        public string CategoryName { get; set; }
        public string CategoryNote { get; set; }

        // Navigation Property for EF Core
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
