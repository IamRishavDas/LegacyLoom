using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyLoom.Tests
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [NotNull]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "Length of the user name should be between 4 to 50 characters!")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [StringLength(maximumLength: 250, MinimumLength = 6, ErrorMessage = "Length of the password should be between 6 to 50 characters!")]
        public required string Password { get; set; }

        public Role Role { get; set; } = Role.User;

        public bool IsDeleted { get; set; } = false;

        public override string ToString()
        {
            return $"Id: {this.Id}, Name: {this.Username}, Email: {this.Email}, Role = {this.Role.ToString()}";
        }
    }

    public enum Role
    {
        User, 
        Admin
    }
}
