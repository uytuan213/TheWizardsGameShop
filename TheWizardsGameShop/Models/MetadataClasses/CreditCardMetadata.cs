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
                yield return new ValidationResult("Credit card number invalid");
            }

            if (!string.IsNullOrEmpty(ExpiryDate))
            {
                var dateDigits = ExpiryDate.Replace("/", "");
                if (dateDigits.Length == 4 && ValidationHelper.IsNumeric(dateDigits))
                {
                    var month = int.Parse(dateDigits.Substring(0, 2));
                    var year = int.Parse("20" + dateDigits.Substring(2, 2));

                    if (month >= 1 && month <= 12 && year >= 2000 && year <= 2099)
                    {
                        // Set the exp date to the last day of month
                        var expDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                        if (expDate < DateTime.Today)
                        {
                            yield return new ValidationResult("Credit card expired");
                        }
                    }
                    else
                    {
                        yield return new ValidationResult("Expiry date invalid");
                    }
                }
                else
                {
                    yield return new ValidationResult("Expiry date invalid");
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

        public virtual WizardsUser User { get; set; }
    }
}
