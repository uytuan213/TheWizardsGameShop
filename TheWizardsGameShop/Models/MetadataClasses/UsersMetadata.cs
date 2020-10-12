﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    [ModelMetadataType(typeof(UsersMetadata))]
    public partial class Users : IValidatableObject
    {
        private const int PASSWORD_MIN_LENGTH = 8;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var password = System.Text.Encoding.UTF8.GetString(PasswordHash);

            if (!string.IsNullOrEmpty(Email))
            {
                if (!ValidationHelper.EmailValidation(Email))
                {
                    yield return new ValidationResult("Email invalid!");
                }
            }

            if (password.Length < 8)
            {
                yield return new ValidationResult("The minimum length of the password is 8 characters");
            }

            if (!ValidationHelper.PasswordValidation(password))
            {
                yield return new ValidationResult("Password has to at least contain a number and a capital character!");
            }

            yield return ValidationResult.Success;
        }
    }

    public class UsersMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name ="Username")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required]
        public byte[] PasswordHash { get; set; }

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
        [RegularExpression (@"\D*([2-9]\d{2})(\D*)([2-9]\d{2})(\D*)(\d{4})\D*", ErrorMessage = "Phone number invalid!")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [RegularExpression (@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email")]
        [Required]
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool ReceivePromotionalEmails { get; set; }

        public virtual Gender GenderNavigation { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CreditCard> CreditCard { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId1Navigation { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId2Navigation { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
