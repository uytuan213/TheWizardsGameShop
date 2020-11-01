using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TheWizardsGameShop.Models
{
    public partial class TheWizardsGameShopContext : DbContext
    {
        public TheWizardsGameShopContext()
        {
        }

        public TheWizardsGameShopContext(DbContextOptions<TheWizardsGameShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<CreditCard> CreditCard { get; set; }
        public virtual DbSet<FavoriteCategory> FavoriteCategory { get; set; }
        public virtual DbSet<FavoritePlatform> FavoritePlatform { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameCategory> GameCategory { get; set; }
        public virtual DbSet<GameImage> GameImage { get; set; }
        public virtual DbSet<GameStatus> GameStatus { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Platform> Platform { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Relationship> Relationship { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<WizardsRole> WizardsRole { get; set; }
        public virtual DbSet<WizardsUser> WizardsUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=prog3050.daehwa.ca;Initial Catalog=TheWizardsGameShop;Persist Security Info=True;User ID=sa;Password=TheWizards!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.ProvinceCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Street1).IsRequired();

                entity.HasOne(d => d.AddressType)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.AddressTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_AddressType");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.ProvinceCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_Provinces");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_WizardsUser");
            });

            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.Property(e => e.AddressTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.Property(e => e.CardHolder)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreditCardNumber)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Cvc)
                    .IsRequired()
                    .HasColumnName("CVC")
                    .HasMaxLength(4);

                entity.Property(e => e.ExpiryDate)
                    .IsRequired()
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<FavoriteCategory>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GameCategoryId });

                entity.HasOne(d => d.GameCategory)
                    .WithMany(p => p.FavoriteCategory)
                    .HasForeignKey(d => d.GameCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCategory_GameCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteCategory)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteCategory_Users");
            });

            modelBuilder.Entity<FavoritePlatform>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PlatformId });

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.FavoritePlatform)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoritePlatform_Platform");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoritePlatform)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoritePlatform_Users");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameDigitalPath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.GamePrice).HasColumnType("smallmoney");

                entity.Property(e => e.GameQty).HasColumnName("GameQTY");

                entity.Property(e => e.GameStatusCode)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.GameCategory)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.GameCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_GameCategory");

                entity.HasOne(d => d.GamePlatform)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.GamePlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Platform");

                entity.HasOne(d => d.GameStatusCodeNavigation)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.GameStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_GameStatus");
            });

            modelBuilder.Entity<GameCategory>(entity =>
            {
                entity.Property(e => e.GameCategory1)
                    .IsRequired()
                    .HasColumnName("GameCategory")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<GameImage>(entity =>
            {
                entity.Property(e => e.GameImagePath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameImage)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameImage_Game");
            });

            modelBuilder.Entity<GameStatus>(entity =>
            {
                entity.HasKey(e => e.GameStatusCode);

                entity.Property(e => e.GameStatusCode).HasMaxLength(1);

                entity.Property(e => e.GameStatus1)
                    .IsRequired()
                    .HasColumnName("GameStatus")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.Gender1);

                entity.Property(e => e.Gender1)
                    .HasColumnName("Gender")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode)
                    .HasName("PK_Provinces_1");

                entity.Property(e => e.ProvinceCode).HasMaxLength(2);

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_Users");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.HasKey(e => new { e.UserId1, e.UserId2 });

                entity.HasOne(d => d.UserId1Navigation)
                    .WithMany(p => p.RelationshipUserId1Navigation)
                    .HasForeignKey(d => d.UserId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Relationship_Users");

                entity.HasOne(d => d.UserId2Navigation)
                    .WithMany(p => p.RelationshipUserId2Navigation)
                    .HasForeignKey(d => d.UserId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Relationship_Users1");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewContent).IsRequired();

                entity.Property(e => e.ReviewDate).HasColumnType("date");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Users");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_UserRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_WizardsRole");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_WizardsUser");
            });

            modelBuilder.Entity<WizardsRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK_Roles");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<WizardsUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Users");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany(p => p.WizardsUser)
                    .HasForeignKey(d => d.Gender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
