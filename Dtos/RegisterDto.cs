using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckListAPI.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 25 characters")]
        public string? Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}