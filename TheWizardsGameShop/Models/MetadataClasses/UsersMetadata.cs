using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    [ModelMetadataType(typeof(UsersMetadata))]
    public partial class Users : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TheWizardsGameShopContext _context = new TheWizardsGameShopContext();

            if (!string.IsNullOrEmpty(Email))
            {
                if (!ValidationHelper.EmailValidation(Email))
                {
                    yield return new ValidationResult("Email invalid!");
                }
            }
        }

        public static string PostalCodeFormat(string value)
        {
            if (value == null)
                return null;
            if (value.Trim().Length == 6)
            {
                value = value.Insert(3, " ");
            }
            return value.ToUpper();
        }
    }

    public class UsersMetadata
    {
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
