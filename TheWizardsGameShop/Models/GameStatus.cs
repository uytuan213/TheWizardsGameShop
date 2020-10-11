using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class GameStatus
    {
        public GameStatus()
        {
            Game = new HashSet<Game>();
        }

        public string GameStatusCode { get; set; }
        public string GameStatus1 { get; set; }

        public virtual ICollection<Game> Game { get; set; }
    }
}
