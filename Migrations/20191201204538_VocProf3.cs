using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestYourself.Migrations
{
    public partial class VocProf3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 45, 37, 619, DateTimeKind.Local).AddTicks(6625),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 39, 27, 744, DateTimeKind.Local).AddTicks(288));

            migrationBuilder.CreateTable(
                name: "VocabularyProfiles",
                columns: table => new
                {
                    VocabularyProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VocabularyId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyProfiles", x => x.VocabularyProfileId);
                    table.ForeignKey(
                        name: "FK_VocabularyProfiles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VocabularyProfiles_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "VocabularyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyProfiles_ProfileId",
                table: "VocabularyProfiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyProfiles_VocabularyId",
                table: "VocabularyProfiles",
                column: "VocabularyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocabularyProfiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 1, 22, 39, 27, 744, DateTimeKind.Local).AddTicks(288),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 1, 22, 45, 37, 619, DateTimeKind.Local).AddTicks(6625));
        }
    }
}
