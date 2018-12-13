using CCSInventory.Models;
using CCSInventory.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSInventory.ViewModels.Reports
{
    public class IncomingOptions : IncomingTemplate
    {
        private CCSDbContext context;

        [Required, DataType(DataType.Date), DisplayName("Start Date")]
        public DateTime Start { get; set; }

        [Required, DataType(DataType.Date), DisplayName("End Date")]
        public DateTime End { get; set; }

        public CCSDbContext Context
        {
            get
            {
                return this.context;
            }
            set
            {
                if (this.context != value)
                {
                    this.context = value;
                    this.CategoryList = this.context.Categories;
                }
            }
        }

        public IQueryable<Category> CategoryList { get; set; }

        [DisplayName("Generate CSV File Instead")]
        public bool CSVDesired { get; set; }

        public IncomingOptions()
        {
            Initialize();
        }

        public IncomingOptions(IncomingTemplate template) : base(template)
        {
            Initialize();
        }

        private void Initialize()
        {
            Start = DateTime.Today;
            End = DateTime.Today;
            CSVDesired = false;
        }

        public override string ToString()
        {
            return $"ContainerOptions={{Start={this.Start}, End={this.End}, Category={this.CategoryID.ToString()}, CSVDesired={this.CSVDesired}, {base.ToString()}}}";
        }
    }
}
