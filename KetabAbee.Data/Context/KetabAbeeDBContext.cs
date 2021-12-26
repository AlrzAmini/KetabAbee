using System.Linq;
using KetabAbee.Domain.Models.Ticket;
using KetabAbee.Domain.Models.User;
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

        #endregion

        #region ticket

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAnswer> TicketAnswers { get; set; }

        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var rel in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                rel.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
                .HasQueryFilter(c => !c.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
