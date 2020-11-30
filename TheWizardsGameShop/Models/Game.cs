using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Game
    {
        public Game()
        {
            GameImage = new HashSet<GameImage>();
            OrderDetail = new HashSet<OrderDetail>();
            Rating = new HashSet<Rating>();
            Review = new HashSet<Review>();
        }

        public int GameId { get; set; }
        public string GameStatusCode { get; set; }
        public short GameCategoryId { get; set; }
        public short GamePlatformId { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public decimal GamePrice { get; set; }
        public short GameQty { get; set; }
        public string GameDigitalPath { get; set; }

        public virtual GameCategory GameCategory { get; set; }
        public virtual Platform GamePlatform { get; set; }
        public virtual GameStatus GameStatusCodeNavigation { get; set; }
        public virtual ICollection<GameImage> GameImage { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
