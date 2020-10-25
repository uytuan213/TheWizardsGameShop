using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class FavoriteCategory
    {
        public int UserId { get; set; }
        public short GameCategoryId { get; set; }

        public virtual GameCategory GameCategory { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
