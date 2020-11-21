using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class CreditCard
    {
        public CreditCard()
        {
            WizardsOrder = new HashSet<WizardsOrder>();
        }

        public int CreditCardId { get; set; }
        public int UserId { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CardHolder { get; set; }
        public string Cvc { get; set; }

        public virtual WizardsUser User { get; set; }
        public virtual ICollection<WizardsOrder> WizardsOrder { get; set; }
    }
}
