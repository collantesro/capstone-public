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
    public class IncomingTemplate
    {
        [Required, DisplayName("Food Category")]
        public int[] CategoryID { get; set; }

        [DisplayName("Report/Template Name")]
        public string ReportName { get; set; }

        public IncomingTemplate()
        {
            Initialize();
        }

        public IncomingTemplate(IncomingTemplate template)
        {
            if (template != null)
            {
                this.CategoryID = template.CategoryID;
                this.ReportName = template.ReportName;
            }
            else
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            this.CategoryID = new int[10000];
            this.ReportName = $"Unnamed Report {DateTime.Today.ToString("yyyy-MM-dd")}";
        }

        public override string ToString()
        {
            return $"ContainerOptions={{Category={this.CategoryID.ToString()}, ReportName={this.ReportName}, {base.ToString()}}}";
        }
    }
}

