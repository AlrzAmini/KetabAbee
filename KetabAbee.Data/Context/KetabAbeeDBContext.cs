using System.Linq;
using KetabAbee.Domain.Models.Banner;
using KetabAbee.Domain.Models.Blog;
using KetabAbee.Domain.Models.Comment.ProductComment;
using KetabAbee.Domain.Models.ContactUs;
using KetabAbee.Domain.Models.Order;
using KetabAbee.Domain.Models.Permission;
using KetabAbee.Domain.Models.Products;
using KetabAbee.Domain.Models.Products.Exam;
using KetabAbee.Domain.Models.Task;
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

        public DbSet<UserIp> UserIps { get; set; }

        public DbSet<BannedIp> BannedIps { get; set; }

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

        public DbSet<RequestBranch>  RequestBranches { get; set; }

        #endregion

        #region Comment

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductCommentAnswer> ProductCommentAnswers { get; set; }

        #endregion

        #region blog

        public DbSet<Blog> Blogs { get; set; }

        #endregion

        #region Task

        public DbSet<Task> Tasks { get; set; }

        #endregion

        #region Exam

        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        #endregion

        #region banner

        public DbSet<Banner> Banners { get; set; }

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
            
            modelBuilder.Entity<UserRole>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Ticket>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<TicketAnswer>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<UserBook>()
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

            modelBuilder.Entity<Blog>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Task>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Wallet>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Exam>()
                .HasQueryFilter(e => !e.IsDelete);
            modelBuilder.Entity<ExamQuestion>()
                .HasQueryFilter(e => !e.IsDelete);
            modelBuilder.Entity<QuestionAnswer>()
                .HasQueryFilter(e => !e.IsDelete);
            modelBuilder.Entity<CorrectAnswer>()
                .HasQueryFilter(e => !e.IsDelete);

            modelBuilder.Entity<Banner>()
                .HasQueryFilter(e => !e.IsDelete);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
