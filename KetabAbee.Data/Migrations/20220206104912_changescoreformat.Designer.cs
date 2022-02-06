﻿// <auto-generated />
using System;
using KetabAbee.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KetabAbee.Data.Migrations
{
    [DbContext(typeof(KetabAbeeDBContext))]
    [Migration("20220206104912_changescoreformat")]
    partial class changescoreformat
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KetabAbee.Domain.Models.ContactUs.ContactUs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserIp")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ContactUses");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.ContactUs.NewsEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("NewsEmails");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.ContactUs.NewsLetter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<bool>("IsSend")
                        .HasColumnType("bit");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("NewsLetters");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Order.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFinally")
                        .HasColumnType("bit");

                    b.Property<int>("OrderSum")
                        .HasColumnType("int");

                    b.Property<bool>("SendingProcessIsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Order.OrderDetail", b =>
                {
                    b.Property<int>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("DetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Permission.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParrentId")
                        .HasColumnType("int");

                    b.Property<string>("PermissionTitle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("PermissionId");

                    b.HasIndex("ParrentId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Permission.RolePermission", b =>
                {
                    b.Property<int>("RolePrmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("RolePrmId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abstract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AgeRange")
                        .HasColumnType("int");

                    b.Property<int>("CoverType")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("ImageName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Inventory")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("PagesCount")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int?>("SubGroup2Id")
                        .HasColumnType("int");

                    b.Property<int>("SubGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Writer")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("BookId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SubGroup2Id");

                    b.HasIndex("SubGroupId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.BookScore", b =>
                {
                    b.Property<int>("ScoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("ContentScore")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsScored")
                        .HasColumnType("bit");

                    b.Property<int>("QualityScore")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScoreDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserIp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScoreId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("BookScores");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.FavoriteBook", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LikeId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteBooks");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.InventoryReport", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("ChangeId")
                        .HasColumnType("int");

                    b.Property<int>("ChangeNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("ReportId");

                    b.HasIndex("BookId");

                    b.ToTable("InventoryReports");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.ProductGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<string>("GroupTitle")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("GroupId");

                    b.HasIndex("ParentId");

                    b.ToTable("ProductGroups");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("PublisherId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Ticket.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsReadByAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReadBySender")
                        .HasColumnType("bit");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("TicketPriority")
                        .HasColumnType("int");

                    b.Property<DateTime>("TicketSendDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TicketState")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("TicketId");

                    b.HasIndex("SenderId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Ticket.TicketAnswer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswerBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("AnswerId");

                    b.HasIndex("SenderId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketAnswers");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RoleTitle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("AvatarName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("EmailActivationCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMobileActive")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MobileActivationCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.UserBook", b =>
                {
                    b.Property<int>("UserProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserProductId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBooks");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Wallet.Wallet", b =>
                {
                    b.Property<int>("WalletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<string>("Behalf")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPay")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WalletType")
                        .HasColumnType("int");

                    b.HasKey("WalletId");

                    b.HasIndex("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("NewsEmailNewsLetter", b =>
                {
                    b.Property<int>("NewsEmailsId")
                        .HasColumnType("int");

                    b.Property<int>("NewsLettersId")
                        .HasColumnType("int");

                    b.HasKey("NewsEmailsId", "NewsLettersId");

                    b.HasIndex("NewsLettersId");

                    b.ToTable("NewsEmailNewsLetter");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.ContactUs.ContactUs", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("ContactUsCollection")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Order.Order", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Order.OrderDetail", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Order.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.Products.Book", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Permission.Permission", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Permission.Permission", null)
                        .WithMany("Permissions")
                        .HasForeignKey("ParrentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Permission.RolePermission", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Permission.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.User.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.Book", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.ProductGroup", "Group")
                        .WithMany("Books")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.Products.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.Products.ProductGroup", "SubGroup2")
                        .WithMany("Subs2")
                        .HasForeignKey("SubGroup2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("KetabAbee.Domain.Models.Products.ProductGroup", "SubGroup")
                        .WithMany("Subs1")
                        .HasForeignKey("SubGroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Publisher");

                    b.Navigation("SubGroup");

                    b.Navigation("SubGroup2");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.BookScore", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.Book", "Book")
                        .WithMany("BookScores")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("BookScores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.FavoriteBook", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.Book", "Book")
                        .WithMany("FavoriteBook")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("FavoriteBooks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.InventoryReport", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.Book", "Book")
                        .WithMany("InventoryReports")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.ProductGroup", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.ProductGroup", null)
                        .WithMany("ProductGroups")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Ticket.Ticket", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.User", "Sender")
                        .WithMany("Tickets")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Ticket.TicketAnswer", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.User", "Sender")
                        .WithMany("TicketAnswers")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.Ticket.Ticket", "Ticket")
                        .WithMany("Answers")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Sender");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.UserBook", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.Products.Book", "Book")
                        .WithMany("UserBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("UserBooks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.UserRole", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Wallet.Wallet", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.User.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsEmailNewsLetter", b =>
                {
                    b.HasOne("KetabAbee.Domain.Models.ContactUs.NewsEmail", null)
                        .WithMany()
                        .HasForeignKey("NewsEmailsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KetabAbee.Domain.Models.ContactUs.NewsLetter", null)
                        .WithMany()
                        .HasForeignKey("NewsLettersId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Order.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Permission.Permission", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.Book", b =>
                {
                    b.Navigation("BookScores");

                    b.Navigation("FavoriteBook");

                    b.Navigation("InventoryReports");

                    b.Navigation("OrderDetails");

                    b.Navigation("UserBooks");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.ProductGroup", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("ProductGroups");

                    b.Navigation("Subs1");

                    b.Navigation("Subs2");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Products.Publisher", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.Ticket.Ticket", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("KetabAbee.Domain.Models.User.User", b =>
                {
                    b.Navigation("BookScores");

                    b.Navigation("ContactUsCollection");

                    b.Navigation("FavoriteBooks");

                    b.Navigation("Orders");

                    b.Navigation("TicketAnswers");

                    b.Navigation("Tickets");

                    b.Navigation("UserBooks");

                    b.Navigation("UserRoles");

                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
