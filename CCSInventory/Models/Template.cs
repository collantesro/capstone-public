using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Template : TrackedModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TemplateType Type { get; set; }
    }

    public enum TemplateType
    {
        DEFAULT,
    }
}
