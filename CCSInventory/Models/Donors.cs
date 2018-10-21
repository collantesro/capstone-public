using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Donors
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public long AddressId { get; set; }
        public long MailingAddressId { get; set; }
        public string Note { get; set; }
        public bool Archived { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
