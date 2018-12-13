using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CCSInventory.Models.Reports;
using CCSInventory.ViewModels;

namespace CCSInventory.Models.Reports
{
    /// <summary>
    /// This class represents the options for running a single report for Containers.  It extends from a template,
    /// since it just adds the Start and End dates.
    /// </summary>
    public class ContainerOptions : ContainerTemplate
    {
        [Required, DataType(DataType.Date), DisplayName("Start Date")]
        public DateTime Start { get; set; }

        [Required, DataType(DataType.Date), DisplayName("End Date")]
        public DateTime End { get; set; }

        [DisplayName("Generate Excel File Instead")]
        public bool ExcelFormat { get; set; }

        public ContainerOptions()
        {
            Initialize();
        }

        public ContainerOptions(ContainerTemplate template) : base(template)
        {
            Initialize();
        }

        private void Initialize()
        {
            Start = DateTime.Today;
            End = DateTime.Today;
            ExcelFormat = false;
        }

        public void SetDates(DateRange dateRange)
        {
            Start = dateRange.Start;
            End = dateRange.End;
        }

        public override string ToString()
        {
            return $"ContainerOptions={{Start={Start}, End={End}, ExcelFormat={ExcelFormat}, {base.ToString()}}}";
        }
    }
}
