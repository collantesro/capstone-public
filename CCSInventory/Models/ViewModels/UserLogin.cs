using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models.ViewModels
{
    public class UserLogin
    {
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
