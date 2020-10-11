using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<Users>();
        }

        public string Gender1 { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
