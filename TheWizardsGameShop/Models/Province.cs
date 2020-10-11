using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Province
    {
        public Province()
        {
            Address = new HashSet<Address>();
        }

        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }
}
