using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Template : TrackedModel
    {
        public int TemplateID { get; set; }
        [Required]
        public string TemplateName { get; set; }
        
        [Required]
        public string TemplateData { get; set; }
        public TemplateType TemplateType { get; set; }
    }

    public enum TemplateType
    {
        DEFAULT,
    }
}
