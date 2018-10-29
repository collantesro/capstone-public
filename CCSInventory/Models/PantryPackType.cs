using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is used to represent different types of Pantry Packs
    /// </summary>
    public class PantryPackType : TrackedModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
