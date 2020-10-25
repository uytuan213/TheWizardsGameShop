using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class FavoritePlatform
    {
        public int UserId { get; set; }
        public short PlatformId { get; set; }

        public virtual Platform Platform { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
