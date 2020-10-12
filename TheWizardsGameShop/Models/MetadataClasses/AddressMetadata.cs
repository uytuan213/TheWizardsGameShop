using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    [ModelMetadataType(typeof(AddressMetadata))]
    public partial class Address : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ValidationHelper.PostalCodeValidation(PostalCode))
            {
                yield return new ValidationResult("Postal code invalid!");
            }
            else
            {
                PostalCode = PostalCodeFormat(PostalCode);
            }

            yield return ValidationResult.Success;
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
    public class AddressMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        public int UserId { get; set; }

        [Display(Name ="Street 1")]
        [Required]
        public string Street1 { get; set; }

        [Display(Name = "Street 2")]
        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ProvinceCode { get; set; }

        [Display(Name = "Postal code")]
        [Required]
        public string PostalCode { get; set; }

        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual Users User { get; set; }
    }
}
