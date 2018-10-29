using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models.ViewModels
{
    /// <summary>
    /// This ViewModel is used in the Login Razor Page to bind to the login form.
    /// </summary>
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
