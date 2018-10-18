using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

// https://crackstation.net/hashing-security.htm : Why we hash and salt passwords

namespace CCSInventory.Models
{

    // This shouldn't be bound to forms.  This class is for the DB and internal logic.
    // Use the ViewModels for UserLogin and UserCreate instead.
    [BindNever]
    public class User : TrackedModel
    {
        public long ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string PasswordHash { get; private set; }
        public string Note { get; set; }
        public UserRole Role { get; set; } = UserRole.DISABLED;

        public bool MatchesPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, this.PasswordHash);
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

    public enum PasswordChangeResult
    {
        PASSWORD_FAILURE,
        PASSWORD_OK,
    }

    public enum UserRole
    {
        DISABLED,
        READONLY,
        STANDARD,
        ADMIN,
    }
}
