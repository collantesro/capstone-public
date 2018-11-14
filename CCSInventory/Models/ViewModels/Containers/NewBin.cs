using System;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CCSInventory.Models.ViewModels.Containers
{
    public class NewBin
    {
        [DisplayName("Auto-generate Bin Number")]
        public bool AutoGenerate { get; set; } = true;

        [DisplayName("USDA")]
        public bool IsUSDA { get; set; }

        [DisplayName("Bin Number")]
        [Range(1000, 9999, ErrorMessage = "Bin Number must be between 1000 and 9999")]
        public int? BinNumber { get; set; } = 0;

        [Required]
        [DisplayName("Subcategory")]
        public int SubcategoryID { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Weight { get; set; } = 0m;

        [Range(0, int.MaxValue, ErrorMessage = "Cases must be greater than 0")]
        public int Cases { get; set; } = 0;

        public string Location { get; set; }

        [DisplayName("Expiration Date (optional)")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Container Note")]
        public string ContainerNote { get; set; }

        public NewBin(){}

        public NewBin(Container c){
            this.IsUSDA = c.IsUSDA;
            this.BinNumber = c.BinNumber;
            this.AutoGenerate = c.BinNumber != 0;
            this.SubcategoryID = c.SubcategoryID;
            this.Weight = c.Weight;
            this.Cases = c.Cases;
            this.Location = c.Location;
            this.ContainerNote = c.ContainerNote;
        }
    }
}
