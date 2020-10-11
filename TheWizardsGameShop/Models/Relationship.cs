using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Relationship
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public bool IsFamily { get; set; }

        public virtual Users UserId1Navigation { get; set; }
        public virtual Users UserId2Navigation { get; set; }
    }
}
