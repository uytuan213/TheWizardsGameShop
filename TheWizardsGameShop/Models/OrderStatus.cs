using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            WizardsOrder = new HashSet<WizardsOrder>();
        }

        public int OrderStatusId { get; set; }
        public string OrderStatus1 { get; set; }

        public virtual ICollection<WizardsOrder> WizardsOrder { get; set; }
    }
}
