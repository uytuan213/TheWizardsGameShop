using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Relationship
    {
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public bool? IsAccepted { get; set; }
        public bool IsFamily { get; set; }

        public virtual WizardsUser ReceiverNavigation { get; set; }
        public virtual WizardsUser SenderNavigation { get; set; }
    }
}
