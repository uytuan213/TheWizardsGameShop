using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop
{
    public class CartItem
    {
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
