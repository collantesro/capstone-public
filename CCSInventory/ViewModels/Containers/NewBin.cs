using System;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CCSInventory.ViewModels.Containers
{
    public class NewBin
    {
        [DisplayName("Auto-generate Bin Number")]
        public bool AutoGenerate { get; set; } = true;

        [DisplayName("Bin Number")]
        // Verified in controller.  No [Range] attribute
        public int? BinNumber { get; set; }

        [Required]
        [DisplayName("Subcategory")]
        public int SubcategoryID { get; set; }

        [Range(0, 1_000_000, ErrorMessage = "Weight must be between 0 and 1,000,000.")]
        public decimal Weight { get; set; } = 0m;

        [Range(0, 1_000, ErrorMessage = "Units/Quantity/Cases must be between 0 and 1,000")]
        public int Units { get; set; } = 0;

        [DisplayName("Storage Location")]
        public int LocationID { get; set; }

        [DisplayName("Expiration Date (optional)")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Container Note")]
        public string ContainerNote { get; set; }

        public NewBin() { }

        public NewBin(CCSInventory.Models.Container c)
        {
            this.BinNumber = c.BinNumber;
            this.AutoGenerate = c.BinNumber != 0;
            this.SubcategoryID = c.SubcategoryID;
            this.Weight = c.Weight;
            this.Units = c.Units;
            this.LocationID = c.LocationID;
            this.ContainerNote = c.ContainerNote;
            this.ExpirationDate = c.ExpirationDate;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("NewBin{AutoGenerate=")
              .Append(this.AutoGenerate ? "true, " : "false, ")
              .Append("BinNumber=")
              .Append(this.BinNumber == null ? "null" : this.BinNumber.Value.ToString())
                .Append(", ")
              .Append("SubcategoryID=")
              .Append(this.SubcategoryID.ToString())
                .Append(", ")
              .Append("LocationID=")
              .Append(this.LocationID.ToString())
                .Append(", ")
              .Append("Weight=")
              .Append(this.Weight.ToString("F5"))
                .Append(", ")
              .Append("Units=")
              .Append(this.Units.ToString())
                .Append(", ")
              .Append("ExpirationDate=")
              .Append(this.ExpirationDate == null ? "null" : this.ExpirationDate.Value.ToString())
                .Append(", ")
              .Append("ContainerNote=")
              .Append(this.ContainerNote)
              .Append("}");
            return sb.ToString();
        }
    }
}
