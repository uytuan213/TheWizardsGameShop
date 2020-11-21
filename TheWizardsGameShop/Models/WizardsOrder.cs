using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class WizardsOrder
    {
        public WizardsOrder()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public double Total { get; set; }
        public int CreditCardId { get; set; }
        public int MailingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public int OrderStatusId { get; set; }

        public virtual CreditCard CreditCard { get; set; }
        public virtual Address MailingAddress { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual WizardsUser User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
