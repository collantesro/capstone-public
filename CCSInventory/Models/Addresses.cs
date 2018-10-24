using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Address : TrackedModel
    {
        public int Id { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

    }
}
