using System.Linq;
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

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails  { get; set; }

        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var rel in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                rel.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region query filters

            modelBuilder.Entity<User>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<ProductGroup>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<Publisher>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<Book>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<FavoriteBook>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<Order>()
                .HasQueryFilter(c => !c.IsDelete);

            modelBuilder.Entity<OrderDetail>()
                .HasQueryFilter(c => !c.IsDelete);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
