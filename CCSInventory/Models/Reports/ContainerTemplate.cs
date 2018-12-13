using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CCSInventory.Models.Reports
{
    /// <summary>
    ///  This Template represents saved options for a Container report.
    /// </summary>
    public class ContainerTemplate
    {
        [DisplayName("Report/Template Name")]
        public string ReportName { get; set; }

        [DisplayName("Expiration Start Date"), DataType(DataType.Date)]
        public DateTime? ExpirationStart { get; set; }

        [DisplayName("Expiration End Date"), DataType(DataType.Date)]
        public DateTime? ExpirationEnd { get; set; }

        public List<int> CategoryIDs { get; set; }
        public List<int> SubcategoryIDs { get; set; }
        public List<int> LocationIDs { get; set; }

        [DisplayName("Weight - Lower Bound")]
        [Range(0, 1_000_000, ErrorMessage = "Weight lower bound must be between 0 and 1,000,000")]
        public decimal WeightLowerBound { get; set; }

        [DisplayName("Weight - Upper Bound")]
        [Range(0, 1_000_000, ErrorMessage = "Weight upper bound must be between 0 and 1,000,000")]
        public decimal WeightUpperBound { get; set; }

        [DisplayName("Units/Cases/Qty - Lower Bound")]
        [Range(0, 1_000, ErrorMessage = "Units lower bound must be between 0 and 1,000")]
        public int UnitsLowerBound { get; set; }

        [DisplayName("Units/Cases/Qty - Upper Bound")]
        [Range(0, 1_000, ErrorMessage = "Units upper bound must be between 0 and 1,000")]
        public int UnitsUpperBound { get; set; }

        public bool IncludeArchived { get; set; }

        public ContainerTemplate()
        {
            Initialize();
        }

        public ContainerTemplate(ContainerTemplate template)
        {
            if (template != null)
            {
                ReportName = template.ReportName;
                ExpirationStart = template.ExpirationStart;
                ExpirationEnd = template.ExpirationEnd;
                CategoryIDs = new List<int>(template.CategoryIDs);
                SubcategoryIDs = new List<int>(template.SubcategoryIDs);
                LocationIDs = new List<int>(template.LocationIDs);
                WeightLowerBound = template.WeightLowerBound;
                WeightUpperBound = template.WeightUpperBound;
                UnitsLowerBound = template.UnitsLowerBound;
                UnitsUpperBound = template.UnitsUpperBound;
                IncludeArchived = template.IncludeArchived;
            }
            else
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            ReportName = $"Unnamed Report {DateTime.Today.ToString("yyyy-MM-dd")}";
            CategoryIDs = new List<int>();
            SubcategoryIDs = new List<int>();
            LocationIDs = new List<int>();
            WeightLowerBound = 0;
            WeightUpperBound = 1_000_000;
            UnitsLowerBound = 0;
            UnitsUpperBound = 1_000;
            IncludeArchived = false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ContainerTemplate{")
              .Append("ReportName=")
                .Append(ReportName)
                  .Append(", ")
              .Append("ExpirationStart=")
                .Append(ExpirationStart == null ? "null" : ExpirationStart.Value.ToString())
                  .Append(", ")
              .Append("ExpirationEnd=")
                .Append(ExpirationEnd == null ? "null" : ExpirationEnd.Value.ToString())
                  .Append(", ")
              .Append("CategoryIDs={");
            foreach (int i in CategoryIDs)
            {
                sb.Append($"{i},");
            }
            sb.Append("}, SubcategoryIDs={");
            foreach (int i in SubcategoryIDs)
            {
                sb.Append($"{i},");
            }
            sb.Append("}, LocationIDs={");
            foreach (int i in SubcategoryIDs)
            {
                sb.Append($"{i},");
            }
            sb.Append("}}, ")
              .Append("WeightLowerBound=").Append(WeightLowerBound).Append(", ")
              .Append("WeightUpperBound=").Append(WeightUpperBound).Append(", ")
              .Append("UnitsLowerBound=").Append(UnitsLowerBound).Append(", ")
              .Append("UnitsUpperBound=").Append(UnitsUpperBound).Append(", ")
              .Append("IncludeArchived=").Append(IncludeArchived)
              .Append("}");

            return sb.ToString();
        }
    }
}
