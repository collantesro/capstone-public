using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.ViewModels
{
    public class DateRange
    {
        [Required, DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime End { get; set; }
    }
}
