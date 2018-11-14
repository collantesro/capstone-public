using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CCSInventory.Models.ViewModels.PantryPack
{
    public class EditPantryPackType : TrackedModel
    {
        public int PantryPackTypeID { get; set; }

        //[Required]
        //public string OldPantryPackTypeNote { get; set; }

        public string NewPantryPackTypeName { get; set; }
        public string NewPantryPackTypeNote { get; set; }
    }
}
