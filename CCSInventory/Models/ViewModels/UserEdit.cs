using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models.ViewModels {
    public class UserEdit {
        [DisplayName("User Name")]
        public string UserName { get; set; }

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

        public string Note { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public UserEdit(){}

        public UserEdit(User user){
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Note = user.Note;
            Role = user.Role;
        }
    }
}
