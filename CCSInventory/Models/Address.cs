using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CCSInventory.Models
{
    public class Address : TrackedModel
    {
        public int AddressID { get; set; }

        [Required]
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }
        public string AddressNote { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(StreetAddress1);
            if (!string.IsNullOrEmpty(StreetAddress2))
            {
                sb.Append(' ').Append(StreetAddress2);
            }
            sb.Append(", ").Append(City).Append(", ").Append(State).Append(" ").Append(Zip);
            return sb.ToString();
        }
    }
}
