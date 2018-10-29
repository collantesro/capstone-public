using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This represents a subcategory.  Such as Category = Perishable, Subcategory (this) = Dairy
    /// </summary>
    public class Subcategory : TrackedModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Note { get; set; }

        // Navigation Properties for the above Foreign Key
        public Category Category { get; set; }
    }
}
