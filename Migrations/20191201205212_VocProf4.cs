using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestYourself.Migrations
{
    public partial class VocProf4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vocabularies_Profiles_ProfileId",
                table: "Vocabularies");

            migrationBuilder.DropIndex(
                name: "IX_Vocabularies_ProfileId",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Vocabularies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 52, 12, 566, DateTimeKind.Local).AddTicks(3781),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 45, 37, 619, DateTimeKind.Local).AddTicks(6625));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Vocabularies",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 45, 37, 619, DateTimeKind.Local).AddTicks(6625),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 52, 12, 566, DateTimeKind.Local).AddTicks(3781));

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_ProfileId",
                table: "Vocabularies",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vocabularies_Profiles_ProfileId",
                table: "Vocabularies",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
