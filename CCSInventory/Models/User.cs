using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

// https://crackstation.net/hashing-security.htm : Why we hash and salt passwords

namespace CCSInventory.Models
{

    // This shouldn't be bound to forms.  This class is for the DbContext and app logic.
    // Use ViewModels instead for specific parts of the User.  Like UserLogin for a login form.
    [BindNever]
    public class User : TrackedModel
    {
        public long ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }
        public string FullNameLastFirst { get => $"{LastName}, {FirstName}"; }

        [Required]
        public string UserName { get; set; }

        // The Email is informational and optional.  It's not used for authentication.
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// BCrypt digest of the salted and hashed password.  Check with MatchesPassword()
        /// </summary>
        /// <value></value>
        public string PasswordHash { get; private set; }
        public string Note { get; set; }
        public UserRole Role { get; set; } = UserRole.DISABLED;

        /// <summary>
        /// Compares the password argument with the PasswordHash using BCrypt.
        /// </summary>
        /// <param name="password">string value of a cleartext password to compare with the hash</param>
        /// <returns>Returns a boolean indicating true of provides password matches the hashed password, but false if not.</returns>
        public bool MatchesPassword(string password)
        {
            return !String.IsNullOrEmpty(this.PasswordHash) && 
                BCrypt.Net.BCrypt.EnhancedVerify(password, this.PasswordHash);
        }

        public PasswordChangeResult ChangePassword(string newPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
            {
                return PasswordChangeResult.PASSWORD_FAILURE;
            }
            else
            {
                // Add constraints here for password complexity
                this.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, workFactor: 10);
                return PasswordChangeResult.PASSWORD_OK;
            }
        }
    }

    /// <summary>
    /// The values of PasswordChangeResult are used to describe whether the password 
    /// was successfully changed or not.
    /// </summary>
    public enum PasswordChangeResult
    {
        PASSWORD_FAILURE,
        PASSWORD_OK,
    }

    // For a more detailed description on the UserRoles, refer to /docs/UserRoles.md (relative to repository, not this project)
    /// <summary>
    /// UserRole is used to indicate the broad classes of permissions for users.
    /// </summary>
    public enum UserRole
    {
        DISABLED,
        READONLY,
        STANDARD,
        ADMIN,
    }
}
