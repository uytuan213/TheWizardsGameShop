using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TheWizardsGameShop.Models;

namespace TheWizardsUnitTest
{
    public class TheWizardsUnitTestContext : TheWizardsGameShopContext
    {
        public Boolean EFValidation()
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();
            string errors = "";

            foreach (var entry in this.ChangeTracker.Entries()
                .Where(c => c.State == EntityState.Added ||
                            c.State == EntityState.Modified || c.State == EntityState.Deleted))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            errors += $"::: {result.ErrorMessage}";
                        }
                    }
                }
            }

            if (errors != "")
            {
                throw new ValidationException(errors);
            }
            else
            {
                return true;
            }
        }

        public virtual DbSet<TestAddress> TestAddress { get; set; }
        //public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<TestCreditCard> TestCreditCard { get; set; }
        //public virtual DbSet<FavoriteCategory> FavoriteCategory { get; set; }
        //public virtual DbSet<FavoritePlatform> FavoritePlatform { get; set; }
        public virtual DbSet<TestGame> TestGame { get; set; }
        //public virtual DbSet<GameCategory> GameCategory { get; set; }
        //public virtual DbSet<GameImage> GameImage { get; set; }
        //public virtual DbSet<GameStatus> GameStatus { get; set; }
        //public virtual DbSet<Gender> Gender { get; set; }
        //public virtual DbSet<Platform> Platform { get; set; }
        //public virtual DbSet<Province> Province { get; set; }
        //public virtual DbSet<Rating> Rating { get; set; }
        //public virtual DbSet<Relationship> Relationship { get; set; }
        //public virtual DbSet<Review> Review { get; set; }
        //public virtual DbSet<UserRole> UserRole { get; set; }
        //public virtual DbSet<WizardsRole> WizardsRole { get; set; }
        public virtual DbSet<TestWizardsUser> TestWizardsUser { get; set; }
    }
}
