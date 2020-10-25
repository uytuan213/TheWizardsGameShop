using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Platform
    {
        public Platform()
        {
            FavoritePlatform = new HashSet<FavoritePlatform>();
            Game = new HashSet<Game>();
        }

        public short PlatformId { get; set; }
        public string PlatformName { get; set; }

        public virtual ICollection<FavoritePlatform> FavoritePlatform { get; set; }
        public virtual ICollection<Game> Game { get; set; }
    }
}
