using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class Users
    {
        public Users()
        {
            Address = new HashSet<Address>();
            CreditCard = new HashSet<CreditCard>();
            RelationshipUserId1Navigation = new HashSet<Relationship>();
            RelationshipUserId2Navigation = new HashSet<Relationship>();
            UserRole = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool ReceivePromotionalEmails { get; set; }

        public virtual Gender GenderNavigation { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CreditCard> CreditCard { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId1Navigation { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId2Navigation { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
