using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CCSInventory.Models;

namespace CCSInventory.ViewModels.Admin
{
    public class UserEdit
    {

        public string Username { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Change Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [DisplayName("User Note")]
        public string UserNote { get; set; }

        [Required]
        [DisplayName("User Role")]
        public UserRole UserRole { get; set; }

        public UserEdit() { }

        public UserEdit(User user)
        {
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            UserNote = user.UserNote;
            UserRole = user.UserRole;
        }
    }
}
