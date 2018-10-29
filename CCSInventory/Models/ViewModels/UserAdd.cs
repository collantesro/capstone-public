using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models.ViewModels
{
    public class UserAdd : UserEdit
    {
        [Required]
        [DisplayName("User Name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "User Name must be between 4 and 20 characters long.")]
        [RegularExpression(@"[a-z0-9_]{4,20}", ErrorMessage = "User Name may only contain lowercase letters, numbers, and underscore.")]
        public new string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 64 characters long")]
        public new string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DisplayName("Confirm Password")]
        public new string ConfirmPassword { get; set; }
    }
}
