using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string ProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public int AddressTypeId { get; set; }

        public virtual AddressType AddressType { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual WizardsUser User { get; set; }
    }
}
