using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    public class TestWizardsUser
    {
        private const int USERNAME_MIN_LENGTH = 5;
        private const int USERNAME_MAX_LENGTH = 20;
        private const int PASSWORD_MIN_LENGTH = 8;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name ="Username")]
        [Required]
        [MinLength(USERNAME_MIN_LENGTH, ErrorMessage = "Username must be at least 5 characters.")]
        [MaxLength(USERNAME_MAX_LENGTH, ErrorMessage = "Username must be 20 characters max.")]
        [RegularExpression(@"^(?=[a-zA-Z0-9._]{5,20}$)(?!.*[_.]{2})[^_.].*[^_.]$", ErrorMessage = "Username must have 5-20 characters, and have no space/special character")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [RegularExpression(@"^(.{0,7}|(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&]*)[a-zA-Z\d@$!%*#?&]{8,})$", ErrorMessage = "Password must contain at least one number, one lowercase and one uppercase letter.")]
        [MinLength(PASSWORD_MIN_LENGTH, ErrorMessage = "Password must be at least 8 characters.")]
        public string PasswordHash { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Date of birth")]
        [Required]
        public DateTime? Dob { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        [Required]
        [RegularExpression (@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone number invalid!")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [RegularExpression (@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "I'd like to receive promotional emails")]
        public bool ReceivePromotionalEmails { get; set; }
    }
}
