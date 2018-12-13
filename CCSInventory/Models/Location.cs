using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Location : TrackedModel
    {
        public int LocationID { get; set; }
        [Required]
        public string LocationName { get; set; }
        public string LocationNote { get; set; }

        public ICollection<Container> Containers;
    }
}
