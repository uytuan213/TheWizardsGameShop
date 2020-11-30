using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int GameId { get; set; }
        public int Quantity { get; set; }
        public bool IsDigital { get; set; }

        public virtual Game Game { get; set; }
        public virtual WizardsOrder Order { get; set; }
    }
}
