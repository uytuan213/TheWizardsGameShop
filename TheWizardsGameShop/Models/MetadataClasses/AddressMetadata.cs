﻿using Microsoft.AspNetCore.Mvc;
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

        [Display(Name ="Street Line 1")]
        [Required]
        public string Street1 { get; set; }

        [Display(Name = "Street Line 2")]
        public string Street2 { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Province")]
        [Required]
        public string ProvinceCode { get; set; }

        [Display(Name = "Postal code")]
        [Required]
        [RegularExpression(@"^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy])\d([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]) ?\d([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz])\d$", 
            ErrorMessage = "Postal code invalid!")]
        public string PostalCode { get; set; }

        [Display(Name = "Address type")]
        [Required]
        public int AddressTypeId { get; set; }

        public virtual AddressType AddressType { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
