using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.ViewModels.Admin
{
    public class UserAdd : UserEdit
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 20 characters long.")]
        [RegularExpression(@"[a-z0-9_]{4,20}", ErrorMessage = "Username may only contain lowercase letters, numbers, and underscore.")]
        public new string Username { get; set; }

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
