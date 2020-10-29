using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    [ModelMetadataType(typeof(CreditCardMetadata))]

    public partial class CreditCard : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsCreditCardValid(CreditCardNumber))
            {
                yield return new ValidationResult("Credit card number invalid!");
            }

            if (!string.IsNullOrEmpty(ExpiryDate))
            {
                var expDate = DateTime.Parse(ExpiryDate);

                // Set the exp date to the last day of month
                expDate = new DateTime(expDate.Year, expDate.Month, DateTime.DaysInMonth(expDate.Year, expDate.Month));
                if (expDate < DateTime.Today)
                {
                    yield return new ValidationResult("The credit card is expired!");
                }
            }

            yield return  ValidationResult.Success;
        }

        public static bool IsCreditCardValid(string cardNumber)
        {
            int i, checkSum = 0;

            // Compute checksum of every other digit starting from right-most digit
            for (i = cardNumber.Length - 1; i >= 0; i -= 2)
                checkSum += (cardNumber[i] - '0');

            // Now take digits not included in first checksum, multiple by two,
            // and compute checksum of resulting digits
            for (i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            // Number is valid if sum of both checksums MOD 10 equals 0
            return ((checkSum % 10) == 0);
        }
    }

    public class CreditCardMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CreditCardId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Credit card number")]
        [Required]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Expiry data")]
        [Required]
        public string ExpiryDate { get; set; }

        [Display(Name = "CVC")]
        [Required]
        [MaxLength(4, ErrorMessage = "Maximun 4 digits")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVC must be 3 or 4 digit")]
        public string Cvc { get; set; }

        [Display(Name = "Card holder")]
        [Required]
        public string CardHolder { get; set; }

        public virtual WizardsUser User { get; set; }
    }
}
