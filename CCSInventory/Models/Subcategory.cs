using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This represents a subcategory.  Such as Category = Perishable, Subcategory (this) = Dairy
    /// </summary>
    public class Subcategory : TrackedModel
    {
        public int SubcategoryID { get; set; }
        public int CategoryID { get; set; }

        [Required]
        public string SubcategoryName { get; set; }
        public string SubcategoryNote { get; set; }

        // Navigation Properties for the above Foreign Key
        public Category Category { get; set; }
    }
}
