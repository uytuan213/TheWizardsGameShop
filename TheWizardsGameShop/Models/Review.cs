using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string ReviewContent { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsPublished { get; set; }

        public virtual Game Game { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
