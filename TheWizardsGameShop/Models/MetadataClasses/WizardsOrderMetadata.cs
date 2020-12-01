using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop.Models
{
    [ModelMetadataType(typeof(WizardsOrderMetadata))]
    public partial class WizardsOrder
    {
    }
    public class WizardsOrderMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Credit card")]
        public int CreditCardId { get; set; }

        [Display(Name = "Mailing address")]
        public int MailingAddressId { get; set; }

        [Display(Name = "Shipping address")]
        public int ShippingAddressId { get; set; }

        [Display(Name = "Status")]
        public int OrderStatusId { get; set; }

        public virtual CreditCard CreditCard { get; set; }
        public virtual Address MailingAddress { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual WizardsUser User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
