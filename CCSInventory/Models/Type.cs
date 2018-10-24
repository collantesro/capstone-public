using System;

namespace CCSInventory.Models
{
    public class Type : TrackedModel
    {
        public int ID { get; set; }
        public int ContainerID { get; set; }
        public string TypeName { get; set; }
        public string Note { get; set; }

        // Navigation Properties for the above Foreign Key
        public Container Container { get; set; }
    }
}
