using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Agency : TrackedModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int AddressID { get; set; }
        public int? MailingAddressID { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public bool IsArchived { get; set; } // For "Deleted".  Hidden from view

        // Navigation Properties for the above Foreign Keys
        public Address Address { get; set; }
        public Address MailingAddress { get; set; }
    }
}