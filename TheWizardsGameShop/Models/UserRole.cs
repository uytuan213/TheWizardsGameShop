﻿using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual WizardsRole Role { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
