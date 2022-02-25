using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changetbltask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_UserRoles_UserRoleId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "UserRoleId",
                table: "Tasks",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserRoleId",
                table: "Tasks",
                newName: "IX_Tasks_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Roles_RoleId",
                table: "Tasks",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Roles_RoleId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Tasks",
                newName: "UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_RoleId",
                table: "Tasks",
                newName: "IX_Tasks_UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_UserRoles_UserRoleId",
                table: "Tasks",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
