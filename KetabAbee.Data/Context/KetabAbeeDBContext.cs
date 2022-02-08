using System.Linq;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.ContactUs;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Permission;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.Ticket;
using KetabAbee.Domain.Models.User;
using KetabAbee.Domain.Models.Wallet;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Context
{
    public class KetabAbeeDBContext : DbContext
    {
        public KetabAbeeDBContext(DbContextOptions<KetabAbeeDBContext> option) : base(option)
        {

        }

        #region User

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserBook> UserBooks { get; set; }

        #endregion

        #region ticket

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAnswer> TicketAnswers { get; set; }

        #endregion

        #region wallet

        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region Products

        public DbSet<ProductGroup> ProductGroups { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<InventoryReport> InventoryReports { get; set; }

        public DbSet<FavoriteBook> FavoriteBooks { get; set; }

        public DbSet<BookScore> BookScores { get; set; }

        public DbSet<ProductDiscount> ProductDiscounts { get; set; }

        public DbSet<ProductDiscountUsage> ProductDiscountUsages { get; set; }

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails  { get; set; }

        #endregion

        #region Contact

        public DbSet<NewsEmail> NewsEmails { get; set; }

        public DbSet<ContactUs> ContactUses { get; set; }

        public DbSet<NewsLetter>  NewsLetters { get; set; }

        #endregion

        #region Comment

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductCommentAnswer> ProductCommentAnswers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var rel in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                rel.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region query filters

            modelBuilder.Entity<User>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<ProductGroup>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Publisher>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Book>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<FavoriteBook>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Order>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<OrderDetail>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<NewsEmail>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<ContactUs>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<BookScore>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<ProductComment>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<ProductCommentAnswer>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<ProductDiscount>()
                .HasQueryFilter(e => !e.IsDelete);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
