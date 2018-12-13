using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Agency : TrackedModel
    {
        public int AgencyID { get; set; }
        [Required]
        public string AgencyName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public int? AddressID { get; set; }
        public bool HasAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string AgencyNote { get; set; }
        public bool IsArchived { get; set; } // For "Deleted".  Hidden from view

        // Navigation Properties for the above Foreign Key
        public Address Address { get; set; }
    }
}
