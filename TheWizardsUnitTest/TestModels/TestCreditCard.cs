using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    public class TestCreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CreditCardId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Card number")]
        [Required]
        [MaxLength(16)]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Card number must be exactly 16 digits")]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Expiration date")]
        [Required]
        [RegularExpression(@"^(1[0-2]|0[1-9])\/?\d\d$", ErrorMessage = "Expiry date invalid")]
        public string ExpiryDate { get; set; }

        [Display(Name = "Security code")]
        [Required]
        [MaxLength(4)]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVC must be 3 or 4 digits")]
        public string Cvc { get; set; }

        [Display(Name = "Cardholder name")]
        [Required]
        public string CardHolder { get; set; }
    }
}
