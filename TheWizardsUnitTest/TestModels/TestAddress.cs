using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{   
    public class TestAddress
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
    }
}
