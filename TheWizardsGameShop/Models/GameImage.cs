using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class GameImage
    {
        public int GameImageId { get; set; }
        public int GameId { get; set; }
        public string GameImagePath { get; set; }

        public virtual Game Game { get; set; }
    }
}
