using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Relationship
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public bool IsFamily { get; set; }

        public virtual WizardsUser UserId1Navigation { get; set; }
        public virtual WizardsUser UserId2Navigation { get; set; }
    }
}
