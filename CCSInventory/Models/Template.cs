using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Template : TrackedModel
    {
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public DateTime TemplateDate { get; set; }
        public TemplateType TemplateType { get; set; }
    }

    public enum TemplateType{
        DEFAULT,
    }
}
