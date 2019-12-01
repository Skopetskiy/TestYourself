using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestYourself.Migrations
{
    public partial class VocabularyProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 35, 54, 278, DateTimeKind.Local).AddTicks(4278),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 11, 1, 12, 21, 11, 577, DateTimeKind.Local).AddTicks(1442));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 1, 12, 21, 11, 577, DateTimeKind.Local).AddTicks(1442),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 35, 54, 278, DateTimeKind.Local).AddTicks(4278));
        }
    }
}
