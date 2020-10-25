using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Gender
    {
        public Gender()
        {
            WizardsUser = new HashSet<WizardsUser>();
        }

        public string Gender1 { get; set; }

        public virtual ICollection<WizardsUser> WizardsUser { get; set; }
    }
}
