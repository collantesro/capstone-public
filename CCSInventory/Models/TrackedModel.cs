using System;

namespace CCSInventory.Models {
    public class TrackedModel {
        public DateTime Created {get; set;}
        public DateTime Modified {get; set;}
        public string CreatedBy {get; set;}
        public string ModifiedBy {get; set;}
    }
}
