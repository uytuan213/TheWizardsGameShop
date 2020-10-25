using System;
using System.Collections.Generic;

namespace TheWizardsGameShop.Models
{
    public partial class WizardsUser
    {
        public WizardsUser()
        {
            Address = new HashSet<Address>();
            CreditCard = new HashSet<CreditCard>();
            FavoriteCategory = new HashSet<FavoriteCategory>();
            FavoritePlatform = new HashSet<FavoritePlatform>();
            Rating = new HashSet<Rating>();
            RelationshipUserId1Navigation = new HashSet<Relationship>();
            RelationshipUserId2Navigation = new HashSet<Relationship>();
            Review = new HashSet<Review>();
            UserRole = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public DateTime? Dob { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool ReceivePromotionalEmails { get; set; }

        public virtual Gender GenderNavigation { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CreditCard> CreditCard { get; set; }
        public virtual ICollection<FavoriteCategory> FavoriteCategory { get; set; }
        public virtual ICollection<FavoritePlatform> FavoritePlatform { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId1Navigation { get; set; }
        public virtual ICollection<Relationship> RelationshipUserId2Navigation { get; set; }
        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
