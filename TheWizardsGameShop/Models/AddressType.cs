using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class AddressType
    {
        public AddressType()
        {
            Address = new HashSet<Address>();
        }

        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }
}
