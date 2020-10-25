using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class GameCategory
    {
        public GameCategory()
        {
            FavoriteCategory = new HashSet<FavoriteCategory>();
            Game = new HashSet<Game>();
        }

        public short GameCategoryId { get; set; }
        public string GameCategory1 { get; set; }

        public virtual ICollection<FavoriteCategory> FavoriteCategory { get; set; }
        public virtual ICollection<Game> Game { get; set; }
    }
}
