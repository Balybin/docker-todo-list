using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DockerTodoList.Infrastructure.Database.Migrations
{
    public partial class FixUserFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                delete from ""TodoListItem"" where ""UserId"" is null
            ");
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListItem_Users_UserId",
                table: "TodoListItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoListItem",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListItem_Users_UserId",
                table: "TodoListItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListItem_Users_UserId",
                table: "TodoListItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoListItem",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListItem_Users_UserId",
                table: "TodoListItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
