using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Donor : TrackedModel
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public int TypeId { get; set; }
        public int AddressId { get; set; }
        public int MailingAddressId { get; set; }
        public string Note { get; set; }
        public bool Archived { get; set; }
    }
}
