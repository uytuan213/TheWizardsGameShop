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
    [ModelMetadataType(typeof(WizardsUserMetadata))]
    public partial class WizardsUser : IValidatableObject
    {
        public const int PASSWORD_MIN_LENGTH = 8;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isEdit = UserId == 0 ? false : true;

            if (!string.IsNullOrEmpty(Email))
            {
                if (!ValidationHelper.EmailValidation(Email))
                {
                    yield return new ValidationResult("Email invalid!");
                }
            }
            if (!isEdit)
            {
                if (string.IsNullOrEmpty(PasswordHash))
                {
                    yield return new ValidationResult("Password cannot be empty");
                }
                var password = PasswordHash;
                if (password.Length < PASSWORD_MIN_LENGTH)
                {
                    yield return new ValidationResult("The minimum length of the password is 8 characters");
                }

                if (!ValidationHelper.PasswordValidation(password))
                {
                    yield return new ValidationResult("Password must contain at least one number, one lowercase and one uppercase letter.");
                }
            }

            yield return ValidationResult.Success;
        }
    }

    public class WizardsUserMetadata
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
        // [Required]
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

        public virtual Gender GenderNavigation { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CreditCard> CreditCard { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId1Navigation { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId2Navigation { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
