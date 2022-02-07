using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changetablecomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "ProductCommentAnswers",
                newName: "AnswerBody");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "ProductComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "AnswerBody",
                table: "ProductCommentAnswers",
                newName: "Body");
        }
    }
}
