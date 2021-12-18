using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changetblticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TicketTitle",
                table: "Tickets",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TicketBody",
                table: "Tickets",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "Tickets",
                newName: "IsReadBySender");

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByAdmin",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TicketPriority",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketState",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadByAdmin",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketPriority",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketState",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tickets",
                newName: "TicketTitle");

            migrationBuilder.RenameColumn(
                name: "IsReadBySender",
                table: "Tickets",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Tickets",
                newName: "TicketBody");
        }
    }
}
