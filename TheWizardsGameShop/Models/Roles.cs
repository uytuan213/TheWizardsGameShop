using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Roles
    {
        public Roles()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
