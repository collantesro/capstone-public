using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is used to represent different types of Pantry Packs
    /// </summary>
    public class PantryPackType : TrackedModel
    {
        public int PantryPackTypeID { get; set; }

        [Required]
        public string PantryPackTypeName { get; set; }
        public string PantryPackTypeNote { get; set; }
    }
}
