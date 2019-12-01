using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestYourself.Migrations
{
    public partial class VocabularyProfile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Vocabularies_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 39, 27, 744, DateTimeKind.Local).AddTicks(288),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 35, 54, 278, DateTimeKind.Local).AddTicks(4278));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Vocabularies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 35, 54, 278, DateTimeKind.Local).AddTicks(4278),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 39, 27, 744, DateTimeKind.Local).AddTicks(288));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Vocabularies_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "Vocabularies",
                principalColumn: "VocabularyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
