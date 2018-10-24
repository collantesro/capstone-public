using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CCSInventory.Models
{
    public class Container : TrackedModel 
    // Extend from TrackedModel (Also singularly named, "Containers" => "Container")
    {
        public int Id { get; set; }
        public int BinNumber { get; set; }
        public int USDACategoryId { get; set; }
        public int TypeId { get; set; }
        public bool USDA { get; set; }
        public decimal Weight { get; set; }
        public int Cases { get; set; }

        // This means you don't specify Created, Modified, CreatedBy, and ModifiedBy.
        // CCSDbContext will automatically handle values for Created and Modified as long as you extend from TrackedModel.
    }
}
