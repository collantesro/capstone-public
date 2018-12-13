using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    /// <summary>
    /// This model represents an object serialized as JSON for a saved report template.
    /// A report template is used by the report controllers.  The user is allowed to make
    /// templates to periodically run the same report over time.
    /// </summary>
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
        Unspecified, // This should be unused.  It's just there to be the value for 0.
        Containers,
        Incoming,
        Outgoing,
        InOut,
    }
}
