using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Game
    {
        public Game()
        {
            GameImage = new HashSet<GameImage>();
        }

        public int GameId { get; set; }
        public string GameStatusCode { get; set; }
        public short GameCategoryId { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public decimal GamePrice { get; set; }
        public short GameQty { get; set; }
        public string GameDigitalPath { get; set; }

        public virtual GameCategory GameCategory { get; set; }
        public virtual GameStatus GameStatusCodeNavigation { get; set; }
        public virtual ICollection<GameImage> GameImage { get; set; }
    }
}
