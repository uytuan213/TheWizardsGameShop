using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Rating
    {
        public double Rate { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }

        public virtual Game Game { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
