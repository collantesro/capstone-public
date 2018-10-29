using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Template : TrackedModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Data { get; set; }
        public TemplateType Type { get; set; }
    }

    public enum TemplateType
    {
        DEFAULT,
    }
}
