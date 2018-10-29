using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Address : TrackedModel
    {
        public int ID { get; set; }

        [Required]
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }
        public string Note { get; set; }
    }
}
